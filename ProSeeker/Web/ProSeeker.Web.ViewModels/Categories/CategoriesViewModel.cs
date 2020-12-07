namespace ProSeeker.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Pagination;

    public class CategoriesViewModel : SpecialistsPagingViewModel, IMapFrom<JobCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public virtual ICollection<SpecialistsInCategoryViewModel> SpecialistsDetails { get; set; }
    }
}
