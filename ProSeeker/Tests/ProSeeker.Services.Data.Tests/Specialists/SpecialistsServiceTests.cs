namespace ProSeeker.Services.Data.Tests.Specialists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Specialists;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Users;
    using Xunit;

    public sealed class SpecialistsServiceTests : BaseServiceTests
    {
        private readonly ISpecialistsService service;

        private List<Specialist_Details> specialists;

        public SpecialistsServiceTests()
        {
            this.specialists = new List<Specialist_Details>();

            var specialistsRepository = new EfDeletableEntityRepository<Specialist_Details>(this.DbContext);
            var ratingsRepository = new EfRepository<Rating>(this.DbContext);
            this.service = new SpecialistsService(specialistsRepository, ratingsRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task GetAllSpecialistsPerCategoryAsync_ShouldReturnAllSpecialistsPerCategoryProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SpecialistShortDetailsViewModel).Assembly);
            var categoryId = 1;

            var allSpecialistsPerCategory =
                await this.service.GetAllSpecialistsPerCategoryAsync<SpecialistShortDetailsViewModel>(
                    categoryId, null, 0, 0);

            var expectedCount = 3;
            var actualCount = allSpecialistsPerCategory.Count();
            var isCategoryTheSame = allSpecialistsPerCategory.All(x => x.JobCategoryId == categoryId);

            Assert.Equal(expectedCount, actualCount);
            Assert.True(isCategoryTheSame);
        }

        [Fact]
        public async Task GetAllSpecialistsPerCategoryAsync_ShouldReturnAllSpecialistsByGivenCityIdOnly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SpecialistShortDetailsViewModel).Assembly);
            var categoryId = 1;
            var cityId = 1;

            var allSpecialistsPerCategory =
                await this.service.GetAllSpecialistsPerCategoryAsync<SpecialistShortDetailsViewModel>(
                    categoryId, null, cityId, 0);

            var expectedCount = 1;
            var actualCount = allSpecialistsPerCategory.Count();
            var isCategoryTheSame = allSpecialistsPerCategory.All(x => x.JobCategoryId == categoryId);

            Assert.Equal(expectedCount, actualCount);
            Assert.True(isCategoryTheSame);
        }

        [Fact]
        public async Task GetSpecialistsCountByCategoryAsync_ShouldReturnCorrectSpecialistsCountByGivenCategoryId()
        {
            var categoryId = 1;
            var expectedCount = 3;

            var actualCount = await this.service.GetSpecialistsCountByCategoryAsync(categoryId, 0);

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetSpecialistsCountByCategoryAsync_ShouldReturnCorrectSpecialistsCountByGivenCategoryIdAndCityId()
        {
            var categoryId = 1;
            var expectedCount = 1;
            var cityId = 1;

            var actualCount = await this.service.GetSpecialistsCountByCategoryAsync(categoryId, cityId);

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetAllSpecialistsPerCategoryAsync_SortByOpinionsCountShouldWorkProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SpecialistShortDetailsViewModel).Assembly);

            var categoryId = 1;
            var sortBy = "OpinionsCount";
            var cityId = 0;

            var allSpecialistsPerCategory =
                await this.service.GetAllSpecialistsPerCategoryAsync<SpecialistShortDetailsViewModel>(
                    categoryId, sortBy, cityId, 0);

            var listedSpecialists = allSpecialistsPerCategory.ToList();
            var actualFirstSpecialistId = listedSpecialists[0].Id;
            var actualSecondSpecialistId = listedSpecialists[1].Id;
            var expectedFirstSpecialistId = "specialist3";
            var expectedSecondSpecialistId = "specialist2";

            Assert.Equal(expectedFirstSpecialistId, actualFirstSpecialistId);
            Assert.Equal(expectedSecondSpecialistId, actualSecondSpecialistId);
        }

        [Fact]
        public async Task GetAllSpecialistsPerCategoryAsync_ShouldSortSpecialistByGivenCityIdOnly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SpecialistShortDetailsViewModel).Assembly);

            var categoryId = 1;
            var cityId = 1;

            var allSpecialistsPerCategory =
                await this.service.GetAllSpecialistsPerCategoryAsync<SpecialistShortDetailsViewModel>(
                    categoryId, null, cityId, 0);

            Assert.Single(allSpecialistsPerCategory);
        }

        private void InitializeRepositoriesData()
        {
            this.specialists.AddRange(new List<Specialist_Details>
            {
                new Specialist_Details
                {
                    Id = "specialist1",
                    UserId = "1",
                    JobCategoryId = 1,
                },
                new Specialist_Details
                {
                    Id = "specialist2",
                    UserId = "2",
                    JobCategoryId = 1,
                    Opinions = new List<Opinion>
                        {
                        new Opinion { Id = 1, Content = "Hey", CreatorId = "2", },
                        },
                    User = new ApplicationUser
                    {
                        Id = "2",
                        SpecialistDetailsId = "specialist2",
                        CityId = 1,
                    },
                },
                new Specialist_Details
                {
                    Id = "specialist3",
                    UserId = "3",
                    JobCategoryId = 1,
                    Opinions = new List<Opinion>
                        {
                        new Opinion { Id = 2, Content = "Hey", CreatorId = "3", },
                        new Opinion { Id = 3, Content = "Hey", CreatorId = "3", },
                        },
                },
            });

            this.DbContext.AddRange(this.specialists);
            this.DbContext.SaveChanges();
        }
    }
}
