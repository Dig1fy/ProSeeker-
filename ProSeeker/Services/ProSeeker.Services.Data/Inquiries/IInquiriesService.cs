namespace ProSeeker.Services.Data.Inquiries
{
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Inquiries;

    public interface IInquiriesService
    {
        Task CreateAsync(InquiryInputModel inputModel);
    }
}
