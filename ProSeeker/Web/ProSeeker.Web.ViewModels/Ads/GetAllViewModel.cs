namespace ProSeeker.Web.ViewModels.Ads
{
    using System.Collections.Generic;

    using ProSeeker.Web.ViewModels.Pagination;

    // XXXViewModel : XXXPagingViewModel : BasePagingViewModel
    public class GetAllViewModel : AdsPagingViewModel
    {
        public IEnumerable<AdsShortDetailsViewModel> Ads { get; set; }
    }
}
