namespace ProSeeker.Services.Data.Tests.Ratings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Raitings;
    using Xunit;

    public sealed class RatingsServiceTests : BaseServiceTests
    {
        private readonly IRatingsService service;

        private List<Rating> ratings;

        public RatingsServiceTests()
        {
            this.ratings = new List<Rating>();

            var ratingsRepository = new EfRepository<Rating>(this.DbContext);

            this.service = new RatingsService(ratingsRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task GetAverageRatingAsync_ShouldReturnCorrectValue()
        {
            var specialistId = "specialistId";
            var expectedAverageRating = 4.5;

            var actualRating = await this.service.GetAverageRatingAsync(specialistId);

            Assert.Equal(expectedAverageRating, actualRating);
        }

        [Fact]
        public async Task GetRatingsCountAsync_ShouldReturnCorrectNumberOfRatings()
        {
            var specialistId = "specialistId";
            var expectedRatingsCount = 2;

            var actualRatingsCount = await this.service.GetRatingsCountAsync(specialistId);

            Assert.Equal(expectedRatingsCount, actualRatingsCount);
        }

        [Fact]
        public async Task GetRatingsCountAsync_OneUserShouldBeAbleToRateOnlyOnceAndHisLastRatingShouldBeAccounted()
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

            this.DbContext.AddRange(this.ratings);
            this.DbContext.SaveChanges();
        }
    }
}
