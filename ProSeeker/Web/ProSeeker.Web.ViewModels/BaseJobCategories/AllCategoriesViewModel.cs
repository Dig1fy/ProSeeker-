namespace ProSeeker.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class AllCategoriesViewModel
    {
        public IEnumerable<BaseJobCategoryViewModel> BaseCategories { get; set; }
    }
}
