namespace ProSeeker.Data.Models
{
    using System;
    using System.Collections.Generic;

    using ProSeeker.Data.Common.Models;

    public class Ad : BaseDeletableModel<string>
    {
        public Ad()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Opinions = new HashSet<Opinion>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public int JobCategoryId { get; set; }

        public JobCategory JobCategory { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        public bool IsVip { get; set; }

        public string UserId { get; set; }

        public string PreparedBudget { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Opinion> Opinions { get; set; }

    }
}
