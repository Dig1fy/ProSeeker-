namespace ProSeeker.Web.ViewModels.Offers
{
    using Ganss.XSS;
    using ProSeeker.Common;
    using ProSeeker.Web.ViewModels.Ads;
    using ProSeeker.Web.ViewModels.Inquiries;
    using ProSeeker.Web.ViewModels.Users;

    public class UserOffersViewModel : BaseOfferViewModel
    {
        public string ShortSanitizedDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);

        public string SentTimeSpan => GlobalMethods.CalculateElapsedTime(this.CreatedOn, false);

        public AdsShortDetailsViewModel Ad { get; set; }

        public InquirySimpleInfoViewModel Inquiry { get; set; }

        public virtual SimpleSpecialistDetailsViewModel SpecialistDetails { get; set; }

        private string ShortDescription => this.Description.Length > 200 ? $"{this.Description.Substring(0, 200)}..." : this.Description;
    }
}
