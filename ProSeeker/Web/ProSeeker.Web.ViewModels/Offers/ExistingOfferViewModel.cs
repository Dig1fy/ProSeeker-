namespace ProSeeker.Web.ViewModels.Offers
{
    using System;

    using Ganss.XSS;
    using ProSeeker.Common;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class ExistingOfferViewModel : IMapFrom<Offer>
    {
        public string Id { get; set; }

        //public string ApplicationUserId { get; set; }

        //public string SpecialistDetailsId { get; set; }

        //public string AdId { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public bool IsOfferOwner { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SentTimeSpan => GlobalMethods.CalculateElapsedTime(this.CreatedOn, false);

        public DateTime ExpirationDate { get; set; }

        public string ExpirationCalculated => GlobalMethods.CalculateElapsedTime(this.ExpirationDate, true);
    }
}
