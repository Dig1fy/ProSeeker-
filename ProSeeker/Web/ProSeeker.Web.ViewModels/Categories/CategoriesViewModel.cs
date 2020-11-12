namespace ProSeeker.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class CategoriesViewModel : IMapFrom<JobCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public virtual ICollection<SpecialistsInCategoryViewModel> SpecialistsDetails { get; set; }
    }
}
