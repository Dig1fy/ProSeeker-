namespace ProSeeker.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Service : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1250)]
        public string Description { get; set; }

        public string SpecialistDetailsId { get; set; }

        public Specialist_Details SpecialistDetails { get; set; }
    }
}
