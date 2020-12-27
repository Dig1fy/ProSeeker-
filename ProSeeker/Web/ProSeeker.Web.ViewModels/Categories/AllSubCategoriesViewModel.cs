namespace ProSeeker.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using ProSeeker.Web.ViewModels.Home;

    public class AllSubCategoriesViewModel
    {
        public IEnumerable<JobCategoriesViewModel> Categories{ get; set; }
    }
}
