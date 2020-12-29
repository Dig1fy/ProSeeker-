namespace ProSeeker.Services.Data.Ads
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Ads;

    public interface IAdsService
    {
        Task<string> CreateAsync(CreateAdInputModel adtInputModel, string userId);

        Task<T> GetAdDetailsByIdAsync<T>(string id);

        Task<int> GetAdsCountByUserIdAsync(string id);

        Task<IEnumerable<T>> GetMyAdsAsync<T>(string id, int page);

        // IEnumerable<T> GetByCreatedOn<T>(int skip = 0);
        Task<IEnumerable<T>> GetByCategoryAsync<T>(string categoryName, string sortBy, int cityId, int page);

        Task<int> AllAdsCountAsync();

        Task<int> AllAdsByCategoryCountAsync(string categoryName, int cityId = 0);

        Task DeleteByIdAsync(string id);

        Task UpdateAdAsync(UpdateInputModel model);

        Task<string> GetUserIdByAdIdAsync(string currentAdId);

        Task MakeAdsVipAsync(string userId);

        Task<int> GetAllAdsCountAsync();
    }
}
