namespace ProSeeker.Web.ViewComponents
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Web.ViewModels.Offers;
    using System.Threading.Tasks;

    public class NewOffersViewComponent : ViewComponent
    {
        private readonly IOffersService offersService;
        private readonly UserManager<ApplicationUser> userManager;

        public NewOffersViewComponent(
            IOffersService offersService,
            UserManager<ApplicationUser> userManager)
        {
            this.offersService = offersService;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var userId = this.userManager.GetUserId((System.Security.Claims.ClaimsPrincipal)this.User);

            var model = new OffersNavbarViewModel
            {
                Count = this.offersService.GetUnredOffersCount(userId),
                IsThereUnredOffer = this.offersService.IsThereUnredOffer(userId),
            };

            return this.View(model);
        }
    }
}
