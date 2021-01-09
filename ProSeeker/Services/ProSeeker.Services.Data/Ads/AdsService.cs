namespace ProSeeker.Services.Data.Ads
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Common;
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
                City = await this.citiesRepository.All().FirstOrDefaultAsync(x => x.Id == adtInputModel.CityId),
                IsVip = false,
                UserId = userId,
                JobCategoryId = adtInputModel.JobCategoryId,
                JobCategory = await this.categoriesRepository.All().FirstOrDefaultAsync(x => x.Id == adtInputModel.JobCategoryId),
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

        public async Task<int> AllAdsByCategoryCountAsync(string categoryName, int cityId)
        {
            if (cityId == 0)
            {
                return await this.adsRepository.AllAsNoTracking()
                .Where(x => x.JobCategory.Name == categoryName)
                .CountAsync();
            }
            else
            {
                return await this.adsRepository.AllAsNoTracking()
               .Where(x => x.JobCategory.Name == categoryName && x.CityId == cityId)
               .CountAsync();
            }
        }

        public async Task<int> AllAdsCountAsync()
            => await this.adsRepository.AllAsNoTracking().CountAsync();

        public async Task<IEnumerable<T>> GetByCategoryAsync<T>(string categoryName, string sortBy, int cityId, int page)
        {
            sortBy = sortBy == null ? GlobalConstants.ByDateDescending : sortBy;

            var sortedAds = this.SortAds(categoryName, sortBy, cityId);
            var allByCategory = await sortedAds
                .Skip((page - 1) * GlobalConstants.ItemsPerPage)
                .Take(GlobalConstants.ItemsPerPage)
                .To<T>()
                .ToListAsync();

            return allByCategory;
        }

        public async Task<T> GetAdDetailsByIdAsync<T>(string id)
        {
            var ad = await this.adsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return ad;
        }

        public async Task<IEnumerable<T>> GetMyAdsAsync<T>(string id, int page)
        {
            var adsToSkip = (page - 1) * GlobalConstants.ItemsPerPage;

            var allMyAds = await this.adsRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == id)
                .OrderByDescending(x => x.CreatedOn)
                .Skip(adsToSkip)
                .Take(GlobalConstants.ItemsPerPage)
                .To<T>()
                .ToListAsync();

            return allMyAds;
        }

        public async Task DeleteByIdAsync(string id)
        {
            var ad = await this.adsRepository.GetByIdWithDeletedAsync(id);
            this.adsRepository.Delete(ad);
            await this.adsRepository.SaveChangesAsync();
        }

        public async Task<int> GetAdsCountByUserIdAsync(string id)
        {
            var allUserAds = await this.adsRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == id)
                .CountAsync();

            return allUserAds;
        }

        public async Task<string> GetUserIdByAdIdAsync(string currentAdId)
        {
            var userId = await this.adsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == currentAdId)
                .Select(y => y.UserId)
                .FirstOrDefaultAsync();

            return userId;
        }

        public async Task MakeAdsVipAsync(string userId)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return;
            }

            var userAds = await this.adsRepository.All().Where(u => u.UserId == userId).ToListAsync();
            if (userAds.Count > 0)
            {
                foreach (var ad in userAds)
                {
                    ad.IsVip = true;
                    ad.VipExpirationDate = user.VipExpirationDate;
                    this.adsRepository.Update(ad);
                    await this.adsRepository.SaveChangesAsync();
                }
            }
        }

        public async Task<int> GetAllAdsCountAsync()
        {
            var allAdsCount = await this.adsRepository.All().CountAsync();

            return allAdsCount;
        }

        // TODO: Try with reflection
        private IQueryable<Ad> SortAds(string categoryName, string sortBy, int cityId)
        {
            var ads = this.adsRepository
                .All()
                .Where(x => x.JobCategory.Name == categoryName);

            if (cityId != 0)
            {
                ads = ads.Where(x => x.CityId == cityId);
            }

            return sortBy switch
            {
                GlobalConstants.ByDateDescending => ads.OrderByDescending(x => x.IsVip == true).ThenByDescending(x => x.CreatedOn),
                GlobalConstants.ByOpinionsDescending => ads.OrderByDescending(x => x.IsVip == true).ThenByDescending(x => x.Opinions.Count),
                GlobalConstants.ByUpVotesDescending => ads.OrderByDescending(x => x.IsVip == true).ThenByDescending(x => x.Votes.Where(v => v.VoteType == VoteType.UpVote).Count()),
                GlobalConstants.ByDownVotesDescending => ads.OrderByDescending(x => x.IsVip == true).ThenByDescending(x => x.Votes.Where(v => v.VoteType == VoteType.DownVote).Count()),
                _ => ads,
            };
        }
    }
}
