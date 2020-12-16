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

    public class OfferDetailsSentViewModel : IMapFrom<Offer>
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public decimal Price { get; set; }

        public bool IsAcountsOwner { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime? AcceptedOn { get; set; }

        public string AcceptedSentTimeSpan => this.AcceptedOn != null ?
            GlobalMethods.CalculateElapsedTime(DateTime.Parse(this.AcceptedOn.ToString()), false) : null;

        public string SentTimeSpan => GlobalMethods.CalculateElapsedTime(this.CreatedOn, false);

        public DateTime ExpirationDate { get; set; }

        public string ApplicationUserId { get; set; }

        public string ExpirationCalculated => GlobalMethods.CalculateElapsedTime(this.ExpirationDate, true);

        public virtual SimpleUserViewModel ApplicationUser { get; set; }

        public string SpecialistDetailsId { get; set; }

        public virtual InquiryDetailsViewModel Inquiry { get; set; }

        public virtual AdsFullDetailsViewModel Ad { get; set; }
    }
}
