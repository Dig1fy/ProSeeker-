namespace ProSeeker.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class SpecialistsLikes
    {
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [ForeignKey(nameof(Specialist_Details))]
        public string SpecialistDetailsId { get; set; }

        public Specialist_Details SpecialistDetails { get; set; }

        public bool IsLiked { get; set; }
    }
}
