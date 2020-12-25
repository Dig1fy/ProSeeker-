namespace ProSeeker.Services.Data.Tests.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Home;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Home;
    using Xunit;

    public sealed class HomeServiceTests : IDisposable
    {
        private readonly IHomeService service;

        private ApplicationDbContext dbContext;
        private List<BaseJobCategory> categories;

        public HomeServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(BaseJobCategoryViewModel).Assembly);

            this.categories = new List<BaseJobCategory>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new ApplicationDbContext(options);
            var categoriesRepository = new EfDeletableEntityRepository<BaseJobCategory>(this.dbContext);

            this.service = new HomeService(categoriesRepository);
            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task ShouldReturnAllBaseCategoriesCorrectly()
        {
            var expectedCount = 3;
            var allCategories = await this.service.GetAllBaseCategoriesAsync<BaseJobCategoryViewModel>();

            Assert.Equal(expectedCount, allCategories.Count());
        }

        [Fact]
        public async Task DataShouldRemainUnchanged()
        {
            var allCategories = await this.service.GetAllBaseCategoriesAsync<BaseJobCategoryViewModel>();

            Assert.Contains(allCategories, x => x.CategoryName == "For your new home");
            Assert.Contains(allCategories, x => x.CategoryName == "For your car");
            Assert.Contains(allCategories, x => x.CategoryName == "Others");
        }

        private void InitializeRepositoriesData()
        {
            this.categories.AddRange(new List<BaseJobCategory>()
            {
                new BaseJobCategory { CategoryName = "For your car", Id = 1, Description = "Everything", },
                new BaseJobCategory { CategoryName = "For your new home", Id = 2, Description = "Everything", },
                new BaseJobCategory { CategoryName = "Others", Id = 3, Description = "Nothing", },
            });

            this.dbContext.AddRange(this.categories);
            this.dbContext.SaveChanges();
        }

        public void Dispose()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }
    }
}
