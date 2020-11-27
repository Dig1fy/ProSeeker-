namespace ProSeeker.Web.ViewComponents
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Home;

    public class NavbarCategoriesViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<BaseJobCategory> baseJobCategoriesRepository;
        private readonly IDeletableEntityRepository<JobCategory> jobCategoriesRepository;

        public NavbarCategoriesViewComponent(
            IDeletableEntityRepository<BaseJobCategory> baseJobCategoriesRepository,
            IDeletableEntityRepository<JobCategory> jobCategoriesRepository)
        {
            this.baseJobCategoriesRepository = baseJobCategoriesRepository;
            this.jobCategoriesRepository = jobCategoriesRepository;
        }

        public IViewComponentResult Invoke()
        {
            var model = new AllCategoriesViewModel
            {
                BaseCategories = this.baseJobCategoriesRepository.All().To<BaseJobCategoryViewModel>().ToList(),
            };

            model
                .BaseCategories
                .Select(x => x.JobCategories.Select(y => this.jobCategoriesRepository
                .All()
                .To<JobCategoriesViewModel>()
                .ToList()));

            return this.View(model);
        }
    }
}
