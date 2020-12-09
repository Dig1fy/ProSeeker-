namespace ProSeeker.Web.Controllers.Category
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Common;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Specialists;
    using ProSeeker.Web.ViewModels.Categories;

    public class JobCategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly ISpecialistsService specialistsService;

        public JobCategoriesController(
            ICategoriesService categoriesService,
            ISpecialistsService specialistsService)
        {
            this.categoriesService = categoriesService;
            this.specialistsService = specialistsService;
        }

        public async Task<IActionResult> GetCategory(int id, string sortBy, int page = 1)
        {
            // Explicitly check the page in case someone wants to cheat :)
            page = page < 1 ? 1 : page;

            var viewModel = await this.categoriesService.GetByIdAsync<CategoriesViewModel>(id);
            viewModel.SortBy = sortBy == null ? GlobalConstants.ByDateDescending : sortBy;
            viewModel.JobCategoryId = id;
            viewModel.PageNumber = page;
            viewModel.SpecialistsCount = await this.specialistsService.GetSpecialistsCountByCategoryAsync(id);

            var specialistsPerPage = await this.specialistsService.GetAllSpecialistsPerCategoryAsync<SpecialistsInCategoryViewModel>(id, sortBy, page);
            viewModel.SpecialistsDetails = specialistsPerPage;

            return this.View(viewModel);
        }
    }
}
