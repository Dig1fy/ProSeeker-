namespace ProSeeker.Data.Models
{
    using ProSeeker.Data.Common.Models;
    using System.ComponentModel.DataAnnotations;

    public class Vote : BaseModel<int>
    {
        public string SpecialistDetailsId { get; set; }

        public Specialist_Details SpecialistDetails { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public VoteType VoteType { get; set; }
    }
}
