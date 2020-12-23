namespace ProSeeker.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Offer : BaseDeletableModel<string>
    {
        public Offer()
        {
            this.Id = Guid.NewGuid().ToString();        // TODO - Add user/specialist [Required] when the logic's been implemented and tested!
        }

        public string InquiryId { get; set; }

        public virtual Inquiry Inquiry { get; set; }

        public string AdId { get; set; }

        public virtual Ad Ad { get; set; }

        [Required]
        [MaxLength(25000)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MaxLength(150)]
        public string StartDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsRed { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime? AcceptedOn { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string SpecialistDetailsId { get; set; }

        public virtual Specialist_Details SpecialistDetails { get; set; }
    }
}
