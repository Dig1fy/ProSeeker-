namespace ProSeeker.Web.ViewModels.Offers
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class OfferDetailsViewModel : IMapFrom<Offer>
    {
        public string Id { get; set; }

    }
}
