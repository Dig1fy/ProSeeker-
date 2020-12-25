namespace ProSeeker.Services.Data.Tests.Cities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Cities;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Cities;
    using Xunit;

    public sealed class CitiesServiceTests : IDisposable
    {
        private readonly ICitiesService citiesService;

        private ApplicationDbContext dbContext;
        private List<City> cities;

        public CitiesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(CitySimpleViewModel).Assembly);

            this.cities = new List<City>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new ApplicationDbContext(options);
            var citiesRepository = new EfRepository<City>(this.dbContext);

            this.citiesService = new CitiesService(citiesRepository);
            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task ShouldReturnAllCitiesCorrectly()
        {
            var allCities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
            var expecedCitiesCount = 3;
            var actualCitiesCount = allCities.Count();

            Assert.Equal(expecedCitiesCount, actualCitiesCount);
        }

        [Fact]
        public async Task ShouldReturnCitiesWithCorrectData()
        {
            var allCities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
            var firstCity = allCities.FirstOrDefault();

            Assert.Equal("Pazardzhik", firstCity.Name);
            Assert.Equal(3, firstCity.Id);
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

            this.dbContext.AddRange(this.cities);
            this.dbContext.SaveChanges();
        }
    }
}
