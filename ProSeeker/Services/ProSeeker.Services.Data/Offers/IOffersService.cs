namespace ProSeeker.Services.Data.Offers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ProSeeker.Web.ViewModels.EmailsSender;
    using ProSeeker.Web.ViewModels.Offers;

    public interface IOffersService
    {
        Task<string> CreateFromAdAsync(CreateOfferInputModel inputModel);

        Task CreateFromInquiryAsync(CreateOfferInputModel inputModel);

        Task<IEnumerable<T>> GetAllUserOffersAsync<T>(string userId);

        Task<IEnumerable<T>> GetAllSpecialistOffersAsync<T>(string specialistId);

        Task<T> GetDetailsByIdAsync<T>(string offerId);

        Task DeleteByIdAsync(string id);

        Task MarkOfferAsRedAsync(string offerId);

        Task<T> GetExistingOfferAsync<T>(string currentAdId, string userId, string specialistId);

        Task AcceptOffer(string offerId);

        // These two are being called from VC so they cannot be async
        bool IsThereUnredOffer(string userId);

        int GetUnredOffersCount(string userId);

        Task<SendEmailViewModel> GetOffersSenderAndReceiverDataByOfferIdAsync(string offerId, string userId);
    }
}
