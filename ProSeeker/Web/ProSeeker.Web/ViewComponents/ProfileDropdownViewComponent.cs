namespace ProSeeker.Web.ViewComponents
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Inquiries;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Web.ViewModels.Offers;

    public class ProfileDropdownViewComponent : ViewComponent
    {
        private readonly IOffersService offersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IInquiriesService inquiriesService;

        public ProfileDropdownViewComponent(
            IOffersService offersService,
            UserManager<ApplicationUser> userManager,
            IInquiriesService inquiriesService)
        {
            this.offersService = offersService;
            this.userManager = userManager;
            this.inquiriesService = inquiriesService;
        }

        public IViewComponentResult Invoke()
        {
            var userId = this.userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)this.User);

            var model = new ProfileDropdownViewModel
            {
                UnredOffersCount = this.offersService.GetUnredOffersCount(userId),
                IsThereUnredOffer = this.offersService.IsThereUnredOffer(userId),
                UnredInquiriesCount = this.inquiriesService.UnredInquiriesCount(userId),
                IsThereUnredInquiry = this.inquiriesService.IsThereUnredInquiry(userId),
            };

            return this.View(model);
        }
    }
}
