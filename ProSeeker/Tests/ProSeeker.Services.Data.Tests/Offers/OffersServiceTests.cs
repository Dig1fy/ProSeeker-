namespace ProSeeker.Services.Data.Tests.Offers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Offers;
    using Xunit;

    public sealed class OffersServiceTests : BaseServiceTests
    {
        private readonly IOffersService service;

        private List<ApplicationUser> users;
        private List<Ad> ads;
        private List<Offer> offers;

        public OffersServiceTests()
        {
            this.users = new List<ApplicationUser>();
            this.ads = new List<Ad>();
            this.offers = new List<Offer>();

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(this.DbContext);
            var offersRepository = new EfDeletableEntityRepository<Offer>(this.DbContext);
            var adsRepository = new EfDeletableEntityRepository<Ad>(this.DbContext);
            this.service = new OffersService(offersRepository, usersRepository, adsRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task ShouldReturnExistingOfferIfAny()
        {
            AutoMapperConfig.RegisterMappings(typeof(ExistingOfferViewModel).Assembly);
            var desiredAdId = "1";
            var applicationUserId = "1";
            var specialistDetailsId = "specialistId";

            var existingOfferByAd = await this.service.GetExistingOfferAsync<ExistingOfferViewModel>(desiredAdId, applicationUserId, specialistDetailsId);

            Assert.Equal(desiredAdId, existingOfferByAd.Id);
        }

        // We can create an offer in two ways - directly from existing Ad/ from user's inquiry
        [Fact]
        public async Task CreateFromAdShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ExistingOfferViewModel).Assembly);
            var newAd = new Ad
            {
                Id = "3",
                UserId = "1",
                CityId = 1,
                Description = "Ново!",
                JobCategoryId = 1,
                PreparedBudget = "2500лв.",
            };

            await this.DbContext.AddAsync(newAd);
            await this.DbContext.SaveChangesAsync();

            var inputModel = new CreateOfferInputModel
            {
                AdId = "3",
                ApplicationUserId = "1",
                SpecialistDetailsId = "specialistId",
                Price = 500,
                Description = "Предлагам незабравима услуга!",
                ExpirationDate = DateTime.UtcNow.AddDays(2),
            };

            var desiredAdId = "3";
            var applicationUserId = "1";
            var specialistDetailsId = "specialistId";

            var newOfferId = await this.service.CreateFromAdAsync(inputModel);
            var checkForNewOffer = await this.service.GetExistingOfferAsync<ExistingOfferViewModel>(desiredAdId, applicationUserId, specialistDetailsId);

            Assert.Equal(checkForNewOffer.Id, newOfferId);
        }

        [Fact]
        public async Task CreatingAnOfferFromInquiryShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ExistingOfferViewModel).Assembly);
            var applicationUserId = "1";

            var createInputModel = new CreateOfferInputModel
            {
                ApplicationUserId = "1",
                InquiryId = "1",
                SpecialistDetailsId = "specialistId",
                Description = "Offer from inquiry",
            };

            await this.service.CreateFromInquiryAsync(createInputModel);
            var allUserOffers = await this.service.GetAllUserOffersAsync<ExistingOfferViewModel>(applicationUserId);
            var expectedUserOffersCount = 2;

            Assert.Equal(expectedUserOffersCount, allUserOffers.Count());
        }

        [Fact]
        public async Task ShouldDeleteAnOfferProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ExistingOfferViewModel).Assembly);

            var offerId = "1";
            var userAdId = "1";
            var applicationUserId = "1";
            var specialistDetailsId = "specialistId";

            await this.service.DeleteByIdAsync(offerId);
            var checkForExistingOffer =
                await this.service.GetExistingOfferAsync<ExistingOfferViewModel>(userAdId, applicationUserId, specialistDetailsId);

            Assert.Null(checkForExistingOffer);
        }

        [Fact]
        public async Task ShouldReturnAllSpecialistOffers()
        {
            AutoMapperConfig.RegisterMappings(typeof(ExistingOfferViewModel).Assembly);

            var specialistDetailsId = "specialistId";

            var specialistOffers = await this.service.GetAllSpecialistOffersAsync<ExistingOfferViewModel>(specialistDetailsId);
            var expectedCount = 2;
            var actualCount = specialistOffers.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task ShouldReturnCorrectOfferByGivenOfferId()
        {
            AutoMapperConfig.RegisterMappings(typeof(ExistingOfferViewModel).Assembly);
            var existingOfferId = "2";

            var offer = await this.service.GetDetailsByIdAsync<ExistingOfferViewModel>(existingOfferId);

            Assert.Equal(existingOfferId, offer.Id);
        }

        [Fact]
        public void ShouldReturnCorrectNumberOfUnredOffers()
        {
            var userId = "2";
            var expectedCount = 1;

            var unredOffersCount = this.service.GetUnredOffersCount(userId);

            Assert.Equal(expectedCount, unredOffersCount);
        }

        [Fact]
        public void ShouldReturnCorrectBoolValueIfThereAreAnyUnredOffers()
        {
            var userId = "2";

            var isUnredOffer = this.service.IsThereUnredOffer(userId);

            Assert.True(isUnredOffer);
        }

        [Fact]
        public async Task ShouldAdjustUnredOfferToRedCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ExistingOfferViewModel).Assembly);
            var existingOfferId = "2";

            await this.service.MarkOfferAsRedAsync(existingOfferId);
            var offer = await this.service.GetDetailsByIdAsync<ExistingOfferViewModel>(existingOfferId);

            Assert.False(offer.IsRed == false);
        }

        [Fact]
        public async Task AcceptingOfferShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ExistingOfferViewModel).Assembly);
            var existingOfferId = "2";

            await this.service.AcceptOffer(existingOfferId);
            var offer = await this.service.GetDetailsByIdAsync<ExistingOfferViewModel>(existingOfferId);

            Assert.True(offer.IsAccepted == true);
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

            this.users.AddRange(new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    FirstName = "Ivo",
                    LastName = "Ivov",
                    CityId = 1,
                    Email = "u@u",
                    ProfilePicture = "SomeProfilePicture",
                    IsSpecialist = false,
                },
                new ApplicationUser
                {
                    Id = "2",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    CityId = 1,
                    Email = "s@s",
                    ProfilePicture = "SpecProfilePicture",
                    IsSpecialist = true,
                    SpecialistDetailsId = "specialistId",
                },
            });

            this.offers.AddRange(new List<Offer>
            {
                new Offer
                {
                    Id = "1",
                    AdId = "1",
                    ApplicationUserId = "1",
                    SpecialistDetailsId = "specialistId",
                    Description = "New offer",
                    Price = 55,
                    IsRed = true,
                    StartDate = "10-10-2030",
                    IsAccepted = true,
                    AcceptedOn = DateTime.Parse("10-10-2020"),
                    ExpirationDate = DateTime.Parse("10-10-2030"),
                },
                new Offer
                {
                    Id = "2",
                    InquiryId = "2",
                    ApplicationUserId = "2",
                    Description = "New offer",
                    Price = 55,
                    IsRed = false,
                    StartDate = "10-10-2030",
                    IsAccepted = false,
                    ExpirationDate = DateTime.Parse("10-10-2030"),
                    SpecialistDetailsId = "specialistId",
                },
            });

            this.DbContext.AddRange(this.users);
            this.DbContext.AddRange(this.offers);
            this.DbContext.AddRange(this.ads);
            this.DbContext.SaveChanges();
        }
    }
}
