namespace ProSeeker.Services.Data.Cities
{
    using System.Collections.Generic;

    public interface ICitiesService
    {
        IEnumerable<T> GetAllCities<T>();
    }
}
