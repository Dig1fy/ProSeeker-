namespace ProSeeker.Web.ViewModels.Ads
{
    using System.Collections.Generic;

    using ProSeeker.Web.ViewModels.Cities;
    using ProSeeker.Web.ViewModels.Pagination;

    // XXXViewModel : XXXPagingViewModel : BasePagingViewModel
    public class GetAllViewModel : AdsPagingViewModel
    {
        public virtual IEnumerable<AdsShortDetailsViewModel> Ads { get; set; }

        public virtual IEnumerable<CitySimpleViewModel> Cities { get; set; }
    }
}
