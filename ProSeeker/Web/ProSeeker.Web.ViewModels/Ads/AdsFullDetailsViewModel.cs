namespace ProSeeker.Web.ViewModels.Ads
{
    using System;

    using Ganss.XSS;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class AdsFullDetailsViewModel : IMapFrom<Ad>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public int JobCategoryId { get; set; }

        public DateTime CreatedOn { get; set; }

        public JobCategory JobCategory { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        public bool IsVip { get; set; }

        public int Views { get; set; }

        public string PreparedBudget { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
