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

        public AdsService(IDeletableEntityRepository<Ad> adsRepository)
        {
            this.adsRepository = adsRepository;
        }

        public async Task<string> CreateAsync(AdInputModel adtInputModel, string userId)
        {
            throw new System.NotImplementedException();
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

        public T GetAdDetails<T>(string id)
        {
            var ad = this.adsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return ad;
        }
    }
}
