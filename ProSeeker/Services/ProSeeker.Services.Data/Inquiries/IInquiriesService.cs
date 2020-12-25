namespace ProSeeker.Services.Data.Inquiries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Inquiries;

    public interface IInquiriesService
    {
        Task CreateAsync(CreateInquiryInputModel inputModel);

        Task<IEnumerable<T>> GetSpecialistEnquiriesAsync<T>(string specialistId);

        Task<T> GetDetailsByIdAsync<T>(string inquiryId);

        Task MarkInquiryAsRedAsync(string inquiryId);

        Task DeleteByIdAsync(string inquiryId);

        Task<T> CheckForExistingOfferAsync<T>(string inquiryId);

        // Invoked in view component
        bool IsThereUnredInquiry(string userId);

        int UnredInquiriesCount(string userId);
    }
}
