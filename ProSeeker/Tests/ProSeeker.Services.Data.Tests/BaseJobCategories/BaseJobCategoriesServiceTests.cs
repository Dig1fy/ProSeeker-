namespace ProSeeker.Services.Data.Tests.BaseJobCategories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.BaseJobCategories;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.BaseJobCategories;
    using Xunit;

    public sealed class BaseJobCategoriesServiceTests : BaseServiceTests
    {
        private readonly IBaseJobCategoriesService service;

        private List<BaseJobCategory> baseCategories;

        public BaseJobCategoriesServiceTests()
        {
            this.baseCategories = new List<BaseJobCategory>();

            var baseCategoriesRepository = new EfDeletableEntityRepository<BaseJobCategory>(this.DbContext);
            this.service = new BaseJobCategoriesService(baseCategoriesRepository);
            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task ShouldCreateBaseCategoryProperly()
        {
            var inputModel = new BaseJobCategoryInputModel
            {
                CategoryName = "тест",
                Description = "Тествам теста",
            };

            var newCategoryId = await this.service.CreateAsync(inputModel);
            var expectedId = 4;

            Assert.Equal(expectedId, newCategoryId);
        }

        [Fact]
        public async Task ShouldDeleteCategoryByGivenId()
        {
            AutoMapperConfig.RegisterMappings(typeof(SimpleBaseJobCategoryViewModel).Assembly);
            var baseCategoryId = 1;

            await this.service.DeleteByIdAsync(baseCategoryId);
            var getDeletedCategory = await this.service.GetBaseJobCategoryById<SimpleBaseJobCategoryViewModel>(baseCategoryId);

            Assert.Null(getDeletedCategory);
        }

        [Fact]
        public async Task ShouldReturnAllBaseCategoriesCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SimpleBaseJobCategoryViewModel).Assembly);

            var allCategories = await this.service.GetAllBaseCategoriesAsync<SimpleBaseJobCategoryViewModel>();
            var expectedCategoriesCount = 3;

            Assert.Equal(expectedCategoriesCount, allCategories.Count());
            Assert.Equal(1, allCategories.First().Id);
            Assert.Equal(3, allCategories.Last().Id);
        }

        [Fact]
        public async Task UpdateShouldWorkProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SimpleBaseJobCategoryViewModel).Assembly);
            var model = new BaseJobCategoryInputModel
            {
                Id = 1,
                CategoryName = "Test",
                Description = "Making test here and there",
            };

            await this.service.UpdateAsync(model);
            var updatedCategory = await this.service.GetBaseJobCategoryById<SimpleBaseJobCategoryViewModel>(1);
            var expectedId = 1;
            var expectedCategoryName = "Test";
            var expectedDescription = "Making test here and there";

            Assert.Equal(expectedId, updatedCategory.Id);
            Assert.Equal(expectedCategoryName, updatedCategory.CategoryName);
            Assert.Equal(expectedDescription, updatedCategory.Description);
        }

        private void InitializeRepositoriesData()
        {
            this.baseCategories.AddRange(new List<BaseJobCategory>()
            {
                new BaseJobCategory { Id = 1, Description = "Лоши хора", CategoryName = "Брокер",  CreatedOn = DateTime.UtcNow, IsDeleted = false },
                new BaseJobCategory { Id = 2, Description = "Добри хора", CategoryName = "Архитект",  CreatedOn = DateTime.UtcNow, IsDeleted = false },
                new BaseJobCategory { Id = 3, Description = "Истински професионалисти", CategoryName = "Урбанист",  CreatedOn = DateTime.UtcNow, IsDeleted = false },
            });

            this.DbContext.AddRange(this.baseCategories);
            this.DbContext.SaveChanges();
        }
    }
}
