namespace ProSeeker.Services.Data.BaseJobCategories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.BaseJobCategories;

    public class BaseJobCategoriesService : IBaseJobCategoriesService
    {
        private readonly IDeletableEntityRepository<BaseJobCategory> baseJobCategoriesRepository;

        public BaseJobCategoriesService(IDeletableEntityRepository<BaseJobCategory> baseJobCategoriesRepository)
        {
            this.baseJobCategoriesRepository = baseJobCategoriesRepository;
        }

        public async Task<int> CreateAsync(BaseJobCategoryInputModel inputModel)
        {
            var newBaseJobCategory = new BaseJobCategory
            {
                Description = inputModel.Description,
                CategoryName = inputModel.CategoryName,
            };

            await this.baseJobCategoriesRepository.AddAsync(newBaseJobCategory);
            await this.baseJobCategoriesRepository.SaveChangesAsync();

            return newBaseJobCategory.Id;
        }

        public async Task DeleteByIdAsync(int baseCategoryId)
        {
            var category = await this.baseJobCategoriesRepository
                .AllAsNoTrackingWithDeleted()
                .FirstOrDefaultAsync(x => x.Id == baseCategoryId);

            if (category == null)
            {
                throw new ArgumentNullException();
            }

            this.baseJobCategoriesRepository.Delete(category);
            await this.baseJobCategoriesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllBaseCategoriesAsync<T>()
        {
            var allBaseCategories = await this.baseJobCategoriesRepository
                .All()
                .To<T>()
                .ToListAsync();

            return allBaseCategories;
        }

        public async Task<T> GetBaseJobCategoryById<T>(int baseCategoryId)
        {
            var category = await this.baseJobCategoriesRepository
                .All()
                .Where(x => x.Id == baseCategoryId)
                .To<T>()
                .FirstOrDefaultAsync();

            return category;
        }

        public async Task UpdateAsync(BaseJobCategoryInputModel inputModel)
        {
            var category = await this.baseJobCategoriesRepository.All().FirstOrDefaultAsync(x => x.Id == inputModel.Id);

            if (category == null)
            {
                throw new ArgumentNullException();
            }

            category.Description = inputModel.Description;
            category.CategoryName = inputModel.CategoryName;

            this.baseJobCategoriesRepository.Update(category);
            await this.baseJobCategoriesRepository.SaveChangesAsync();
        }
    }
}
