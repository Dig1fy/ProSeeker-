namespace ProSeeker.Services.Data.Ads
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Ads;

    public interface IAdsService
    {
        Task<string> CreateAsync(CreateAdInputModel adtInputModel, string userId);

        T GetAdDetails<T>(string id);

        // IEnumerable<T> GetByCreatedOn<T>(int skip = 0);
        IEnumerable<T> GetByCategory<T>(string categoryName, int skip = 0);

        int AllAdsCount();

        int AllAdsByCategoryCount(string name);

        // Task<int> DeleteAsync(string id);

        // Task<string> UpdateAsync(AdInputModel adInputModel, string id);

        // IEnumerable<T> GetByKeyWord<T>(string search, string category, string city);
    }
}
