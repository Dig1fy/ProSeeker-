namespace ProSeeker.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class JobCategory : BaseDeletableModel<int>
    {
        public JobCategory()
        {
            this.SpecialistsDetails = new HashSet<Specialist_Details>();
            this.Ads = new HashSet<Ad>();
        }

        [Required]
        [MaxLength(70)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string PictureUrl { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public int BaseJobCategoryId { get; set; }

        public virtual BaseJobCategory BaseJobCategory { get; set; }

        public virtual ICollection<Specialist_Details> SpecialistsDetails { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
    }
}
