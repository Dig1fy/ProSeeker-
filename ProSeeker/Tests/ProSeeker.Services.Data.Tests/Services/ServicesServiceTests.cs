namespace ProSeeker.Services.Data.Tests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.ServicesService;
    using Xunit;

    public sealed class ServicesServiceTests : BaseServiceTests
    {
        private readonly IServicesService service;

        private List<Service> services;

        public ServicesServiceTests()
        {
            this.services = new List<Service>();

            var servicesRepository = new EfDeletableEntityRepository<Service>(this.DbContext);

            this.service = new ServicesService(servicesRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task DeleteAsync_ShouldBeWorkingCorrectly()
        {
            var serviceId = 1;
            await this.service.DeleteAsync(serviceId);
            var doesServiceStillExist = await this.service.CheckIfServiceExists(serviceId);

            Assert.False(doesServiceStillExist);
        }

        [Fact]
        public async Task CheckIfServiceExists_CheckForExistingServiceShouldReturnTrue()
        {
            var serviceId = 1;

            var doesServiceStillExist = await this.service.CheckIfServiceExists(serviceId);

            Assert.True(doesServiceStillExist);
        }

        private void InitializeRepositoriesData()
        {
            this.services.AddRange(new List<Service>
            {
                new Service
                {
                    Id = 1,
                    SpecialistDetailsId = "specialistId",
                    Description = "ho-ho",
                    Name = "I make this and that",
                },
                new Service
                {
                    Id = 2,
                    SpecialistDetailsId = "specialistId",
                    Description = "I can do it",
                    Name = "I make this and that",
                },
            });

            this.DbContext.AddRange(this.services);
            this.DbContext.SaveChanges();
        }
    }
}
