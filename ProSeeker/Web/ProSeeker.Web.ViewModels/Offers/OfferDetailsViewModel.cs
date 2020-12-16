namespace ProSeeker.Web.ViewModels.Offers
{
    using System;

    using Ganss.XSS;
    using ProSeeker.Common;
    using ProSeeker.Web.ViewModels.Users;

    public class OfferDetailsViewModel : BaseOfferViewModel
    {
        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public bool IsAcountsOwner { get; set; }

        public DateTime? AcceptedOn { get; set; }

        public string AcceptedSentTimeSpan => this.AcceptedOn != null ?
            GlobalMethods.CalculateElapsedTime(DateTime.Parse(this.AcceptedOn.ToString()), false) : null;

        public string SentTimeSpan => GlobalMethods.CalculateElapsedTime(this.CreatedOn, false);

        public string ExpirationCalculated => GlobalMethods.CalculateElapsedTime(this.ExpirationDate, true);

        public virtual SimpleSpecialistDetailsViewModel SpecialistDetails { get; set; }
    }
}
