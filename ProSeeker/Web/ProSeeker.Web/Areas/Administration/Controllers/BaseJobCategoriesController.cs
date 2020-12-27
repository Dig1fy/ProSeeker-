namespace ProSeeker.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Common;
    using ProSeeker.Data;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.BaseJobCategories;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Web.Controllers;
    using ProSeeker.Web.ViewModels.BaseJobCategories;
    using ProSeeker.Web.ViewModels.Categories;

    [Area("Administration")]
    [Authorize (Roles =GlobalConstants.AdministratorRoleName)]
    public class BaseJobCategoriesController : BaseController
    {
        private const string NotAllowed = "NotAllowed";
        private readonly IBaseJobCategoriesService baseJobCategoriesService;
        private readonly ICategoriesService categoriesService;

        public BaseJobCategoriesController(
            IBaseJobCategoriesService baseJobCategoriesService,
            ICategoriesService categoriesService)
        {
            this.baseJobCategoriesService = baseJobCategoriesService;
            this.categoriesService = categoriesService;
        }

        // GET: Administration/BaseJobCategories
        public async Task<IActionResult> Index()
        {
            var allCategories = await this.baseJobCategoriesService.GetAllBaseCategoriesAsync<SingleBaseJobCategoryViewModel>();
            return this.View(allCategories);
        }

        // GET: Administration/BaseJobCategories/Create
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BaseJobCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.baseJobCategoriesService.CreateAsync(inputModel);

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/BaseJobCategories/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return this.CustomNotFound();
            }

            var baseJobCategory = await this.baseJobCategoriesService.GetBaseJobCategoryById<SingleBaseJobCategoryViewModel>(id);

            if (baseJobCategory == null)
            {
                return this.CustomNotFound();
            }

            var inputModel = new BaseJobCategoryInputModel
            {
                Id = baseJobCategory.Id,
                CategoryName = baseJobCategory.CategoryName,
                Description = baseJobCategory.Description,
            };

            return this.View(inputModel);
        }

        // POST: Administration/BaseJobCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BaseJobCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.baseJobCategoriesService.UpdateAsync(inputModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                return this.CustomNotFound();
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            if (id == 0)
            {
                return this.CustomNotFound();
            }

            var countOfAllCategoriesInTheBaseCategory = await this.categoriesService.GetCategiesCountInBaseJobCategoryAsync(id);

            if (countOfAllCategoriesInTheBaseCategory != 0)
            {
                var model = new JobCategoriesCountViewModel { Count = countOfAllCategoriesInTheBaseCategory };
                return this.View(NotAllowed, model);
            }

            try
            {
                await this.baseJobCategoriesService.DeleteByIdAsync(id);
            }
            catch (Exception)
            {
                return this.CustomNotFound();
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
