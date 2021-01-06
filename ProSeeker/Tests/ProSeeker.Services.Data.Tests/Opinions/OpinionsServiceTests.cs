namespace ProSeeker.Services.Data.Tests.Opinions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Opinions;
    using Xunit;

    public sealed class OpinionsServiceTests : BaseServiceTests
    {
        private readonly IOpinionsService service;

        private List<Ad> ads;
        private List<Opinion> opinions;
        private List<Specialist_Details> specialists;

        public OpinionsServiceTests()
        {
            this.ads = new List<Ad>();
            this.opinions = new List<Opinion>();
            this.specialists = new List<Specialist_Details>();

            var adsRepository = new EfDeletableEntityRepository<Ad>(this.DbContext);
            var opinionsRepository = new EfDeletableEntityRepository<Opinion>(this.DbContext);
            var specialistsRepository = new EfDeletableEntityRepository<Specialist_Details>(this.DbContext);
            this.service = new OpinionsService(opinionsRepository, adsRepository, specialistsRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task CreateAdOpinionAsync_ShouldCreateOpinionToAdProperly()
        {
            var desiredAdId = "1";
            var userId = "1";
            var content = "Xo";

            // We currently have 4 opinions
            var newOpinionIndex = 5;

            await this.service.CreateAdOpinionAsync(desiredAdId, userId, content, null);
            var isThereNewOpinionInAd = await this.service.IsInAdIdAsync(newOpinionIndex, desiredAdId);

            Assert.True(isThereNewOpinionInAd);
        }

        [Fact]
        public async Task CreateSpecOpinionAsync_ShouldCreateOpinionToSpecialistProfileProperly()
        {
            var desiredSpecialistId = "specialist";
            var userId = "1";
            var content = "Xo";

            // We currently have 4 opinions
            var newOpinionIndex = 5;

            await this.service.CreateSpecOpinionAsync(desiredSpecialistId, userId, content, null);
            var isThereNewOpinionToSpecialist = await this.service.IsInSpecialistIdAsync(newOpinionIndex, desiredSpecialistId);

            Assert.True(isThereNewOpinionToSpecialist);
        }

        [Fact]
        public async Task IsInSpecialistIdAsync_ShouldReturnCorrectValuesIfGivenOpinionIsInSpecialistProfile()
        {
            var desiredSpecialistId = "specialist";
            var opinionId = 3;

            var isThereAnOpinion = await this.service.IsInSpecialistIdAsync(opinionId, desiredSpecialistId);

            Assert.True(isThereAnOpinion);
        }

        [Fact]
        public async Task IsInAdIdAsync_ShouldReturnCorrectValuesIfGivenOpinionIsInAdWithGivenAdId()
        {
            var desiredAdId = "1";
            var opinionId = 2;

            var isThereAnOpinion = await this.service.IsInAdIdAsync(opinionId, desiredAdId);

            Assert.True(isThereAnOpinion);
        }

        private void InitializeRepositoriesData()
        {
            this.ads.AddRange(new List<Ad>
            {
                new Ad
                {
                    Id = "1",
                    CityId = 1,
                    Description = "Търся архитект",
                    JobCategoryId = 1,
                    PreparedBudget = "Достатъчно",
                    Title = "Спешно",
                    IsVip = false, UserId = "1",
                    CreatedOn = DateTime.UtcNow,
                },
                new Ad
                {
                    Id = "2",
                    CityId = 2,
                    Description = "Търся брокер",
                    JobCategoryId = 2,
                    PreparedBudget = "Достатъчно",
                    Title = "Спешно",
                    IsVip = false,
                    UserId = "2",
                },
            });

            this.specialists.AddRange(new List<Specialist_Details>
            {
                new Specialist_Details
                {
                    Id = "specialist",
                    UserId = "1",
                    Opinions = new List<Opinion>
                        {
                        new Opinion { Id = 3, Content = "Hey", CreatorId = "1", },
                        },
                },
                new Specialist_Details
                {
                    Id = "specialist2",
                    UserId = "2",
                    Opinions = new List<Opinion>
                        {
                        new Opinion { Id = 4, Content = "Hey", CreatorId = "2", },
                        },
                },
            });

            this.opinions.AddRange(new List<Opinion>
            {
                new Opinion
                {
                    Id = 1,
                    AdId = "1",
                    SpecialistDetailsId = "specialist",
                    Content = "Hey",
                    CreatorId = "1",
                },
                new Opinion
                {
                    Id = 2,
                    AdId = "1",
                    Content = "Hey",
                    CreatorId = "2",
                },
            });

            this.DbContext.AddRange(this.specialists);
            this.DbContext.AddRange(this.opinions);
            this.DbContext.AddRange(this.ads);
            this.DbContext.SaveChanges();
        }
    }
}
