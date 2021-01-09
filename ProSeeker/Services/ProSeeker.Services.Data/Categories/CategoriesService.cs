namespace ProSeeker.Services.Data.CategoriesService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Categories;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<JobCategory> categoriesRepository;
        private readonly IDeletableEntityRepository<Ad> adsRepository;
        private readonly IRepository<Offer> offersRepository;
        private readonly IDeletableEntityRepository<Specialist_Details> specialistsRepository;

        public CategoriesService(
            IDeletableEntityRepository<JobCategory> categoriesRepository,
            IDeletableEntityRepository<Ad> adsRepository,
            IRepository<Offer> offersRepository,
            IDeletableEntityRepository<Specialist_Details> specialistsRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.adsRepository = adsRepository;
            this.offersRepository = offersRepository;
            this.specialistsRepository = specialistsRepository;
        }

        public async Task<int> GetCategiesCountInBaseJobCategoryAsync(int baseJobCategoryId)
        {
            var categoriesInJobCategory = await this.categoriesRepository
                .All()
                .Where(x => x.BaseJobCategoryId == baseJobCategoryId)
                .CountAsync();
            return categoriesInJobCategory;
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

        public async Task<string> GetCategoryNameByOfferIdAsync(string offerId)
        {
            var offer = await this.offersRepository
                .All()
                .Where(x => x.Id == offerId)
                .FirstOrDefaultAsync();

            var ad = await this.adsRepository
                .All()
                .FirstOrDefaultAsync(a => a.Id == offer.AdId);

            // If the request for delete comes from Inquiries
            if (ad == null)
            {
                return string.Empty;
            }

            var category = await this.categoriesRepository
                .All()
                .Where(x => x.Id == ad.JobCategoryId)
                .FirstOrDefaultAsync();

            return category.Name;
        }

        public async Task<int> CreateAsync(CategoryInputModel inputModel)
        {
            var category = new JobCategory
            {
                BaseJobCategoryId = inputModel.BaseJobCategoryId,
                Description = inputModel.Description,
                Name = inputModel.Name,
                PictureUrl = inputModel.PictureUrl,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.Id;
        }

        public async Task UpdateAsync(CategoryInputModel inputModel)
        {
            var category = await this.categoriesRepository.All().FirstOrDefaultAsync(x => x.Id == inputModel.Id);

            if (category == null)
            {
                throw new ArgumentNullException();
            }

            category.Description = inputModel.Description;
            category.Name = inputModel.Name;
            category.PictureUrl = inputModel.PictureUrl;

            this.categoriesRepository.Update(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task<int> GetSpecialistsCountInCategoryAsync(int categoryId)
        {
            var specialistsCount = await this.specialistsRepository
                .All()
                .Where(x => x.JobCategoryId == categoryId)
                .CountAsync();

            return specialistsCount;
        }

        public async Task DeleteByIdAsync(int categoryId)
        {
            var category = await this.categoriesRepository
                .AllWithDeleted()
                .FirstOrDefaultAsync(x => x.Id == categoryId);

            if (category == null)
            {
                throw new ArgumentNullException();
            }

            this.categoriesRepository.Delete(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task<string> GetCategoryPictureByCategoryId(int categoryId)
        {
            var categoryPicture = await this.categoriesRepository
                .All()
                .Where(x => x.Id == categoryId)
                .Select(x => x.PictureUrl)
                .FirstOrDefaultAsync();

            return categoryPicture;
        }
    }
}
