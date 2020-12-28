namespace ProSeeker.Services.Data.Tests.Categories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Categories;
    using Xunit;

    public sealed class CategoriesServiceTests : BaseServiceTests
    {
        private readonly ICategoriesService service;

        private List<JobCategory> categories;
        private List<Ad> ads;
        private List<Offer> offers;

        public CategoriesServiceTests()
        {
            this.ads = new List<Ad>();
            this.offers = new List<Offer>();
            this.categories = new List<JobCategory>();

            var adsRepository = new EfDeletableEntityRepository<Ad>(this.DbContext);
            var offersRepository = new EfRepository<Offer>(this.DbContext);
            var categoriesRepository = new EfDeletableEntityRepository<JobCategory>(this.DbContext);
            var specialistssRepository = new EfDeletableEntityRepository<Specialist_Details>(this.DbContext);
            this.service = new CategoriesService(categoriesRepository, adsRepository, offersRepository, specialistssRepository);
            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task GetAllCategoriesShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(CategoriesViewModel).Assembly);
            var expecedCategoriesCount = 3;
            var actualCategories = await this.service.GetAllCategoriesAsync<CategoriesViewModel>();

            Assert.Equal(expecedCategoriesCount, actualCategories.Count());
        }

        [Fact]
        public async Task ShouldReturnCorrectCategoryByGivenId()
        {
            AutoMapperConfig.RegisterMappings(typeof(CategoriesViewModel).Assembly);
            var id = 1;

            var expectedCategory = new CategoriesViewModel
            {
                Id = 1,
                Name = "Архитект",
                Description = "лоши хора",
                PictureUrl = "archPicture",
            };

            var actualCategory = await this.service.GetByIdAsync<CategoriesViewModel>(id);

            Assert.Equal(expectedCategory.Id, actualCategory.Id);
            Assert.Equal(expectedCategory.Name, actualCategory.Name);
            Assert.Equal(expectedCategory.PictureUrl, actualCategory.PictureUrl);
        }

        [Fact]
        public async Task ShouldReturnCorrectCategoryNameByGivenOfferId()
        {
            var offerId = "1";
            var expectedCategoryName = "Архитект";
            var actualCategoryName = await this.service.GetCategoryNameByOfferIdAsync(offerId);

            Assert.Equal(expectedCategoryName, actualCategoryName);
        }

        [Fact]
        public async Task ShouldReturnCorrectNumberOfCategoriesByGivenJobCategoryId()
        {
            var baseJobCategoryId = 2;
            var categoriesCount = await this.service.GetCategiesCountInBaseJobCategoryAsync(baseJobCategoryId);

            var expectedCount = 2;

            Assert.Equal(expectedCount, categoriesCount);
        }

        [Fact]
        public async Task ShouldCreateCategoryCorrectly()
        {
            var inputModel = new CategoryInputModel
            {
                BaseJobCategoryId = 1,
                Description = "test",
                Name = "TEST",
            };

            var newCategoryId = await this.service.CreateAsync(inputModel);

            Assert.Equal(4, newCategoryId);
        }

        [Fact]
        public async Task ShouldUpdateTheCategoryCorrectly()
        {
            var inputModel = new CategoryInputModel
            {
                Id = 1,
                BaseJobCategoryId = 1,
                Description = "test",
                Name = "TEST",
                PictureUrl = "xxx",
            };

            await this.service.UpdateAsync(inputModel);
            var newCategoryPicture = await this.service.GetCategoryPictureByCategoryId(1);
            var expectedNewCategoryPicture = "xxx";

            Assert.Equal(expectedNewCategoryPicture, newCategoryPicture);
        }

        [Fact]
        public async Task ShouldReturnCorrectNumberOfSpecialistsPerGivenCategory()
        {
            var categoryIdWithZeroSpecialists = 1;
            var categoryIdWithOneSpecialists = 3;

            var actualResultOfFirstCategory = await this.service.GetSpecialistsCountInCategoryAsync(categoryIdWithZeroSpecialists);
            var actualResultOfThirdCategory = await this.service.GetSpecialistsCountInCategoryAsync(categoryIdWithOneSpecialists);

            var expectedNumberOfSpecialistInFirstCategory = 0;
            var expectedNumberOfSpecialistInThirdCategory = 1;

            Assert.Equal(expectedNumberOfSpecialistInFirstCategory, actualResultOfFirstCategory);
            Assert.Equal(expectedNumberOfSpecialistInThirdCategory, actualResultOfThirdCategory);
        }

        [Fact]
        public async Task ShouldDeleteCategoryProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(CategoriesViewModel).Assembly);
            var categoryId = 1;

            await this.service.DeleteByIdAsync(categoryId);
            var deletedCategory = await this.service.GetByIdAsync<CategoriesViewModel>(categoryId);

            Assert.Null(deletedCategory);
        }



        private void InitializeRepositoriesData()
        {
            this.categories.AddRange(new List<JobCategory>()
            {
                new JobCategory { Id = 1, BaseJobCategoryId = 1, Name = "Архитект", Description = "лоши хора", PictureUrl = "archPicture", CreatedOn = DateTime.UtcNow, IsDeleted = false },
                new JobCategory { Id = 2, BaseJobCategoryId = 2, Name = "Брокер", Description = "по-лоши хора", PictureUrl = "realEstateAgentPicture", CreatedOn = DateTime.UtcNow, IsDeleted = false },
                new JobCategory { Id = 3, BaseJobCategoryId = 2, Name = "Урбанист", Description = "иновативни хора", PictureUrl = "urbanistPicure", CreatedOn = DateTime.UtcNow, IsDeleted = false, SpecialistsDetails = new List<Specialist_Details> { new Specialist_Details { Id = "555" } } },
            });

            this.ads.AddRange(new List<Ad>
            {
                new Ad { Id = "1", CityId = 1, Description = "Търся архитект", JobCategoryId = 1, PreparedBudget = "Достатъчно", Title = "Спешно", IsVip = false, UserId = "1", CreatedOn = DateTime.UtcNow },
                new Ad { Id = "2", CityId = 2, Description = "Търся брокер", JobCategoryId = 2, PreparedBudget = "Достатъчно", Title = "Спешно", IsVip = false, UserId = "2" },
                new Ad { Id = "3", CityId = 1, Description = "И аз търся архитект", JobCategoryId = 1, PreparedBudget = "Достатъчно", Title = "Спешно", IsVip = false, UserId = "2", CreatedOn = DateTime.UtcNow.AddMinutes(5) },
            });

            this.offers.AddRange(new List<Offer>
            {
                new Offer { Id = "1", AdId = "1", Description = "Предлагам услуга", ApplicationUserId = "1", InquiryId = null, Price = 200, StartDate = "Скоро", CreatedOn = DateTime.UtcNow, SpecialistDetailsId = "1", ExpirationDate = DateTime.UtcNow.AddDays(5), },
                new Offer { Id = "2", AdId = "2", Description = "Ще ти дам пари", ApplicationUserId = "1", InquiryId = null, Price = 200, StartDate = "Скоро", CreatedOn = DateTime.UtcNow, SpecialistDetailsId = "1", ExpirationDate = DateTime.UtcNow.AddDays(5), },
                new Offer { Id = "3", AdId = null, Description = "Предлагам да започваме", ApplicationUserId = "1", InquiryId = "1", Price = 5500, CreatedOn = DateTime.UtcNow.AddMinutes(5), SpecialistDetailsId = "1", ExpirationDate = DateTime.UtcNow.AddDays(5), },
            });

            this.DbContext.AddRange(this.categories);
            this.DbContext.AddRange(this.ads);
            this.DbContext.AddRange(this.offers);
            this.DbContext.SaveChanges();
        }
    }
}
