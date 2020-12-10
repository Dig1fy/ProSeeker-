namespace ProSeeker.Web.Controllers.Ads
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
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
    using ProSeeker.Web.ViewModels.Pagination;

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
        public async Task<IActionResult> GetByCategory(AdsPerPageViewModel input)
        {
            // Explicitly check the page in case someone wants to cheat :)
            input.Page = input.Page < 1 ? 1 : input.Page;

            var model = new GetAllViewModel
            {
                PageNumber = input.Page,
                AdsCount = await this.adsService.AllAdsByCategoryCountAsync(input.CategoryName, input.CityId),
                SortBy = input.SortBy == null ? GlobalConstants.ByDateDescending : input.SortBy,
                CityId = input.CityId,
                CategoryName = input.CategoryName,
                Cities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>(),
            };

            // If we're on page 4 and decide to filter by city, and the ads count are not enough for 4 pages, pagination will break.
            // Therefore, we need to explicitly check each time and then get all model Ads.
            if (model.AdsCount <= GlobalConstants.ItemsPerPage)
            {
                model.PageNumber = 1;
            }

            model.Ads = await this.adsService.GetByCategoryAsync<AdsShortDetailsViewModel>(input.CategoryName, input.SortBy, input.CityId, model.PageNumber);

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

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var model = await this.adsService.GetAdDetailsByIdAsync<UpdateInputModel>(id);

            if (model.UserId != this.userManager.GetUserId(this.User))
            {
                return this.RedirectToAction("AccessDenied", "Errors");
            }

            var allCities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
            var allcategories = await this.categoriesService.GetAllCategoriesAsync<CategorySimpleViewModel>();

            model.Cities = allCities;
            model.Categories = allcategories;

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(UpdateInputModel inputModel)
        {
            if (this.userManager.GetUserId(this.User) == inputModel.UserId)
            {
                return this.RedirectToAction("AccessDenied", "Errors");
            }

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
            var currenUserId = this.userManager.GetUserId(this.User);
            var ad = await this.adsService.GetAdDetailsByIdAsync<AdsFullDetailsViewModel>(id);
            ad.IsOwnerOfAd = ad.UserId == currenUserId;
            return this.View(ad);
        }
    }
}
