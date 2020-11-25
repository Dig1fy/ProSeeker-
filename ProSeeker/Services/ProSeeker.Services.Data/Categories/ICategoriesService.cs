using System.Collections.Generic;

namespace ProSeeker.Services.Data.CategoriesService
{
    public interface ICategoriesService
    {
        T GetById<T>(int id);

        IEnumerable<T> GetAllCategories<T>();
    }
}
