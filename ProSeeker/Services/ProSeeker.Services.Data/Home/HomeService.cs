namespace ProSeeker.Services.Data.Home
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class HomeService : IHomeService
    {
        // Our services will work either with the corresponding repository or with the DbContext
        private readonly IDeletableEntityRepository<BaseJobCategory> baseCategoriesRepository;

        public HomeService(IDeletableEntityRepository<BaseJobCategory> baseCategoriesRepository)
        {
            this.baseCategoriesRepository = baseCategoriesRepository;
        }

        public async Task<IEnumerable<T>> GetAllBaseCategoriesAsync<T>(int? count = null)
        {
            var query = await this.baseCategoriesRepository
                .AllAsNoTracking()
                .To<T>()
                .ToListAsync();

            return query;
        }
    }
}
