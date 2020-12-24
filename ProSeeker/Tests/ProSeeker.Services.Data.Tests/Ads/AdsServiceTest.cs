namespace ProSeeker.Services.Data.Tests.Ads
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using ProSeeker.Data;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Ads;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels;
    using ProSeeker.Web.ViewModels.Ads;
    using Xunit;

    public class AdsServiceTest : IDisposable
    {
        private readonly IAdsService adsService;
        private CreateAdInputModel inputModel;

        private AdsService service;
        private EfRepository<City> citiesRepository;
        private EfDeletableEntityRepository<ApplicationUser> usersRepository;
        private EfDeletableEntityRepository<Ad> adsRepository;
        private EfDeletableEntityRepository<JobCategory> categoriesRepository;
        private ApplicationDbContext dbContext;

        private List<ApplicationUser> users;
        private List<Ad> ads = new List<Ad>();
        private List<City> cities;
        private List<JobCategory> jobCategories;

        public AdsServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, Assembly.Load("ProSeeker.Services.Data.Tests"));
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new ApplicationDbContext(options);
            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(this.dbContext);
            this.citiesRepository = new EfRepository<City>(this.dbContext);
            this.adsRepository = new EfDeletableEntityRepository<Ad>(this.dbContext);
            this.categoriesRepository = new EfDeletableEntityRepository<JobCategory>(this.dbContext);
            this.service = new AdsService(this.adsRepository, this.usersRepository, this.citiesRepository, this.categoriesRepository);

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
            await this.service.CreateAsync(this.inputModel, userId);
            var count = await this.service.GetAdsCountByUserIdAsync(userId);
            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CountOfUserAdsShouldIncrementProperly()
        {
            var userId = "3";
            await this.service.CreateAsync(this.inputModel, userId);
            await this.service.CreateAsync(this.inputModel, userId);
            await this.service.CreateAsync(this.inputModel, userId);
            await this.service.CreateAsync(this.inputModel, userId);
            await this.service.CreateAsync(this.inputModel, userId);
            var count = await this.service.GetAdsCountByUserIdAsync(userId);
            Assert.Equal(5, count);
        }

        [Fact]
        public async Task MakeUserAdsVipShouldChangeTheStatusOfHisAdsOnly()
        {
            var firstUserId = "1";
            var secondUserId = "2";

            await this.service.MakeAdsVipAsync(secondUserId);
            var secondAd = await this.adsRepository.All().FirstOrDefaultAsync(x => x.UserId == secondUserId);
            var firsAd = await this.adsRepository.All().FirstOrDefaultAsync(x => x.UserId == firstUserId);

            Assert.True(!firsAd.IsVip);
            Assert.True(secondAd.IsVip);
        }

        [Fact]
        public async Task AllAdsCountShouldReturnCorrectValue()
        {
            var userId = "1";
            await this.service.CreateAsync(this.inputModel, userId);
            await this.service.CreateAsync(this.inputModel, userId);
            await this.service.CreateAsync(this.inputModel, userId);
            var allAdsCount = await this.service.AllAdsCountAsync();

            Assert.Equal(6, allAdsCount);
        }

        [Fact]
        public async Task ShouldReturnCorrectNumberOfListedAds()
        {
            var categoryName = "Архитект";
            var cityId = 1;
            var emptyCityId = 0;
            var firstCount = await this.service.AllAdsByCategoryCountAsync(categoryName, cityId);
            var counWithoutCityId = await this.service.AllAdsByCategoryCountAsync(categoryName, emptyCityId);

            Assert.Equal(1, firstCount);
        }

        [Fact]
        public async Task GetAddDetailsShouldMapGenericToViewModel()
        {
            var desiredAdId = "1";
            AutoMapperConfig.RegisterMappings(typeof(CreateAdInputModel).Assembly);

            var viewModel = await this.service.GetAdDetailsByIdAsync<CreateAdInputModel>(desiredAdId);
            Assert.Equal("1", viewModel.UserId);
            Assert.Equal(1, viewModel.JobCategoryId);
        }

        public void Dispose()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        private void InitializeRepositoriesData()
        {
            this.inputModel = new CreateAdInputModel
            {
                JobCategoryId = 1,
                CityId = 1,
                Opinions = new List<Opinion> { new Opinion { AdId = "spec", Content = "Здравей", SpecialistDetailsId = "1", CreatorId = "1" } },
                Title = "Търся брокер",
                PreparedBudget = "a lot",
                Description = "Нуждая се от имот",
            };

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
                new Ad { Id = "1", CityId = 1, Description = "Търся архитект", JobCategoryId = 1, PreparedBudget = "Достатъчно", Title = "Спешно", IsVip = false, UserId = "1" },
                new Ad { Id = "2", CityId = 2, Description = "Търся брокер", JobCategoryId = 2, PreparedBudget = "Достатъчно", Title = "Спешно", IsVip = false, UserId = "2" },
                new Ad { Id = "3", CityId = 3, Description = "Търся урбанист", JobCategoryId = 3, PreparedBudget = "Достатъчно", Title = "Спешно", IsVip = false, UserId = "2" },
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

    }
}

