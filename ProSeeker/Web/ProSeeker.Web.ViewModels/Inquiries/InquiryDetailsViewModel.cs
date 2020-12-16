
namespace ProSeeker.Web.ViewModels.Inquiries
{
    using System;

    using Ganss.XSS;
    using ProSeeker.Common;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Offers;
    using ProSeeker.Web.ViewModels.Users;

    public class InquiryDetailsViewModel : BaseInquiryViewModel
    {
        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public bool IsAcountOwner { get; set; }

        public DateTime ValidUntil { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SentTimeSpan => GlobalMethods.CalculateElapsedTime(this.CreatedOn, false);

        public string ValidUntilTimeSpan => GlobalMethods.CalculateElapsedTime(this.ValidUntil, true);

        public bool IsRed { get; set; }

        public string SpecialistDetailsId { get; set; }

        public string UserId { get; set; }

        public virtual SimpleUserViewModel User { get; set; }

        public int CityId { get; set; }

        public virtual ExistingOfferViewModel Offer{ get; set; }
    }
}
