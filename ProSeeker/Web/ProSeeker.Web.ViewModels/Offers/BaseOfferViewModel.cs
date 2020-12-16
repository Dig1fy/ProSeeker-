namespace ProSeeker.Web.ViewModels.Offers
{
    using System;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public abstract class BaseOfferViewModel : IMapFrom<Offer>
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsRed { get; set; }

        public bool IsAccepted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
