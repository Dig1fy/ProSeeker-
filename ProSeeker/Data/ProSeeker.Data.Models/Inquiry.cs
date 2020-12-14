namespace ProSeeker.Data.Models
{
    using System;

    using ProSeeker.Data.Common.Models;

    public class Inquiry : BaseDeletableModel<string>
    {
        public Inquiry()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Content { get; set; }

        public DateTime ValidUntil { get; set; }

        public bool IsRed { get; set; }

        public string SpecialistDetailsId { get; set; }

        public virtual Specialist_Details SpecialistDetails { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int CityId { get; set; }
    }
}
