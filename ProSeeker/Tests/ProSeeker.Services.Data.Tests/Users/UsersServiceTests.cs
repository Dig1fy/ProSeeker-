namespace ProSeeker.Services.Data.Tests.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.UsersService;
    using Xunit;

    public sealed class UsersServiceTests : IDisposable
    {
        private readonly IUsersService service;

        private ApplicationDbContext dbContext;

        private List<ApplicationUser> users;

        public UsersServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new ApplicationDbContext(options);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(this.dbContext);
            this.service = new UsersService(usersRepository);

            this.users = new List<ApplicationUser>();
            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task ShouldReturnCorrectFirstNameByGivenUserId()
        {
            var userId = "1";
            var expectedUserFirstName = "Ivo";
            var actualFirstName = await this.service.GetUserFirstNameByIdAsync(userId);

            Assert.Equal(expectedUserFirstName, actualFirstName);
        }

        [Fact]
        public async Task ShouldReturnUsersProfilePictureByGivenId()
        {
            var userId = "1";
            var expectedResult = "SomeProfilePicture";
            var actualResult = await this.service.GetUserProfilePictureAsync(userId);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task ShouldReturnCorrectCountOfAllSpecialists()
        {
            var expectedCount = 1;
            var actualCount = await this.service.GetAllSpecialistsCountAsync();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task ShouldReturnCorrectCountOfAllRegularUsers()
        {
            var expectedCount = 1;
            var actualCount = await this.service.GetAllClientsCountAsync();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task ShouldReturnCoorectUserIdByGivenSpecialistId()
        {
            var specialistId = "specialistId";

            var expectedUserId = "2";
            var actualUserId = await this.service.GetUserIdBySpecialistIdAsync(specialistId);

            Assert.Equal(expectedUserId, actualUserId);
        }

        [Fact]
        public async Task ShouldMakeUserVipAndSetExpirationDateOfOneWeek()
        {
            var userId = "1";
            await this.service.MakeUserVip(userId);
            var user = this.users.FirstOrDefault(x => x.Id == userId);
            var isVip = user.IsVip == true;
            var hasNewExpirationDate = DateTime.UtcNow.AddDays(6);

            Assert.True(isVip);
            Assert.True(hasNewExpirationDate < user.VipExpirationDate);
        }

        public void Dispose()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        private void InitializeRepositoriesData()
        {
            this.users.AddRange(new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", FirstName = "Ivo", LastName = "Ivov", CityId = 1, Email = "u@u", ProfilePicture = "SomeProfilePicture", IsSpecialist = false, IsVip = false, VipExpirationDate = DateTime.UtcNow },
                new ApplicationUser { Id = "2", FirstName = "Gosho", LastName = "Goshev", CityId = 1, Email = "s@s", ProfilePicture = "SpecProfilePicture", IsSpecialist = true, SpecialistDetailsId = "specialistId" },
            });

            this.dbContext.AddRange(this.users);
            this.dbContext.SaveChanges();
        }
    }
}
