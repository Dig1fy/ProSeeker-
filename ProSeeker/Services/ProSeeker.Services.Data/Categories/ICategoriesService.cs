namespace ProSeeker.Services.Data.CategoriesService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        Task<T> GetByIdAsync<T>(int id);

        Task<IEnumerable<T>> GetAllCategoriesAsync<T>();
    }
}
