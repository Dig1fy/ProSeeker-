namespace ProSeeker.Web.Controllers.Offers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using ProSeeker.Common;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Ads;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Web.ViewModels.Offers;
    using ProSeeker.Web.ViewModels.Pagination;

    public class OffersController : BaseController
    {
        private readonly IOffersService offersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly IAdsService adsService;
        private readonly ICategoriesService categoriesService;

        public OffersController(
            IOffersService offersService,
            UserManager<ApplicationUser> userManager,
            IUsersService usersService,
            IAdsService adsService,
            ICategoriesService categoriesService)
        {
            this.offersService = offersService;
            this.userManager = userManager;
            this.usersService = usersService;
            this.adsService = adsService;
            this.categoriesService = categoriesService;
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.SpecialistRoleName)]
        public async Task<IActionResult> Create(CreateOfferViewModel viewModel)
        {
            var specialist = await this.userManager.GetUserAsync(this.User);
            var userId = await this.adsService.GetUserIdByAdIdAsync(viewModel.Id);

            // The specialist can make an offer in two ways - from inquiry/from Ad. If the offer is being made from an inquiry, the Ad is null.
            // The logic of the app: only 1 offer by Ad / many offers by inquiry
            if (viewModel.AdId != null)
            {
                var existingOffer = await this.offersService.GetExistingOfferAsync<ExistingOfferViewModel>(viewModel.Id, userId, specialist.SpecialistDetailsId);

                if (existingOffer != null)
                {
                    return this.View(nameof(this.ExistingOffer), existingOffer);
                }
            }

            var inputModel = new CreateOfferInputModel
            {
                AdId = viewModel.Id,
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

            var specialistId = user.SpecialistDetailsId;
            var newOfferId = await this.offersService.CreateAsync(inputModel, specialistId);

            return this.Redirect("/");
        }

        public async Task<IActionResult> UserOffers()
        {
            var userId = this.userManager.GetUserId(this.User);
            var allMyOffers = await this.offersService.GetAllUserOffersAsync<UserOffersViewModel>(userId);

            var model = new AllMyOffersViewModel
            {
                Offers = allMyOffers,
            };

            return this.View(model);
        }

        public async Task<IActionResult> Details(string offerId)
        {
            var currenUserId = this.userManager.GetUserId(this.User);
            var offer = await this.offersService.GetDetailsByIdAsync<OfferDetailsViewModel>(offerId);

            if (offer is null)
            {
                return this.NotFound();
            }

            if (offer.ApplicationUserId != currenUserId)
            {
                return this.RedirectToAction("AccessDenied", "Errors");
            }

            if (!offer.IsRed)
            {
                offer.IsRed = true;
                await this.offersService.MarkOfferAsRedAsync(offerId);
            }

            offer.IsAcountsOwner = offer.ApplicationUserId == currenUserId;
            return this.View(offer);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string offerId)
        {
            var categoryName = await this.categoriesService.GetCategoryNameByOfferIdAsync(offerId);
            var currentUser = await this.userManager.GetUserAsync(this.User);
            await this.offersService.DeleteByIdAsync(offerId);

            if (currentUser.IsSpecialist)
            {
                var model = new AdsPerPageViewModel { CategoryName = categoryName };
                return this.RedirectToAction("GetByCategory", "Ads", new AdsPerPageViewModel { CategoryName = categoryName });
            }
            else
            {
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
            await this.offersService.AcceptOffer(offerId);
            return this.RedirectToAction(nameof(this.Details), new { offerId });
        }
    }
}
