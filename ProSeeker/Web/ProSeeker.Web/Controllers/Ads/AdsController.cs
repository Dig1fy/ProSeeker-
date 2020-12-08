namespace ProSeeker.Web.Controllers.Ads
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Common;
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
        public async Task<IActionResult> Create()
        {
            var allCities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
            var allcategories = await this.categoriesService.GetAllCategoriesAsync<CategorySimpleViewModel>();

            var createModel = new CreateAdInputModel
            {
                Categories = allcategories,
                Cities = allCities,
            };

            return this.View(createModel);
        }

        // [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var createModel = new CreateAdInputModel
                {
                    Categories = await this.categoriesService.GetAllCategoriesAsync<CategorySimpleViewModel>(),
                    Cities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>(),
                };

                return this.View(createModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var newAdId = await this.adsService.CreateAsync(input, user.Id);

            this.TempData["Message"] = "Успешно създадохте нова обява!";
            return this.RedirectToAction(nameof(this.MyAds));

            // return this.RedirectToAction("GetDetails", new { id = id });
            // TODO: USE SANITIZER WHEN SHOWING AD DETAILS !!!!
        }

        // [Authorize]
        public async Task<IActionResult> GetByCategory(string categoryName, int page = 1)
        {
            // Explicitly check the page in case someone wants to cheat :)
            page = page < 1 ? 1 : page;

            var model = new GetAllViewModel
            {
                CategoryName = categoryName,
                PageNumber = page,
                AdsCount = await this.adsService.AllAdsByCategoryCountAsync(categoryName),
                Ads = await this.adsService.GetByCategoryAsync<AdsShortDetailsViewModel>(categoryName, page),
            };

            return this.View(model);
        }

        //[Authorize]
        // Is in role RegularUser
        public async Task<IActionResult> MyAds(int page = 1)
        {
            page = page < 1 ? 1 : page;

            var user = await this.userManager.GetUserAsync(this.User);
            var adsPerPage = await this.adsService.GetMyAdsAsync<AdsShortDetailsViewModel>(user.Id, page);
            var allMyAdsCount = await this.adsService.GetAdsCountByUserIdAsync(user.Id);

            var model = new GetAllViewModel
            {
                Ads = adsPerPage,
                AdsCount = allMyAdsCount,
                PageNumber = page,
            };

            return this.View(model);
        }

        // [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await this.adsService.DeleteByIdAsync(id);
            return this.RedirectToAction(nameof(this.MyAds));
        }

        // [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await this.adsService.GetAdDetailsByIdAsync<UpdateInputModel>(id);
            var allCities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
            var allcategories = await this.categoriesService.GetAllCategoriesAsync<CategorySimpleViewModel>();

            model.Cities = allCities;
            model.Categories = allcategories;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel.Cities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
                inputModel.Categories = await this.categoriesService.GetAllCategoriesAsync<CategorySimpleViewModel>();
                return this.View(inputModel);
            }

            await this.adsService.UpdateAdAsync(inputModel);

            // TODO Find a way to show this meesage only after the ad's been adjusted
            // this.TempData["Message"] = "Успешно коригирахте своята обява!";
            return this.RedirectToAction(nameof(this.MyAds));
        }

        public async Task<IActionResult> GetById(string id)
        {
            var ad = await this.adsService.GetAdDetailsByIdAsync<AdsFullDetailsViewModel>(id);
            return this.View(ad);
        }
    }
}
