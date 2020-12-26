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
            this.service = new CategoriesService(categoriesRepository, adsRepository, offersRepository);
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

        private void InitializeRepositoriesData()
        {
            this.categories.AddRange(new List<JobCategory>()
            {
                new JobCategory { Id = 1, Name = "Архитект", Description = "лоши хора", PictureUrl = "archPicture", CreatedOn = DateTime.UtcNow, IsDeleted = false },
                new JobCategory { Id = 2, Name = "Брокер", Description = "по-лоши хора", PictureUrl = "realEstateAgentPicture", CreatedOn = DateTime.UtcNow, IsDeleted = false },
                new JobCategory { Id = 3, Name = "Урбанист", Description = "иновативни хора", PictureUrl = "urbanistPicure", CreatedOn = DateTime.UtcNow, IsDeleted = false },
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
