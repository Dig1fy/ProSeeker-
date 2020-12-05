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

        // [Authorize(Roles = "User")]
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

        // [Authorize(Roles = "User")]
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

            this.TempData["Message"] = "Успешно създадохте нова обява!"; //ТОДО - CHANGE REDIRECT
            return this.Redirect("/");

            // return this.RedirectToAction("GetDetails", new { id = id });
            // TODO: USE SANITIZER WHEN SHOWING AD DETAILS !!!!
        }

        // [Authorize]
        public IActionResult GetByCategory(string id)
        {
            var adsByCategory = this.adsService.GetByCategory<AdsShortDetailsViewModel>(id);
            var model = new GetAllViewModel();
            model.Ads = adsByCategory;

            return this.View(model);
        }

        //[Authorize]
        // Is in role RegularUser
        public async Task<IActionResult> MyAds()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var allMyAds = this.adsService.GetMyAds<AdsShortDetailsViewModel>(user.Id);
            var model = new GetAllViewModel();
            model.Ads = allMyAds;
            return this.View(model);
        }

        // [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await this.adsService.DeleteById(id);
            return this.RedirectToAction(nameof(this.MyAds));
        }

        // [Authorize]
        public IActionResult Edit(string id)
        {
            var model = this.adsService.GetAdDetailsById<UpdateInputModel>(id);
            var allCities = this.citiesService.GetAllCities<CitySimpleViewModel>();
            var allcategories = this.categoriesService.GetAllCategories<CategorySimpleViewModel>();

            model.Cities = allCities;
            model.Categories = allcategories;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel.Cities = this.citiesService.GetAllCities<CitySimpleViewModel>();
                inputModel.Categories = this.categoriesService.GetAllCategories<CategorySimpleViewModel>();
                return this.View(inputModel);
            }

            await this.adsService.UpdateAdAsync(inputModel);

            this.TempData["Message"] = "Успешно коригирахте своята обява!";
            return this.RedirectToAction(nameof(this.MyAds));
        }

        public IActionResult GetById(string id)
        {
            var ad = this.adsService.GetAdDetailsById<AdsFullDetailsViewModel>(id);
            return this.View(ad);
        }
    }
}
