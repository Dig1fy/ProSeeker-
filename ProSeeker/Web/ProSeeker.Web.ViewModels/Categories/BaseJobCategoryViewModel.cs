namespace ProSeeker.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Home;

    public class BaseJobCategoryViewModel : IMapFrom<BaseJobCategory>
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public IEnumerable<JobCategoriesViewModel> JobCategories { get; set; }
    }
}
