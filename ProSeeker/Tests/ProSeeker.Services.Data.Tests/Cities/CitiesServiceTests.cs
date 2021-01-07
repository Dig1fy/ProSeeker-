namespace ProSeeker.Services.Data.Tests.Cities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Cities;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Cities;
    using Xunit;

    public sealed class CitiesServiceTests : BaseServiceTests
    {
        private readonly ICitiesService citiesService;

        private List<City> cities;

        public CitiesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(CitySimpleViewModel).Assembly);

            this.cities = new List<City>();

            var citiesRepository = new EfRepository<City>(this.DbContext);

            this.citiesService = new CitiesService(citiesRepository);
            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task GetAllCitiesAsync_ShouldReturnAllCitiesCorrectly()
        {
            var allCities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
            var expecedCitiesCount = 3;
            var actualCitiesCount = allCities.Count();

            Assert.Equal(expecedCitiesCount, actualCitiesCount);
        }

        [Fact]
        public async Task GetAllCitiesAsync_ShouldReturnCitiesWithCorrectData()
        {
            var allCities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
            var firstCity = allCities.FirstOrDefault();

            Assert.Equal("Pazardzhik", firstCity.Name);
            Assert.Equal(3, firstCity.Id);
        }

        private void InitializeRepositoriesData()
        {
            this.cities.AddRange(new List<City>()
            {
                new City { Name = "Sofia", Id = 1, },
                new City { Name = "Plovdiv", Id = 2, },
                new City { Name = "Pazardzhik", Id = 3, },
            });

            this.DbContext.AddRange(this.cities);
            this.DbContext.SaveChanges();
        }
    }
}
