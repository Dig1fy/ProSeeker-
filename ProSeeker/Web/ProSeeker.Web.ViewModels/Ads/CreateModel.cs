namespace ProSeeker.Web.ViewModels.Ads
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Cities;

    public class CreateModel
    {
        public Ad Ad { get; set; }

        public IEnumerable<AllCitiesViewModel> Cities { get; set; }

        public IEnumerable<AllCategoriesViewModel> Categories { get; set; }
    }
}
