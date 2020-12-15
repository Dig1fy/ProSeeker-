namespace ProSeeker.Services.Data.Offers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Offers;

    public interface IOffersService
    {
        Task<string> CreateAsync(CreateOfferInputModel inputModel, string specialistId);

        Task<IEnumerable<T>> GetAllUserOffersAsync<T>(string userId);

        Task<T> GetDetailsByIdAsync<T>(string offerId);

        Task DeleteByIdAsync(string id);

        Task MarkOfferAsRedAsync(string offerId);

        Task<T> GetExistingOfferAsync<T>(string currentAdId, string userId, string specialistId);

        Task AcceptOffer(string offerId);

        // These two are being called from VC so they cannot be async
        bool IsThereUnredOffer(string userId);

        int GetUnredOffersCount(string userId);
    }
}
