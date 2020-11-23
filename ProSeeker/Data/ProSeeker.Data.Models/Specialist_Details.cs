namespace ProSeeker.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using ProSeeker.Data.Common.Models;

    public class Specialist_Details : BaseDeletableModel<string>
    {
        public Specialist_Details()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Opinions = new HashSet<Opinion>();
            this.Votes = new HashSet<Vote>();
            this.Services = new HashSet<Service>();
            this.Raitings = new HashSet<Raiting>();

            // this.Opinions = new HashSet<Opinion>();
            // this.Recommendations = new HashSet<Recommendation>();
        }

        // [ForeignKey(nameof(Specialist_Details))]
        // public string Id { get; set; }
        public string AboutMe { get; set; }

        public string CompanyName { get; set; }

        public string Website { get; set; }

        public string Experience { get; set; }

        public string Qualification { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int JobCategoryId { get; set; }

        public virtual JobCategory JobCategory { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public virtual ICollection<Opinion> Opinions { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<Raiting> Raitings { get; set; }

        // public virtual ICollection<Opinion> Opinions { get; set; }
        // public virtual ICollection<Recommendation> Recommendations { get; set; }
    }
}
