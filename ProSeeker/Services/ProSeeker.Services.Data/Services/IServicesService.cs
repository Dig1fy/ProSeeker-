namespace ProSeeker.Services.Data.ServicesService
{
    using System.Threading.Tasks;

    public interface IServicesService
    {
        Task DeleteAsync(int id);

        Task<bool> CheckIfServiceExists(int serviceId);
    }
}
