namespace ProSeeker.Web.ViewModels.Home
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using System;

    public class JobCategoriesViewModel : IMapFrom<JobCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public string ShortDescription =>
            this.Description.Length > 30 ? this.Description.Substring(0, 30) + "..." : this.Description;

        public int BaseJobCategoryId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
