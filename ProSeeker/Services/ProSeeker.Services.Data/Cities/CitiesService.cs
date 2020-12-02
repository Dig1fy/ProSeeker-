namespace ProSeeker.Services.Data.Cities
{
    using System.Collections.Generic;
    using System.Linq;

    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class CitiesService : ICitiesService
    {
        private readonly IRepository<City> citiesRepository;

        public CitiesService(IRepository<City> citiesRepository)
        {
            this.citiesRepository = citiesRepository;
        }

        public IEnumerable<T> GetAllCities<T>()
        {
            var cities = this.citiesRepository
                .AllAsNoTracking()
                .To<T>()
                .ToList();

            return cities;
        }
    }
}
