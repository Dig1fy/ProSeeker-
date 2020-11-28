namespace ProSeeker.Web.Controllers.Offers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Web.ViewModels.Offers;

    public class OffersController : BaseController
    {
        private readonly IOffersService offersService;
        private readonly UserManager<ApplicationUser> userManager;

        public OffersController(IOffersService offersService, UserManager<ApplicationUser> userManager)
        {
            this.offersService = offersService;
            this.userManager = userManager;
        }

        //[Authorize(Roles = GlobalConstants.SpecialistRoleName)]
        public IActionResult Create(CreateOfferViewModel viewModel)
        {
            return this.View(viewModel);
        }

        [HttpPost]
        //[Authorize(Roles = GlobalConstants.SpecialistRoleName)]
        public async Task<IActionResult> Create(CreateOfferInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var specialistId = user.SpecialistDetailsId;
            var newOfferId = await this.offersService.CreateAsync(inputModel, specialistId);

            return this.Redirect("/");
        }
    }
}
