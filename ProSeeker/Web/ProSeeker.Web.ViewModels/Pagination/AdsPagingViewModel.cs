namespace ProSeeker.Web.ViewModels.Pagination
{
    using System;

    using ProSeeker.Common;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    // XXXViewModel : AdsPagingViewModel : BasePagingViewModel
    public class AdsPagingViewModel : BasePagingViewModel, IMapFrom<Ad>
    {
        public string SortBy { get; set; }

        public int OpinionsCount { get; set; }

        public string CategoryName { get; set; }

        public int AdsCount { get; set; }

        public int PagesCount => (int)Math.Ceiling((double)this.AdsCount / GlobalConstants.ItemsPerPage);

        public bool HasNextPage => this.PageNumber < this.PagesCount;
    }
}
