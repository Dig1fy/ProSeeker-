namespace ProSeeker.Web.Controllers.Category
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Common;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Cities;
    using ProSeeker.Services.Data.Specialists;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Cities;

    public class JobCategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly ISpecialistsService specialistsService;
        private readonly ICitiesService citiesService;

        public JobCategoriesController(
            ICategoriesService categoriesService,
            ISpecialistsService specialistsService,
            ICitiesService citiesService)
        {
            this.categoriesService = categoriesService;
            this.specialistsService = specialistsService;
            this.citiesService = citiesService;
        }

        public async Task<IActionResult> GetCategory(int id, string sortBy, int cityId, int page = 1)
        {
            // Explicitly check the page in case someone wants to cheat :)
            page = page < 1 ? 1 : page;

            var viewModel = await this.categoriesService.GetByIdAsync<CategoriesViewModel>(id);
            viewModel.CityId = cityId;
            viewModel.SortBy = sortBy == null ? GlobalConstants.ByDateDescending : sortBy;
            viewModel.JobCategoryId = id;
            viewModel.PageNumber = page;
            viewModel.SpecialistsCount = await this.specialistsService.GetSpecialistsCountByCategoryAsync(id, cityId);

            if (viewModel.SpecialistsCount <= GlobalConstants.SpecialistsPerPage)
            {
                viewModel.PageNumber = 1;
            }

            viewModel.Cities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
            var specialistsPerPage = await this.specialistsService
                .GetAllSpecialistsPerCategoryAsync<SpecialistsInCategoryViewModel>(id, sortBy, viewModel.CityId, viewModel.PageNumber);
            viewModel.SpecialistsDetails = specialistsPerPage;

            return this.View(viewModel);
        }
    }
}
