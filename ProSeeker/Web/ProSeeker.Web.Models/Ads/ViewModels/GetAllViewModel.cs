namespace ProSeeker.Web.Models.Ads.ViewModels
{
    using System.Collections.Generic;
    using ProSeeker.Web.Models.Cities.ViewModels;
    using ProSeeker.Web.Models.Pagination.ViewModels;

    // XXXViewModel : XXXPagingViewModel : BasePagingViewModel
    public class GetAllViewModel : AdsPagingViewModel
    {
        public virtual IEnumerable<AdsShortDetailsViewModel> Ads { get; set; }

        public virtual IEnumerable<CitySimpleViewModel> Cities { get; set; }
    }
}
