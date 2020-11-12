namespace ProSeeker.Services.Data.CategoriesService
{
    public interface ICategoriesService
    {
        T GetByName<T>(string name);
    }
}
