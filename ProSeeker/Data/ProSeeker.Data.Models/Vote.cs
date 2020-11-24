namespace ProSeeker.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Vote : BaseModel<int>
    {
        public int OpinionId { get; set; }

        public Opinion Opinion { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public VoteType VoteType { get; set; }
    }
}
