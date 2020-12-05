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
        private readonly IRepository<City> citiesRepository;
        private readonly IDeletableEntityRepository<JobCategory> categoriesRepository;

        public AdsService(
            IDeletableEntityRepository<Ad> adsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<City> citiesRepository,
            IDeletableEntityRepository<JobCategory> categoriesRepository)
        {
            this.adsRepository = adsRepository;
            this.usersRepository = usersRepository;
            this.citiesRepository = citiesRepository;
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<string> CreateAsync(CreateAdInputModel adtInputModel, string userId)
        {
            var ad = new Ad
            {
                CityId = adtInputModel.CityId,
                City = this.citiesRepository.All().FirstOrDefault(x => x.Id == adtInputModel.CityId),
                IsVip = false,
                UserId = userId,
                JobCategoryId = adtInputModel.JobCategoryId,
                JobCategory = this.categoriesRepository.All().FirstOrDefault(x => x.Id == adtInputModel.JobCategoryId),
                Opinions = new List<Opinion>(),
                PreparedBudget = adtInputModel.PreparedBudget,
                Title = adtInputModel.Title,
                Description = adtInputModel.Description,
            };

            await this.adsRepository.AddAsync(ad);
            await this.adsRepository.SaveChangesAsync();
            return ad.Id;
        }

        public async Task UpdateAdAsync(UpdateInputModel model)
        {
            var ad = this.adsRepository.All().FirstOrDefault(x => x.Id == model.Id);

            ad.CityId = model.CityId;
            ad.Description = model.Description;
            ad.Title = model.Title;
            ad.PreparedBudget = model.PreparedBudget;
            ad.JobCategoryId = model.JobCategoryId;

            this.adsRepository.Update(ad);
            await this.adsRepository.SaveChangesAsync();
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
                .AllAsNoTracking()
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
