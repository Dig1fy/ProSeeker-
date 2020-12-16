namespace ProSeeker.Web.ViewModels.Offers
{
    using Ganss.XSS;
    using ProSeeker.Common;

    public class ExistingOfferViewModel : BaseOfferViewModel
    {
        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public bool IsOfferOwner { get; set; }

        public string SentTimeSpan => GlobalMethods.CalculateElapsedTime(this.CreatedOn, false);

        public string ExpirationCalculated => GlobalMethods.CalculateElapsedTime(this.ExpirationDate, true);
    }
}
