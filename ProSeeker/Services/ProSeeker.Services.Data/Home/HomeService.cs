namespace ProSeeker.Services.Data.Home
{
    using System.Collections.Generic;
    using System.Linq;

    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class HomeService : IHomeService
    {
        // Our services will work either with the corresponding repository or with the DbContext
        private readonly IDeletableEntityRepository<BaseJobCategory> categoriesRepository;

        public HomeService(IDeletableEntityRepository<BaseJobCategory> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<T> GetAllBaseCategories<T>(int? count = null)
        {
            var query = this.categoriesRepository.All();

            // Check if the controller needs a specific count of entities from the db (in case we need pagination)
            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            // We let the controller take the decision for "To<T>"
            return query.To<T>().ToList();
        }
    }
}
