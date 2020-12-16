namespace ProSeeker.Web.ViewModels.Offers
{
    using System;

    using Ganss.XSS;
    using ProSeeker.Common;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Ads;
    using ProSeeker.Web.ViewModels.Inquiries;
    using ProSeeker.Web.ViewModels.Users;

    public class SpecialistOffersViewModel : BaseOfferViewModel
    {
        public string ShortSanitizedDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);

        public string SentTimeSpan => GlobalMethods.CalculateElapsedTime(this.CreatedOn, false);

        public AdsShortDetailsViewModel Ad { get; set; }

        public InquirySimpleInfoViewModel Inquiry { get; set; }

        public virtual SimpleUserViewModel ApplicationUser { get; set; }

        private string ShortDescription => this.Description.Length > 200 ? $"{this.Description.Substring(0, 200)}..." : this.Description;
    }
}
