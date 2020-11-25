using System.Collections.Generic;

namespace ProSeeker.Services.Data.CategoriesService
{
    public interface ICategoriesService
    {
        T GetByName<T>(string name);

        IEnumerable<T> GetAllCategories<T>();
    }
}
