namespace ProSeeker.Web.ViewModels.Offers
{
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class CreateOfferViewModel : IMapFrom<Offer>
    {
        public string Id { get; set; }

        public string AdId { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string StartDate { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
