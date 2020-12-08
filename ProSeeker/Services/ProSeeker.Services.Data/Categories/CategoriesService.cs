namespace ProSeeker.Services.Data.CategoriesService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<T>> GetAllCategoriesAsync<T>()
        {
            var allCategories = await this.categoriesRepository
                .AllAsNoTracking()
                .To<T>()
                .ToListAsync();

            return allCategories;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var category = await this.categoriesRepository
                 .All()
                 .Where(x => x.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();

            return category;
        }
    }
}
