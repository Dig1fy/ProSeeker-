namespace ProSeeker.Services.Data.Tests.Ratings
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Raitings;
    using Xunit;

    public sealed class RatingsServiceTests : IDisposable
    {
        private readonly IRatingsService service;

        private ApplicationDbContext dbContext;

        private List<Rating> ratings;

        public RatingsServiceTests()
        {
            this.ratings = new List<Rating>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new ApplicationDbContext(options);

            var ratingsRepository = new EfRepository<Rating>(this.dbContext);

            this.service = new RatingsService(ratingsRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task AverageRatingShouldReturnCorrectValue()
        {
            var specialistId = "specialistId";
            var expectedAverageRating = 4.5;

            var actualRating = await this.service.GetAverageRatingAsync(specialistId);

            Assert.Equal(expectedAverageRating, actualRating);
        }

        [Fact]
        public async Task ShouldReturnCorrectNumberOfRatings()
        {
            var specialistId = "specialistId";
            var expectedRatingsCount = 2;

            var actualRatingsCount = await this.service.GetRatingsCountAsync(specialistId);

            Assert.Equal(expectedRatingsCount, actualRatingsCount);
        }

        [Fact]
        public async Task OneUserShouldBeAbleToRateOnlyOnceAndHisLastRatingShouldBeAccounted()
        {
            var userId = "1";
            var specialistId = "specialistId";
            var lastRatingValue = 3;

            await this.service.SetRatingAsync(specialistId, userId, 5);
            await this.service.SetRatingAsync(specialistId, userId, 4);
            await this.service.SetRatingAsync(specialistId, userId, 1);
            await this.service.SetRatingAsync(specialistId, userId, 1);
            await this.service.SetRatingAsync(specialistId, userId, lastRatingValue);
            var actualRatingsCount = await this.service.GetRatingsCountAsync(specialistId);
            var expectedRatingsCount = 2;

            Assert.Equal(expectedRatingsCount, actualRatingsCount);
        }

        public void Dispose()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        private void InitializeRepositoriesData()
        {
            this.ratings.AddRange(new List<Rating>
            {
                new Rating
                {
                    Id = 1,
                    UserId = "1",
                    SpecialistDetailsId = "specialistId",
                    Value = 4,
                },
                new Rating
                {
                    Id = 2,
                    UserId = "2",
                    SpecialistDetailsId = "specialistId",
                    Value = 5,
                },
            });

            this.dbContext.AddRange(this.ratings);
            this.dbContext.SaveChanges();
        }
    }
}
