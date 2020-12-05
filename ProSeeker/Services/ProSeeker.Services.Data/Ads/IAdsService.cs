namespace ProSeeker.Services.Data.Ads
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Ads;

    public interface IAdsService
    {
        Task<string> CreateAsync(CreateAdInputModel adtInputModel, string userId);

        T GetAdDetailsById<T>(string id);

        int GetAdsCountByUserId(string id);

        IEnumerable<T> GetMyAds<T>(string id, int page);

        // IEnumerable<T> GetByCreatedOn<T>(int skip = 0);
        IEnumerable<T> GetByCategory<T>(string categoryName, int page);

        int AllAdsCount();

        int AllAdsByCategoryCount(string name);

        Task DeleteById(string id);

        Task UpdateAdAsync(UpdateInputModel model);

        // Task<string> UpdateAsync(AdInputModel adInputModel, string id);

        // IEnumerable<T> GetByKeyWord<T>(string search, string category, string city);
    }
}
