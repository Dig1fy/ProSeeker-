namespace ProSeeker.Web.ViewModels.Ads
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class AdShortDetailsViewModel : IMapFrom<Ad>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ShortTitle =>
            this.Title.Length > 30 ? this.Title.Substring(0, 30) + "..." : this.Title;

        public string Description { get; set; }

        public string ShortDescription =>
            this.Description.Length > 50 ? this.Description.Substring(0, 50) + "..." : this.Description;

        public int JobCategoryId { get; set; }

        public JobCategory JobCategory { get; set; }

        public int CityId { get; set; }

        public bool IsVip { get; set; }

        public int Views { get; set; }

        public string PreparedBudget { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Opinion> Opinions { get; set; }
    }
}
