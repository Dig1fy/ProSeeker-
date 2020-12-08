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
        Task<IEnumerable<T>> GetByCategoryAsync<T>(string categoryName, int page);

        Task<int> AllAdsCountAsync();

        Task<int> AllAdsByCategoryCountAsync(string name);

        Task DeleteByIdAsync(string id);

        Task UpdateAdAsync(UpdateInputModel model);

        // Task<string> UpdateAsync(AdInputModel adInputModel, string id);

        // IEnumerable<T> GetByKeyWord<T>(string search, string category, string city);
    }
}
