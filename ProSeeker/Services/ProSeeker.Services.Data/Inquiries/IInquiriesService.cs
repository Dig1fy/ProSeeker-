namespace ProSeeker.Services.Data.Inquiries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Inquiries;

    public interface IInquiriesService
    {
        Task CreateAsync(InquiryInputModel inputModel);

        Task<IEnumerable<T>> GetSpecialistEnquiriesAsync<T>(string specialistId);

        Task<T> GetDetailsByIdAsync<T>(string inquiryId);

        Task MarkInquiryAsRedAsync(string inquiryId);

        Task DeleteByIdAsync(string inquiryId);
    }
}
