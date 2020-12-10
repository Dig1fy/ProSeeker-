namespace ProSeeker.Services.Data.Specialists
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISpecialistsService
    {
        Task<int> GetSpecialistsCountByCategoryAsync(int categoryId, int cityId = 0);

        Task<IEnumerable<T>> GetAllSpecialistsPerCategoryAsync<T>(int categoryId, string sortBy, int cityId, int page);
    }
}
