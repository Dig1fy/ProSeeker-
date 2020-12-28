namespace ProSeeker.Services.Data.CategoriesService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Categories;

    public interface ICategoriesService
    {
        Task<int> CreateAsync(CategoryInputModel inputModel);

        Task UpdateAsync(CategoryInputModel inputModel);

        Task<T> GetByIdAsync<T>(int id);

        Task<IEnumerable<T>> GetAllCategoriesAsync<T>();

        Task<string> GetCategoryNameByOfferIdAsync (string offerId);

        Task<int> GetCategiesCountInBaseJobCategoryAsync(int baseJobCategoryId);

        Task<int> GetSpecialistsCountInCategoryAsync(int categoryId);

        Task DeleteByIdAsync(int categoryId);

        Task<string> GetCategoryPictureByCategoryId(int categoryId);
    }
}
