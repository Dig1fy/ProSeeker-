namespace ProSeeker.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class Ad : BaseDeletableModel<string>
    {
        public Ad()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Opinions = new HashSet<Opinion>();
            this.Votes = new HashSet<Vote>();
        }

        [Required]
        [MaxLength(95)]
        public string Title { get; set; }

        [Required]
        [MaxLength(25000)]
        public string Description { get; set; }

        public int JobCategoryId { get; set; }

        public JobCategory JobCategory { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        public bool IsVip { get; set; }

        public DateTime VipExpirationDate { get; set; }

        public int Views { get; set; }

        [Required]
        [MaxLength(150)]
        public string PreparedBudget { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Opinion> Opinions { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

    }
}
