namespace ProSeeker.Web.Controllers.Category
{
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Web.ViewModels.Categories;

    public class JobCategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public JobCategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult GetCategory(int id)
        {
            var viewModel = this.categoriesService.GetById<CategoriesViewModel>(id);
            return this.View(viewModel);
        }
    }
}
