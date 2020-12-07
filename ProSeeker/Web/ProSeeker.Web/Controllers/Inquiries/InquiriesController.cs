namespace ProSeeker.Web.Controllers.Inquiries
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Cities;
    using ProSeeker.Services.Data.Inquiries;
    using ProSeeker.Web.ViewModels.Cities;
    using ProSeeker.Web.ViewModels.Inquiries;

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

        public IActionResult Create(string specialistId)
        {
            var model = new InquiryInputModel
            {
                SpecialistDetailsId = specialistId,
                Cities = this.citiesService.GetAllCities<CitySimpleViewModel>(),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InquiryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            inputModel.UserId = user.Id;
            await this.inquiriesService.CreateAsync(inputModel);
            return this.Redirect("/");   // TODO: Redirect to MyEnquiries when you create that section.
        }
    }
}
