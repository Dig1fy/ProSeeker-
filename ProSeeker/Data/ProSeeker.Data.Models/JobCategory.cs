namespace ProSeeker.Data.Models
{
    using System.Collections.Generic;

    using ProSeeker.Data.Common.Models;

    public class JobCategory : BaseDeletableModel<int>
    {
        public JobCategory()
        {
            this.SpecialistsDetails = new HashSet<Specialist_Details>();
        }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Specialist_Details> SpecialistsDetails { get; set; }
    }
}
