namespace ProSeeker.Web.Controllers.Category
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Common;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Cities;
    using ProSeeker.Services.Data.Specialists;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Cities;
    using ProSeeker.Web.ViewModels.Pagination;

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

        public async Task<IActionResult> GetCategory(SpecialistsPerPageViewtModel input)
        {
            // Explicitly check the page in case someone wants to cheat :)
            input.Page = input.Page < 1 ? 1 : input.Page;

            var viewModel = await this.categoriesService.GetByIdAsync<CategoriesViewModel>(input.Id);
            viewModel.CityId = input.CityId;
            viewModel.SortBy = input.SortBy == null ? GlobalConstants.ByDateDescending : input.SortBy;
            viewModel.JobCategoryId = input.Id;
            viewModel.PageNumber = input.Page;
            viewModel.SpecialistsCount = await this.specialistsService.GetSpecialistsCountByCategoryAsync(input.Id, input.CityId);

            if (viewModel.SpecialistsCount <= GlobalConstants.SpecialistsPerPage)
            {
                viewModel.PageNumber = 1;
            }

            viewModel.Cities = await this.citiesService.GetAllCitiesAsync<CitySimpleViewModel>();
            var specialistsPerPage = await this.specialistsService
                .GetAllSpecialistsPerCategoryAsync<SpecialistsInCategoryViewModel>(input.Id, input.SortBy, viewModel.CityId, viewModel.PageNumber);

            viewModel.SpecialistsDetails = specialistsPerPage;

            return this.View(viewModel);
        }
    }
}
