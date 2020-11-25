namespace ProSeeker.Services.Data.CategoriesService
{
    using System.Collections.Generic;
    using System.Linq;

    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<JobCategory> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<JobCategory> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<T> GetAllCategories<T>()
        {
            var allCategories = this.categoriesRepository
                .All()
                .To<T>()
                .ToList();

            return allCategories;
        }

        public T GetById<T>(int id)
        {
            var category = this.categoriesRepository
                 .All()
                 .Where(x => x.Id == id)
                 .To<T>()
                 .FirstOrDefault();

            return category;
        }
    }
}
