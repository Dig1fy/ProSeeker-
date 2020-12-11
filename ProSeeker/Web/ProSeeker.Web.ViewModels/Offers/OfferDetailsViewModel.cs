namespace ProSeeker.Web.ViewModels.Offers
{
    using System;

    using Ganss.XSS;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Users;

    public class OfferDetailsViewModel : IMapFrom<Offer>
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public decimal Price { get; set; }

        public DateTime ExpirationDate { get; set; }

        public virtual SimpleSpecialistDetailsViewModel SpecialistDetails { get; set; }
    }
}
