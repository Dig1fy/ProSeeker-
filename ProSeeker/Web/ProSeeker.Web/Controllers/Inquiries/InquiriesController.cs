namespace ProSeeker.Web.Controllers.Inquiries
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Cities;
    using ProSeeker.Services.Data.Inquiries;
    using ProSeeker.Web.ViewModels.Cities;
    using ProSeeker.Web.ViewModels.Inquiries;
    using ProSeeker.Web.ViewModels.Offers;

    public class InquiriesController : BaseController
    {
        private readonly IInquiriesService inquiriesService;
        private readonly ICitiesService citiesService;
        private readonly UserManager<ApplicationUser> userManager;

        public InquiriesController(
            IInquiriesService inquiriesService,
            ICitiesService citiesService,
            UserManager<ApplicationUser> userManager)
        {
            this.inquiriesService = inquiriesService;
            this.citiesService = citiesService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Create(string specialistId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (currentUser.SpecialistDetailsId == specialistId)
            {
                return this.CustomAccessDenied();
            }

            var model = new CreateInquiryInputModel
            {
                SpecialistDetailsId = specialistId,
                Cities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>(),
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateInquiryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel.Cities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
                return this.View(inputModel);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            // If someone tries to cheat :)
            if (currentUser.SpecialistDetailsId == inputModel.SpecialistDetailsId)
            {
                return this.CustomAccessDenied();
            }

            inputModel.UserId = currentUser.Id;
            await this.inquiriesService.CreateAsync(inputModel);
            return this.RedirectToAction("GetProfile", "SpecialistsDetails", new { id = inputModel.SpecialistDetailsId });
        }

        [Authorize]
        public async Task<IActionResult> MyInquiries()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var model = new AllMyInquiriesViewModel();

            model.Inquiries = await this.inquiriesService.GetSpecialistEnquiriesAsync<InquiriesViewModel>(user.SpecialistDetailsId);

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(string inquiryId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var inquiry = await this.inquiriesService.GetDetailsByIdAsync<InquiryDetailsViewModel>(inquiryId);

            if (inquiry == null || currentUser == null)
            {
                return this.CustomNotFound();
            }

            // Explicit check against cheaters :)
            if (inquiry.SpecialistDetailsId != currentUser.SpecialistDetailsId)
            {
                return this.CustomAccessDenied();
            }

            if (!inquiry.IsRed)
            {
                inquiry.IsRed = true;
                await this.inquiriesService.MarkInquiryAsRedAsync(inquiryId);
            }

            inquiry.IsAcountOwner = inquiry.SpecialistDetailsId == currentUser.SpecialistDetailsId;
            return this.View(inquiry);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string inquiryId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var existingOffer = await this.inquiriesService.CheckForExistingOfferAsync<ExistingOfferViewModel>(inquiryId);

            // If there is an offer related to the inquiry, the offer needs to be deleted first.
            if (existingOffer != null)
            {
                return this.View("ExistingOffer", existingOffer);
            }

            await this.inquiriesService.DeleteByIdAsync(inquiryId);

            if (currentUser.IsSpecialist)
            {
                return this.RedirectToAction(nameof(this.MyInquiries));
            }
            else
            {
                return this.RedirectToAction("UserOffers", "Offers");
            }
        }
    }
}
