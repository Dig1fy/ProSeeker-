namespace ProSeeker.Services.Data.Specialists
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISpecialistsService
    {
        Task<int> GetSpecialistsCountByCategoryAsync(int categoryId);

        Task<IEnumerable<T>> GetAllSpecialistsPerCategoryAsync<T>(int categoryId, string sortBy, int page);
    }
}
