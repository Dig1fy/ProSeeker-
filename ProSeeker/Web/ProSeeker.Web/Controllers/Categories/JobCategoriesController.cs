namespace ProSeeker.Web.Controllers.Category
{
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Specialists;
    using ProSeeker.Web.ViewModels.Categories;
    using System.Linq;

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

        public IActionResult GetCategory(int id, int page = 1)
        {
            // Explicitly check the page in case someone wants to cheat :)
            page = page < 1 ? 1 : page;

            var viewModel = this.categoriesService.GetById<CategoriesViewModel>(id);
            viewModel.JobCategoryId = id;
            viewModel.PageNumber = page;
            viewModel.SpecialistsCount = this.specialistsService.GetSpecialistsCountByCategory(id);

            var specialistsPerPage = this.specialistsService.GetAllSpecialistsPerCategory<SpecialistsInCategoryViewModel>(id, page).ToList();
            viewModel.SpecialistsDetails = specialistsPerPage;

            return this.View(viewModel);
        }
    }
}
