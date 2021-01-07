namespace ProSeeker.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using ProSeeker.Web.ViewModels.Categories;

    public class IndexViewModel
    {
        public IEnumerable<BaseJobCategoryViewModel> BaseCategories { get; set; }

        public IndexCountersViewModel Counters { get; set; }
    }
}
