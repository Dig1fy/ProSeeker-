namespace ProSeeker.Services.Data.Ads
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Ads;

    public class AdsService : IAdsService
    {
        private readonly IDeletableEntityRepository<Ad> adsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public AdsService(
            IDeletableEntityRepository<Ad> adsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.adsRepository = adsRepository;
            this.usersRepository = usersRepository;
        }

        public async Task<string> CreateAsync(CreateAdInputModel adtInputModel, string userId)
        {
            var ad = new Ad
            {
                CityId = adtInputModel.CityId,
                IsVip = false,
                UserId = userId,
                JobCategoryId = adtInputModel.JobCategoryId,
                Opinions = new List<Opinion>(),
                PreparedBudget = adtInputModel.PreparedBudget,
                Title = adtInputModel.Title,
                Description = adtInputModel.Description,
            };

            await this.adsRepository.AddAsync(ad);
            await this.adsRepository.SaveChangesAsync();
            return ad.Id;
        }

        public int AllAdsByCategoryCount(string name)
        {
            var count = this.adsRepository.All()
                .Where(x => x.JobCategory.Name == name)
                .ToList()
                .Count();

            return count;
        }

        public int AllAdsCount()
            => this.adsRepository.All().Count();

        public IEnumerable<T> GetByCategory<T>(string categoryName, int skip = 0)
        {
            var allByCategory = this.adsRepository
                .All()
                .Where(x => x.JobCategory.Name == categoryName)
                .To<T>()
                .ToList();

            return allByCategory;
        }

        public T GetAdDetailsById<T>(string id)
        {
            var ad = this.adsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return ad;
        }

        public IEnumerable<T> GetMyAds<T>(string id)
        {
            var allMyAds = this.adsRepository
                .All()
                .Where(x => x.UserId == id)
                .To<T>()
                .ToList();

            return allMyAds;
        }

        public async Task DeleteById(string id)
        {
            var ad = await this.adsRepository.GetByIdWithDeletedAsync(id);
            this.adsRepository.Delete(ad);
            await this.adsRepository.SaveChangesAsync();
        }
    }
}
