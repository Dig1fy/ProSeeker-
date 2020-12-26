namespace ProSeeker.Services.Data.BaseJobCategories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.BaseJobCategories;

    public interface IBaseJobCategoriesService
    {
        Task<int> CreateAsync(BaseJobCategoryInputModel inputModel);

        Task<IEnumerable<T>> GetAllBaseCategoriesAsync<T>();

        Task<T> GetBaseJobCategoryById<T>(int baseCategoryId);

        Task UpdateAsync(BaseJobCategoryInputModel inputModel);

        Task DeleteByIdAsync(int baseCategoryId);
    }
}
