namespace ProSeeker.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Common;
    using ProSeeker.Services.Data.BaseJobCategories;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Cloud;
    using ProSeeker.Web.Controllers;
    using ProSeeker.Web.ViewModels.BaseJobCategories;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Home;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class JobCategoriesController : BaseController
    {
        private const string NotAllowed = "NotAllowed";
        private readonly IBaseJobCategoriesService baseJobCategoriesService;
        private readonly ICategoriesService categoriesService;
        private readonly ICloudinaryApplicationService cloudinaryApplicationService;

        public JobCategoriesController(
            IBaseJobCategoriesService baseJobCategoriesService,
            ICategoriesService categoriesService,
            ICloudinaryApplicationService cloudinaryApplicationService)
        {
            this.baseJobCategoriesService = baseJobCategoriesService;
            this.categoriesService = categoriesService;
            this.cloudinaryApplicationService = cloudinaryApplicationService;
        }

        // GET: Administration/JobCategories
        public async Task<IActionResult> Index()
        {
            var allCategories = await this.categoriesService.GetAllCategoriesAsync<JobCategoriesViewModel>();
            var model = new AllSubCategoriesViewModel() { Categories = allCategories };
            return this.View(model);
        }

        // GET: Administration/JobCategories/Create
        public async Task<IActionResult> Create()
        {
            var baseJobCategories = await this.baseJobCategoriesService.GetAllBaseCategoriesAsync<SimpleBaseJobCategoryViewModel>();
            var model = new CategoryInputModel();
            model.BaseJobCategories = baseJobCategories;

            return this.View(model);
        }

        // POST: Administration/JobCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryInputModel inputModel, IFormFile imageFile)
        {
            // We need to cover a few edge cases for the category picture:
            // We can create new category without changing the default picture / or change it/ or choose invalid file for picture and save.
            if (!this.ModelState.IsValid || !this.cloudinaryApplicationService.IsFileValid(imageFile))
            {
                var baseJobCategories = await this.baseJobCategoriesService.GetAllBaseCategoriesAsync<SimpleBaseJobCategoryViewModel>();
                inputModel.BaseJobCategories = baseJobCategories;

                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }
                else if (!this.cloudinaryApplicationService.IsFileValid(imageFile))
                {
                    inputModel.StatusMessage = GlobalConstants.InvalidProfilePictureMessage;
                    return this.View(inputModel);
                }
            }

            if (imageFile != null)
            {
                var imageName = Guid.NewGuid().ToString() + imageFile.FileName;
                var imageUrl = await this.cloudinaryApplicationService.UploadImageAsync(imageFile, imageName);
                inputModel.PictureUrl = imageUrl;
            }
            else
            {
                inputModel.PictureUrl = GlobalConstants.DefaultCategoryPictureUrl;
            }

            var newCategory = await this.categoriesService.CreateAsync(inputModel);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return this.CustomNotFound();
            }

            var jobCategory = await this.categoriesService.GetByIdAsync<JobCategoriesViewModel>(id);

            if (jobCategory == null)
            {
                return this.CustomNotFound();
            }

            var inputModel = new CategoryInputModel
            {
                BaseJobCategoryId = jobCategory.BaseJobCategoryId,
                Description = jobCategory.Description,
                Id = jobCategory.Id,
                Name = jobCategory.Name,
                PictureUrl = jobCategory.PictureUrl,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryInputModel inputModel, IFormFile imageFile)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }
            else if (imageFile != null && !this.cloudinaryApplicationService.IsFileValid(imageFile))
            {
                inputModel.StatusMessage = GlobalConstants.InvalidProfilePictureMessage;
                return this.View(inputModel);
            }

            if (imageFile != null)
            {
            var imageName = imageFile.FileName;
            var imageUrl = await this.cloudinaryApplicationService.UploadImageAsync(imageFile, imageName);
            inputModel.PictureUrl = imageUrl;
            }
            else
            {
                var existingCategoryPicture = await this.categoriesService.GetCategoryPictureByCategoryId(inputModel.Id);
                if (existingCategoryPicture != null)
                {
                    inputModel.PictureUrl = existingCategoryPicture;
                }
            }

            try
            {
                await this.categoriesService.UpdateAsync(inputModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                return this.CustomNotFound();
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int categoryId)
        {
            if (categoryId == 0)
            {
                return this.CustomNotFound();
            }

            var countOfAllSpecialistsInThisCategory = await this.categoriesService.GetSpecialistsCountInCategoryAsync(categoryId);

            if (countOfAllSpecialistsInThisCategory != 0)
            {
                var model = new SpecialistsInCategorySimpleViewModel { Count = countOfAllSpecialistsInThisCategory };
                return this.View(NotAllowed, model);
            }

            try
            {
                await this.categoriesService.DeleteByIdAsync(categoryId);
            }
            catch (Exception)
            {
                return this.CustomNotFound();
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
