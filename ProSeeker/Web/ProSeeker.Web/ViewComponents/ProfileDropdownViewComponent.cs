namespace ProSeeker.Web.ViewComponents
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Web.ViewModels.Offers;

    public class ProfileDropdownViewComponent : ViewComponent
    {
        private readonly IOffersService offersService;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileDropdownViewComponent(
            IOffersService offersService,
            UserManager<ApplicationUser> userManager)
        {
            this.offersService = offersService;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var userId = this.userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)this.User);

            var model = new ProfileDropdownViewModel
            {
                Count = this.offersService.GetUnredOffersCount(userId),
                IsThereUnredOffer = this.offersService.IsThereUnredOffer(userId),
            };

            return this.View(model);
        }
    }
}
