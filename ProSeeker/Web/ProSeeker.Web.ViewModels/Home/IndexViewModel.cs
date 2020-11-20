namespace ProSeeker.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<BaseJobCategoryViewModel> BaseCategories { get; set; }
    }
}
