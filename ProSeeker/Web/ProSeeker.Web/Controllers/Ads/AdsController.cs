namespace ProSeeker.Web.Controllers.Ads
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Ads;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Cities;
    using ProSeeker.Web.ViewModels.Ads;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Cities;

    public class AdsController : BaseController
    {
        private readonly IAdsService adsService;
        private readonly ICategoriesService categoriesService;
        private readonly ICitiesService citiesService;
        private readonly UserManager<ApplicationUser> userManager;

        public AdsController(
            IAdsService adsService,
            ICategoriesService categoriesService,
            ICitiesService citiesService,
            UserManager<ApplicationUser> userManager)
        {
            this.adsService = adsService;
            this.categoriesService = categoriesService;
            this.citiesService = citiesService;
            this.userManager = userManager;
        }

        //[Authorize(Roles = "User")]
        public IActionResult Create()
        {
            var allCities = this.citiesService.GetAllCities<CitySimpleViewModel>();
            var allcategories = this.categoriesService.GetAllCategories<CategorySimpleViewModel>();

            var createModel = new
                CreateAdInputModel();

            createModel.Categories = allcategories;
            createModel.Cities = allCities.OrderBy(x => x.Name);

            return this.View(createModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var allCities = this.citiesService.GetAllCities<CitySimpleViewModel>();
                var allcategories = this.categoriesService.GetAllCategories<CategorySimpleViewModel>();

                var createModel = new CreateAdInputModel();

                createModel.Categories = allcategories;
                createModel.Cities = allCities.OrderBy(x => x.Name);

                return this.View(createModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var newAdId = await this.adsService.CreateAsync(input, user.Id);
            
            return this.Redirect("/");
            //return this.RedirectToAction("GetDetails", new { id = id });


            // TODO: USE SANITIZER WHEN SHOWING AD DETAILS !!!!
        }
    }
}
