namespace ProSeeker.Web.ViewModels.Inquiries
{
    using Ganss.XSS;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class InquirySimpleInfoViewModel : IMapFrom<Inquiry>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        private string ShortContent => this.Content.Length > 50 ? this.Content.Substring(0, 50) : this.Content;

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.ShortContent);
    }
}
