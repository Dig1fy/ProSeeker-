namespace ProSeeker.Services.Data.Tests.Ads
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Ads;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Ads;
    using Xunit;

    public sealed class AdsServiceTests : IDisposable
    {
        private readonly IAdsService service;

        private ApplicationDbContext dbContext;

        private List<ApplicationUser> users;
        private List<Ad> ads = new List<Ad>();
        private List<City> cities;
        private List<JobCategory> jobCategories;

        public AdsServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(CreateAdInputModel).Assembly);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new ApplicationDbContext(options);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(this.dbContext);
            var citiesRepository = new EfRepository<City>(this.dbContext);
            var adsRepository = new EfDeletableEntityRepository<Ad>(this.dbContext);
            var categoriesRepository = new EfDeletableEntityRepository<JobCategory>(this.dbContext);
            this.service = new AdsService(adsRepository, usersRepository, citiesRepository, categoriesRepository);

            this.users = new List<ApplicationUser>();
            this.ads = new List<Ad>();
            this.cities = new List<City>();
            this.jobCategories = new List<JobCategory>();

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task CreateShouldAddNewAdCorrectly()
        {
            var userId = "3";
            var inputModel = this.GetInputModel();
            await this.service.CreateAsync(inputModel, userId);

            var expectedCount = 1;
            var countResult = await this.service.GetAdsCountByUserIdAsync(userId);
            Assert.Equal(expectedCount, countResult);
        }

        [Fact]
        public async Task CountOfUserAdsShouldIncrementProperly()
        {
            var userId = "3";
            var expectedResult = 5;
            var inputModel = this.GetInputModel();

            await this.service.CreateAsync(inputModel, userId);
            await this.service.CreateAsync(inputModel, userId);
            await this.service.CreateAsync(inputModel, userId);
            await this.service.CreateAsync(inputModel, userId);
            await this.service.CreateAsync(inputModel, userId);
            var actualCountResult = await this.service.GetAdsCountByUserIdAsync(userId);
            Assert.Equal(expectedResult, actualCountResult);
        }

        [Fact]
        public async Task MakeUserAdsVipShouldChangeTheStatusOfHisAdsOnly()
        {
            var firstUserId = "1";
            var secondUserId = "2";

            await this.service.MakeAdsVipAsync(secondUserId);
            var vipAds = await this.service.GetMyAdsAsync<AdsShortDetailsViewModel>(secondUserId, 0);
            var normalAds = await this.service.GetMyAdsAsync<AdsShortDetailsViewModel>(firstUserId, 0);

            var areAllUsersAdsVip = vipAds.All(x => x.IsVip == true);
            var areOtherAdsNormal = normalAds.All(x => x.IsVip == false);

            Assert.True(areAllUsersAdsVip);
            Assert.True(areOtherAdsNormal);
        }

        [Fact]
        public async Task AllAdsCountShouldReturnCorrectValue()
        {
            var userId = "1";
            var expectedResult = 6;
            var inputModel = this.GetInputModel();

            await this.service.CreateAsync(inputModel, userId);
            await this.service.CreateAsync(inputModel, userId);
            await this.service.CreateAsync(inputModel, userId);
            var actualResult = await this.service.AllAdsCountAsync();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ShouldReturnCorrectNumberOfListedAds()
        {
            var categoryName = "Архитект";
            var cityId = 1;
            var emptyCityId = 0;
            var firstCount = await this.service.AllAdsByCategoryCountAsync(categoryName, cityId);
            var counWithoutCityId = await this.service.AllAdsByCategoryCountAsync(categoryName, emptyCityId);

            Assert.Equal(2, firstCount);
        }

        [Fact]
        public async Task AdsByUserShouldReturnCorrectCount()
        {
            var userId = "2";

            var userAds = await this.service.GetMyAdsAsync<AdsShortDetailsViewModel>(userId, 0);
            var expectedAdsCount = 2;
            var actual = userAds.Count();

            Assert.Equal(expectedAdsCount, actual);
        }

        [Fact]
        public async Task GetAddDetailsShouldMapGenericToViewModel()
        {
            var desiredAdId = "1";

            var viewModel = await this.service.GetAdDetailsByIdAsync<CreateAdInputModel>(desiredAdId);
            Assert.Equal("1", viewModel.UserId);
            Assert.Equal(1, viewModel.JobCategoryId);
        }

        [Fact]
        public async Task ShouldReturnCorrectUserId()
        {
            var expectedUserId = "2";

            var desiredAdId = "3";
            var actualUserId = await this.service.GetUserIdByAdIdAsync(desiredAdId);

            Assert.Equal(expectedUserId, actualUserId);
        }

        [Fact]
        public async Task DeleteAdByIdShouldWorkCorrectly()
        {
            var desiredAdToDelete = "2";

            await this.service.DeleteByIdAsync(desiredAdToDelete);

            var result = await this.service.GetAdDetailsByIdAsync<CreateAdInputModel>(desiredAdToDelete);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateShouldAdjustTheAdCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(UpdateInputModel).Assembly);
            var newDescription = "Бих искал да си наема кон!";
            var newTitle = "Търся брокер за отдаване под наем";
            var newCategoryId = 2;
            var newBudget = "500 лева";

            var model = new UpdateInputModel
            {
                Id = "2",
                Description = newDescription,
                Title = newTitle,
                JobCategoryId = newCategoryId,
                PreparedBudget = newBudget,
                CityId = 1,
                UserId = "1",
            };

            var desiredAdId = "2";
            await this.service.UpdateAdAsync(model);
            var updatedAd = await this.service.GetAdDetailsByIdAsync<UpdateInputModel>(desiredAdId);

            Assert.Equal(newBudget, updatedAd.PreparedBudget);
            Assert.Equal(newTitle, updatedAd.Title);
            Assert.Equal(newDescription, updatedAd.Description);
            Assert.Equal(newCategoryId, updatedAd.JobCategoryId);
        }

        [Fact]
        public async Task GetByCategoryShouldReturnCoorectCountOfAds()
        {
            AutoMapperConfig.RegisterMappings(typeof(AdsShortDetailsViewModel).Assembly);
            var categoryName = "Архитект";
            var cityId = 1;

            var allAdsByCategory = await this.service.GetByCategoryAsync<AdsShortDetailsViewModel>(categoryName, null, cityId, 0);

            Assert.Equal(2, allAdsByCategory.Count());
        }

        [Fact]
        public async Task GetByCategoryShouldHaveAccurateDefaultSorting()
        {
            AutoMapperConfig.RegisterMappings(typeof(AdsShortDetailsViewModel).Assembly);
            var categoryName = "Архитект";
            var cityId = 1;

            var allAdsByCategory = await this.service.GetByCategoryAsync<AdsShortDetailsViewModel>(categoryName, null, cityId, 0);
            var firstAdId = allAdsByCategory.First().Id;

            Assert.Equal("3", firstAdId);
        }

        public void Dispose()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        private void InitializeRepositoriesData()
        {
            this.cities.AddRange(new List<City>()
            {
                new City { Name = "Sofia", Id = 1, },
                new City { Name = "Plovdiv", Id = 2, },
                new City { Name = "Pazardzhik", Id = 3, },
            });

            this.jobCategories.AddRange(new List<JobCategory>()
            {
                new JobCategory { Id = 1, Name = "Архитект", Description = "лоши хора", PictureUrl = "archPicture", },
                new JobCategory { Id = 2, Name = "Брокер", Description = "по-лоши хора", PictureUrl = "realEstateAgentPicture", },
                new JobCategory { Id = 3, Name = "Урбанист", Description = "иновативни хора", PictureUrl = "urbanistPicure", },
            });

            this.ads.AddRange(new List<Ad>
            {
                new Ad { Id = "1", CityId = 1, Description = "Търся архитект", JobCategoryId = 1, PreparedBudget = "Достатъчно", Title = "Спешно", IsVip = false, UserId = "1", CreatedOn = DateTime.UtcNow },
                new Ad { Id = "2", CityId = 2, Description = "Търся брокер", JobCategoryId = 2, PreparedBudget = "Достатъчно", Title = "Спешно", IsVip = false, UserId = "2" },
                new Ad { Id = "3", CityId = 1, Description = "И аз търся архитект", JobCategoryId = 1, PreparedBudget = "Достатъчно", Title = "Спешно", IsVip = false, UserId = "2", CreatedOn = DateTime.UtcNow.AddMinutes(5) },
            });

            this.users.AddRange(new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", FirstName = "Ivo", LastName = "Ivov", CityId = 1, Email = "u@u", ProfilePicture = "SomeProfilePicture", IsSpecialist = false },
                new ApplicationUser { Id = "2", FirstName = "Gosho", LastName = "Goshev", CityId = 1, Email = "s@s", ProfilePicture = "SpecProfilePicture", IsSpecialist = true, SpecialistDetailsId = "specialistId" },
            });

            this.dbContext.AddRange(this.users);
            this.dbContext.AddRange(this.cities);
            this.dbContext.AddRange(this.jobCategories);
            this.dbContext.AddRange(this.ads);
            this.dbContext.SaveChanges();
        }

        private CreateAdInputModel GetInputModel()
        {
            return new CreateAdInputModel
            {
                JobCategoryId = 1,
                CityId = 1,
                Opinions = new List<Opinion> { new Opinion { AdId = "spec", Content = "Здравей", SpecialistDetailsId = "1", CreatorId = "1" } },
                Title = "Търся брокер",
                PreparedBudget = "a lot",
                Description = "Нуждая се от имот",
            };
        }
    }
}
