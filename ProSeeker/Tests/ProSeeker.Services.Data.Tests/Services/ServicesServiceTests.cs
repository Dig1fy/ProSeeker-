namespace ProSeeker.Services.Data.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.ServicesService;
    using Xunit;

    public sealed class ServicesServiceTests : IDisposable
    {
        private readonly IServicesService service;

        private ApplicationDbContext dbContext;

        private List<Service> services;

        public ServicesServiceTests()
        {
            this.services = new List<Service>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new ApplicationDbContext(options);

            var servicesRepository = new EfDeletableEntityRepository<Service>(this.dbContext);

            this.service = new ServicesService(servicesRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task DeletingShouldBeWorkingCorrectly()
        {
            var serviceId = 1;
            await this.service.DeleteAsync(serviceId);
            var doesServiceStillExist = await this.service.CheckIfServiceExists(serviceId);

            Assert.False(doesServiceStillExist);
        }

        [Fact]
        public async Task CheckForExistingServiceShouldReturnTrue()
        {
            var serviceId = 1;

            var doesServiceStillExist = await this.service.CheckIfServiceExists(serviceId);

            Assert.True(doesServiceStillExist);
        }

        public void Dispose()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
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

            this.dbContext.AddRange(this.services);
            this.dbContext.SaveChanges();
        }
    }
}
