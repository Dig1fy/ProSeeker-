namespace ProSeeker.Services.Data.Home
{
    using System.Collections.Generic;

    public interface IHomeService
    {
        IEnumerable<T> GetAllBaseCategories<T>(int? count = null);
    }
}
