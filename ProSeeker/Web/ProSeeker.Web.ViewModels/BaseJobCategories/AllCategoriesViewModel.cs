namespace ProSeeker.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using ProSeeker.Web.ViewModels.Home;

    public class AllCategoriesViewModel
    {
        public IEnumerable<BaseJobCategoryViewModel> BaseCategories { get; set; }
    }
}
