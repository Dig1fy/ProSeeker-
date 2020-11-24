namespace ProSeeker.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Rating : BaseModel<int>
    {
        public string SpecialistDetailsId { get; set; }

        public Specialist_Details SpecialistDetails { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int Value { get; set; }
    }
}
