namespace ProSeeker.Data.Models
{
    using System.Collections.Generic;

    using ProSeeker.Data.Common.Models;

    public class Opinion : BaseDeletableModel<int>
    {
        public Opinion()
        {
            this.Votes = new HashSet<Vote>();
        }

        public string Content { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public int? ParentOpinionId { get; set; }

        public virtual Opinion ParentOpinion { get; set; }

        public string AdId { get; set; }

        public virtual Ad Ad { get; set; }

        public string SpecialistDetailsId { get; set; }

        public virtual Specialist_Details SpecialistDetails { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
