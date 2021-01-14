namespace ProSeeker.Web.Controllers.Offers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Common;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Ads;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Services.Messaging;
    using ProSeeker.Web.ViewModels.Offers;
    using ProSeeker.Web.ViewModels.Pagination;

    public class OffersController : BaseController
    {
        private readonly IOffersService offersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly IAdsService adsService;
        private readonly ICategoriesService categoriesService;
        private readonly IEmailSender emailSender;

        public OffersController(
            IOffersService offersService,
            UserManager<ApplicationUser> userManager,
            IUsersService usersService,
            IAdsService adsService,
            ICategoriesService categoriesService,
            IEmailSender emailSender)
        {
            this.offersService = offersService;
            this.userManager = userManager;
            this.usersService = usersService;
            this.adsService = adsService;
            this.categoriesService = categoriesService;
            this.emailSender = emailSender;
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.SpecialistRoleName)]
        public async Task<IActionResult> Create(CreateOfferViewModel viewModel)
        {
            var specialist = await this.userManager.GetUserAsync(this.User);
            var userId = string.Empty;

            // The specialist can make an offer in two ways - from inquiry/from Ad. If the offer is being made from an inquiry, the Ad is null.
            // The logic of the app: only 1 offer by Ad / many offers by inquiry
            // viewModel.Id => offer ID
            if (viewModel.Id != null)
            {
                userId = await this.adsService.GetUserIdByAdIdAsync(viewModel.Id);
                var existingOffer = await this.offersService.GetExistingOfferAsync<ExistingOfferViewModel>(viewModel.Id, userId, specialist.SpecialistDetailsId);

                if (existingOffer != null)
                {
                    return this.View(nameof(this.ExistingOffer), existingOffer);
                }
            }
            else
            {
                userId = viewModel.ApplicationUserId;
            }

            var inputModel = new CreateOfferInputModel
            {
                SpecialistDetailsId = specialist.SpecialistDetailsId,
                ApplicationUserId = userId,
                AdId = viewModel.Id,
                InquiryId = viewModel.InquiryId,
                PhoneNumber = specialist.PhoneNumber,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.SpecialistRoleName)]
        public async Task<IActionResult> Create(CreateOfferInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (inputModel.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = inputModel.PhoneNumber;
                await this.userManager.UpdateAsync(user);
            }

            inputModel.SpecialistDetailsId = user.SpecialistDetailsId;

            // If the offer comes from an Ad
            if (inputModel.AdId != null)
            {
                await this.offersService.CreateFromAdAsync(inputModel);
            }

            // The offer comes from an Inquiry
            else
            {
                await this.offersService.CreateFromInquiryAsync(inputModel);
            }

            return this.Redirect("/");
        }

        [Authorize]
        public async Task<IActionResult> UserOffers()
        {
            var userId = this.userManager.GetUserId(this.User);
            var allMyOffers = await this.offersService.GetAllUserOffersAsync<UserOffersViewModel>(userId);

            var model = new AllMyUserOffersViewModel
            {
                Offers = allMyOffers,
            };

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> SpecialistOffers()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var allMyOffers = await this.offersService.GetAllSpecialistOffersAsync<SpecialistOffersViewModel>(user.SpecialistDetailsId);

            var model = new AllMySpecialistOffersViewModel
            {
                Offers = allMyOffers,
            };

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(string offerId)
        {
            var currenUserId = this.userManager.GetUserId(this.User);
            var offer = await this.offersService.GetDetailsByIdAsync<OfferDetailsViewModel>(offerId);

            if (offer is null)
            {
                return this.CustomNotFound();
            }

            if (offer.ApplicationUserId != currenUserId)
            {
                return this.CustomAccessDenied();
            }

            if (!offer.IsRed)
            {
                offer.IsRed = true;
                await this.offersService.MarkOfferAsRedAsync(offerId);
            }

            offer.IsAcountsOwner = offer.ApplicationUserId == currenUserId;
            return this.View(offer);
        }

        [Authorize]
        public async Task<IActionResult> DetailsSent(string offerId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var offer = await this.offersService.GetDetailsByIdAsync<OfferDetailsSentViewModel>(offerId);

            if (offer is null)
            {
                return this.NotFound();
            }

            if (offer.SpecialistDetailsId != currentUser.SpecialistDetailsId)
            {
                return this.CustomAccessDenied();
            }

            offer.IsAcountsOwner = offer.SpecialistDetailsId == currentUser.SpecialistDetailsId;
            return this.View(offer);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string offerId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            /* We have 3 redirections:
             - 2 for specialists - delete request comes from attempt to send 2 offers for 1 ad / delete inquiry which has related offers to it
             - 1 for user */
            if (currentUser.IsSpecialist)
            {
                var categoryName = await this.categoriesService.GetCategoryNameByOfferIdAsync(offerId);
                await this.offersService.DeleteByIdAsync(offerId);

                if (categoryName == string.Empty)
                {
                    // If the delete request comes from "MyInquiries", the category name could not be taken and the redirect has to be different.
                    return this.RedirectToAction("MyInquiries", "Inquiries");
                }
                else
                {
                    return this.RedirectToAction("GetByCategory", "Ads", new AdsPerPageViewModel { CategoryName = categoryName });
                }
            }
            else
            {
                await this.offersService.DeleteByIdAsync(offerId);
                return this.RedirectToAction(nameof(this.UserOffers));
            }
        }

        [Authorize]
        public IActionResult ExistingOffer()
        {
            return this.View();
        }

        [Authorize]
        public async Task<IActionResult> Accept(string offerId)
        {
            try
            {
                // Users cannot accept an offer if their profile has no phone number data.
                var currentUser = await this.userManager.GetUserAsync(this.User);
                if (currentUser.PhoneNumber == null)
                {
                    return this.RedirectToAction(nameof(this.MissingPhoneNumber), new { offerId });
                }

                var currentUserId = this.userManager.GetUserId(this.User);
                var model = await this.offersService.GetOffersSenderAndReceiverDataByOfferIdAsync(offerId, currentUserId);
                await this.SendEmailsToBothSidesAsync(model);
                await this.offersService.AcceptOffer(offerId);
                return this.RedirectToAction(nameof(this.Details), new { offerId });
            }
            catch (Exception)
            {
                return this.CustomCommonError();
            }
        }

        public IActionResult MissingPhoneNumber(string offerId)
        {
            var model = new MissingOfferInputModel
            {
                OfferId = offerId,
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MissingPhoneNumber(MissingOfferInputModel model)
        { // ADD phoneNumber validation (regex) and implement the view

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);
                await this.usersService.UpdateUserPhoneNumberAsync(currentUser.Id, model.UserPhoneNumber);

                return this.RedirectToAction(nameof(this.Accept), new { model.OfferId });
            }
            catch (Exception)
            {
                return this.CustomNotFound();
            }
        }

        private async Task SendEmailsToBothSidesAsync(ViewModels.EmailsSender.SendEmailViewModel model)
        {
            var subject = GlobalConstants.AcceptedOfferSubject;
            var content = GlobalMethods.GetContentForAcceptedOffer(
                model.UserFullname,
                model.UserEmail,
                model.UserPhone,
                model.SpecialistPhone,
                model.SpecialistFullName,
                model.SpecialistEmail,
                model.OfferDescription,
                model.Price);

            // SendGrind requires registering sender in their platform so we need to send the messages separately - in both emails the sender will be ProSeeker 
            // Send to user
            await this.emailSender.SendEmailAsync(GlobalConstants.ApplicationEmail, GlobalConstants.ApplicationName, model.UserEmail, subject, content);

            // Send to specialist
            await this.emailSender.SendEmailAsync(GlobalConstants.ApplicationEmail, GlobalConstants.ApplicationName, model.SpecialistEmail, subject, content);
        }
    }
}
