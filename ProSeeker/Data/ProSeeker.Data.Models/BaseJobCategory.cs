namespace ProSeeker.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using ProSeeker.Data.Common.Models;

    public class BaseJobCategory : BaseDeletableModel<int>
    {
        public BaseJobCategory()
        {
            this.JobCategories = new HashSet<JobCategory>();
        }

        [MaxLength(50)]
        public string CategoryName { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public virtual ICollection<JobCategory> JobCategories { get; set; }
    }
}
