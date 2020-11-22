using System.Threading.Tasks;

namespace ProSeeker.Services.Data.ServicesService
{
    public interface IServicesService
    {
        Task DeleteAsync(int id);
    }
}
