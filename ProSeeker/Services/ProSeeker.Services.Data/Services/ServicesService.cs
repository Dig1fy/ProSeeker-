namespace ProSeeker.Services.Data.ServicesService
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;

    public class ServicesService : IServicesService
    {
        private readonly IDeletableEntityRepository<Service> serviceRepository;

        public ServicesService(IDeletableEntityRepository<Service> serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        public async Task DeleteAsync(int id)
        {
            var service = await this.serviceRepository.GetByIdWithDeletedAsync(id);
            this.serviceRepository.Delete(service);
            await this.serviceRepository.SaveChangesAsync();
        }

        public async Task<bool> CheckIfServiceExists(int serviceId)
        {
            var service = await this.serviceRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == serviceId);

            return service != null;
        }
    }
}
