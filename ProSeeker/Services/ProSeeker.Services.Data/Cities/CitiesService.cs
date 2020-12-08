namespace ProSeeker.Services.Data.Cities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<T>> GetAllCitiesAsync<T>()
        {
            var cities = await this.citiesRepository
                .All()
                .OrderBy(c => c.Name)
                .To<T>()
                .ToListAsync();

            return cities;
        }
    }
}
