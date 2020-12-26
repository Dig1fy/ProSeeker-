namespace ProSeeker.Services.Data.Tests.Inquiries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Inquiries;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Inquiries;
    using ProSeeker.Web.ViewModels.Offers;
    using Xunit;

    public sealed class InquiriesServiceTests : BaseServiceTests
    {
        private readonly IInquiriesService service;

        private List<Offer> offers;
        private List<ApplicationUser> users;
        private List<Inquiry> inquiries;

        public InquiriesServiceTests()
        {
            this.offers = new List<Offer>();
            this.users = new List<ApplicationUser>();
            this.inquiries = new List<Inquiry>();

            var offersRepository = new EfDeletableEntityRepository<Offer>(this.DbContext);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(this.DbContext);
            var inquiriesRepository = new EfDeletableEntityRepository<Inquiry>(this.DbContext);

            this.service = new InquiriesService(inquiriesRepository, offersRepository, usersRepository);
            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task ShouldReturnExistingOfferByGivenInquiryIdIfAny()
        {
            var inquiryId = "1";

            AutoMapperConfig.RegisterMappings(typeof(ExistingOfferViewModel).Assembly);
            var offer = await this.service.CheckForExistingOfferAsync<ExistingOfferViewModel>(inquiryId);
            var expectedOfferId = "2";

            Assert.NotNull(offer);
            Assert.Equal(expectedOfferId, offer.Id);
        }

        [Fact]
        public async Task CreateShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(InquiriesViewModel).Assembly);
            var specialistId = "specialistId";
            var inputModel = new CreateInquiryInputModel
            {
                CityId = 2,
                Content = "What is what",
                SpecialistDetailsId = "specialistId",
                UserId = "2",
                ValidUntil = DateTime.UtcNow.AddDays(6),
            };

            await this.service.CreateAsync(inputModel);
            var specialistInquiries = await this.service.GetSpecialistEnquiriesAsync<InquiriesViewModel>(specialistId);
            var expectedInquiriesCount = 3;

            Assert.Equal(expectedInquiriesCount, specialistInquiries.Count());
        }

        [Fact]
        public async Task ShouldReturnCorrectInquiryIfAny()
        {
            AutoMapperConfig.RegisterMappings(typeof(InquiriesViewModel).Assembly);
            var existingInquiry = this.inquiries.FirstOrDefault(x => x.Id == "1");

            var inquiry = await this.service.GetDetailsByIdAsync<InquiriesViewModel>(existingInquiry.Id);

            Assert.Equal(existingInquiry.Id, inquiry.Id);
            Assert.Equal(existingInquiry.Content, inquiry.Content);
        }

        [Fact]
        public void ShouldReturnTrueIfThereIsAnyUnseenInquiry()
        {
            var userId = "2";

            var result = this.service.IsThereUnredInquiry(userId);

            Assert.True(result);
        }

        [Fact]
        public async Task ShouldMarkTheInquiryAsRedByGivenInquiryId()
        {
            var inquiryId = "2";
            var userId = "2";

            await this.service.MarkInquiryAsRedAsync(inquiryId);
            var unredInquiriesCount = this.service.UnredInquiriesCount(userId);
            var isThereUnredInquiry = unredInquiriesCount > 0;

            Assert.False(isThereUnredInquiry);
        }

        [Fact]
        public async Task ShouldDeleteInquiryByGivenId()
        {
            AutoMapperConfig.RegisterMappings(typeof(InquiriesViewModel).Assembly);
            var inquiryId = "2";

            await this.service.DeleteByIdAsync(inquiryId);
            var inquiry = await this.service.GetDetailsByIdAsync<InquiriesViewModel>(inquiryId);

            Assert.Null(inquiry);
        }

        private void InitializeRepositoriesData()
        {
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
                    CityId = 1, Email = "s@s",
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
                    Description = "New offer",
                    Price = 55,
                    IsRed = true,
                    StartDate = "10-10-2030",
                    IsAccepted = true,
                    AcceptedOn = DateTime.Parse("10-10-2020"),
                    ExpirationDate = DateTime.Parse("10-10-2030"),
                    SpecialistDetailsId = "specialistId",
                },
                new Offer
                {
                    Id = "2",
                    InquiryId = "1",
                    ApplicationUserId = "1",
                    Description = "New offer",
                    Price = 55,
                    IsRed = false,
                    StartDate = "10-10-2030",
                    IsAccepted = false,
                    ExpirationDate = DateTime.Parse("10-10-2030"),
                    SpecialistDetailsId = "specialistId",
                },
            });

            this.inquiries.AddRange(new List<Inquiry>
            {
                new Inquiry
                {
                    Id = "1",
                    UserId = "1",
                    SpecialistDetailsId = "specialistId",
                    CityId = 1,
                    Content = "I want to ask you something",
                    IsRed = true,
                    ValidUntil = DateTime.Parse("10-10-2030"),
                    CreatedOn = DateTime.Parse("10-10-2020"),
                },
                new Inquiry
                {
                    Id = "2",
                    UserId = "2",
                    SpecialistDetailsId = "specialistId",
                    CityId = 1,
                    Content = "I've got a couple of questions",
                    IsRed = false,
                    ValidUntil = DateTime.Parse("10-10-2030"),
                    CreatedOn = DateTime.Parse("10-10-2020"),
                },
            });

            this.DbContext.AddRange(this.users);
            this.DbContext.AddRange(this.inquiries);
            this.DbContext.AddRange(this.offers);
            this.DbContext.SaveChanges();
        }
    }
}
