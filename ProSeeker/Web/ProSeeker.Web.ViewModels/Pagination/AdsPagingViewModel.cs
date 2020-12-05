namespace ProSeeker.Web.ViewModels.Pagination
{
    using System;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Categories;

    // XXXViewModel : AdsPagingViewModel : BasePagingViewModel
    public class AdsPagingViewModel : BasePagingViewModel, IMapFrom<Ad>
    {
        public string CategoryName { get; set; }

        public int AdsCount { get; set; }

        public int PagesCount => (int)Math.Ceiling((double)this.AdsCount / this.ItemsPerPage);

        public bool HasNextPage => this.PageNumber < this.PagesCount;
    }
}
