namespace ProSeeker.Web.ViewModels.Home
{
    using ProSeeker.Web.ViewModels.Categories;
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<BaseJobCategoryViewModel> BaseCategories { get; set; }

        public IndexCountersViewModel Counters { get; set; }
    }
}
