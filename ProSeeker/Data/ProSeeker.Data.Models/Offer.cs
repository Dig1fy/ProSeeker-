﻿namespace ProSeeker.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Offer : BaseModel<string>
    {
        public Offer()
        {
            this.Id = Guid.NewGuid().ToString();        // TODO - Add user/specialist [Required] when the logic's been implemented and tested!
        }

        [Required]
        public string AdId { get; set; }

        public virtual Ad Ad { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string StartDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string SpecialistDetailsId { get; set; }

        public virtual Specialist_Details SpecialistDetails { get; set; }

    }
}
