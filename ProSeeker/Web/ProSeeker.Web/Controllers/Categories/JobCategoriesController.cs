namespace ProSeeker.Web.Controllers.Category
{
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Web.ViewModels.Categories;

    public class JobCategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public JobCategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult ByName(string name)
        {
            var viewModel = this.categoriesService.GetByName<CategoriesViewModel>(name);
            return this.View(viewModel);
        }
    }
}
