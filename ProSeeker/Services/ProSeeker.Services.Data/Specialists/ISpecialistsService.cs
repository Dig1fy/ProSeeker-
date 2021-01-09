namespace ProSeeker.Services.Data.Specialists
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Categories;

    public interface ISpecialistsService
    {
        Task<int> GetSpecialistsCountByCategoryAsync(int categoryId, int cityId = 0);

        Task<IEnumerable<T>> GetAllSpecialistsPerCategoryAsync<T>(int categoryId, string sortBy, int cityId, int page);

        Task<IEnumerable<SpecialistsInCategoryViewModel>> GetSpecialistsFullDetailsPerCategoryAsync(int categoryId, string sortBy, int cityId, int page);
    }
}
