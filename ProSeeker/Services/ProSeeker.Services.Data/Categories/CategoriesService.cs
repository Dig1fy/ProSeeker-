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
        private readonly IDeletableEntityRepository<Ad> adsRepository;
        private readonly IRepository<Offer> offersRepository;

        public CategoriesService(
            IDeletableEntityRepository<JobCategory> categoriesRepository,
            IDeletableEntityRepository<Ad> adsRepository,
            IRepository<Offer> offersRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.adsRepository = adsRepository;
            this.offersRepository = offersRepository;
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
                .AllAsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == offer.AdId);

            // If the request for delete comes from Inquiries
            if (ad == null)
            {
                return string.Empty;
            }

            var category = await this.categoriesRepository
                .AllAsNoTracking()
                .Where(x => x.Id == ad.JobCategoryId)
                .FirstOrDefaultAsync();

            return category.Name;
        }
    }
}
