namespace ProSeeker.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Opinion : BaseDeletableModel<int>
    {
        [MaxLength(15000)]
        public string Content { get; set; }

        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public int? ParentOpinionId { get; set; }

        public virtual Opinion ParentOpinion { get; set; }

        public string AdId { get; set; }

        public virtual Ad Ad { get; set; }

        public string SpecialistDetailsId { get; set; }

        public virtual Specialist_Details SpecialistDetails { get; set; }
    }
}
