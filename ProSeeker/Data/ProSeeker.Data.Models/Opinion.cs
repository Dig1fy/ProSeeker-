namespace ProSeeker.Data.Models
{
    using System;

    using ProSeeker.Data.Common.Models;

    public class Opinion : BaseDeletableModel<int>
    {
        public string SpecialistDetailsId { get; set; }

        public virtual Specialist_Details SpecialistDetails { get; set; }

        public string Content { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public string ParentOpinionId { get; set; }

        public virtual Opinion ParentOpinion { get; set; }
    }
}
