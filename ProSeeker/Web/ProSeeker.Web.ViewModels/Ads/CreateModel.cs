namespace ProSeeker.Web.ViewModels.Ads
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Cities;

    public class CreateModel
    {
        public CreateAdInputModel Ad { get; set; }

        //[Required(ErrorMessage = "ssss")]
        //public IEnumerable<CitySimpleViewModel> Cities { get; set; }

        //[Required(ErrorMessage = "ssss")]
        //public IEnumerable<CategorySimpleViewModel> Categories { get; set; }
    }
}
