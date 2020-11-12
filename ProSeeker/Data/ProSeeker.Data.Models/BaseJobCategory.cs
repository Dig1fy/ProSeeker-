namespace ProSeeker.Data.Models
{
    using System.Collections.Generic;

    using ProSeeker.Data.Common.Models;

    public class BaseJobCategory : BaseDeletableModel<int>
    {
        public BaseJobCategory()
        {
            this.JobCategories = new HashSet<JobCategory>();
        }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<JobCategory> JobCategories { get; set; }
    }
}
