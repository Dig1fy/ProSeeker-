namespace ProSeeker.Web.ViewModels.Offers
{
    using System;

    using Ganss.XSS;
    using ProSeeker.Common;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Ads;
    using ProSeeker.Web.ViewModels.Users;

    public class UserOffersViewModel : IMapFrom<Offer>
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string ShortSanitizedDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);

        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SentTimeSpan => GlobalMethods.CalculateElapsedTime(this.CreatedOn);

        public DateTime ExpirationDate { get; set; }

        public string ApplicationUserId { get; set; }

        public AdsShortDetailsViewModel Ad { get; set; }

        public virtual SimpleSpecialistDetailsViewModel SpecialistDetails { get; set; }

        private string ShortDescription => this.Description.Length > 200 ? $"{this.Description.Substring(0, 200)}..." : this.Description;
    }
}
