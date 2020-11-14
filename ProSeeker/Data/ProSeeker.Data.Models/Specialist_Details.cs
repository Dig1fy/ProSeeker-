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
            this.Comments = new HashSet<Comment>();

            // this.Opinions = new HashSet<Opinion>();
            // this.Ratings = new HashSet<Rating>();
            // this.Recommendations = new HashSet<Recommendation>();
        }

        // [ForeignKey(nameof(Specialist_Details))]
        // public string Id { get; set; }
        public string AboutMe { get; set; }

        public string CompanyName { get; set; }

        public string Website { get; set; }

        public string WorkActivities { get; set; }

        public int Likes { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int JobCategoryId { get; set; }

        public virtual JobCategory JobCategory { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        // public virtual ICollection<Opinion> Opinions { get; set; }
        // public virtual ICollection<Rating> Ratings { get; set; }
        // public virtual ICollection<Recommendation> Recommendations { get; set; }
    }
}
