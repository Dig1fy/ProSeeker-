namespace ProSeeker.Services.Data.Specialists
{
    using System.Collections.Generic;

    public interface ISpecialistsService
    {
        int GetSpecialistsCountByCategory(int categoryId);

        IEnumerable<T> GetAllSpecialistsPerCategory<T>(int categoryId, int page);
    }
}
