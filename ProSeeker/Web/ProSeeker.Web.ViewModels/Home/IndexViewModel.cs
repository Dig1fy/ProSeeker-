namespace ProSeeker.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models;

    public class IndexViewModel
    {
        public IEnumerable<BaseJobCategoryViewModel> BaseCategories { get; set; }
    }
}
