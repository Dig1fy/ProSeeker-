namespace ProSeeker.Services.Data.CategoriesService
{
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

        public T GetByName<T>(string name)
        {
            var category = this.categoriesRepository
                 .All()
                 .Where(x => x.Name == name)
                 .To<T>()
                 .FirstOrDefault();

            return category;
        }
    }
}
