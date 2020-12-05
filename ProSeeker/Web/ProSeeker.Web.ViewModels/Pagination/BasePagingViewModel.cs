namespace ProSeeker.Web.ViewModels.Pagination
{
    using ProSeeker.Common;

    // XXXViewModel : XXXPagingViewModel : BasePagingViewModel
    public abstract class BasePagingViewModel
    {
        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public int PreviousPageNumber => this.PageNumber - 1;

        public int NextPageNumber => this.PageNumber + 1;

        public int ItemsPerPage => GlobalConstants.ItemsPerPage;
    }
}
