namespace ProSeeker.Services.Data.Home
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHomeService
    {
        Task<IEnumerable<T>> GetAllBaseCategoriesAsync<T>(int? count = null);
    }
}
