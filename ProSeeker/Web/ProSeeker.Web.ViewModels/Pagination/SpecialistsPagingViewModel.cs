namespace ProSeeker.Web.ViewModels.Pagination
{
    using System;

    using ProSeeker.Common;

    public class SpecialistsPagingViewModel : BasePagingViewModel
    {
        public int JobCategoryId { get; set; }

        public int SpecialistsCount { get; set; }

        public int PagesCount => (int)Math.Ceiling((double)this.SpecialistsCount / GlobalConstants.SpecialistsPerPage);

        public bool HasNextPage => this.PageNumber < this.PagesCount;
    }
}
