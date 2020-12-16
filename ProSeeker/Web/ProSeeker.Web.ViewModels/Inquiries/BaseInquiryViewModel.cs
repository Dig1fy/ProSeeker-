namespace ProSeeker.Web.ViewModels.Inquiries
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public abstract class BaseInquiryViewModel : IMapFrom<Inquiry>
    {
        public string Id { get; set; }

        public string Content { get; set; }
    }
}
