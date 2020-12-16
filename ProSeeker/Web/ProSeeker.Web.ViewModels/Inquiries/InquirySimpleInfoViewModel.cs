namespace ProSeeker.Web.ViewModels.Inquiries
{
    using Ganss.XSS;

    public class InquirySimpleInfoViewModel : BaseInquiryViewModel
    {
        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.ShortContent);

        private string ShortContent => this.Content.Length > 50 ? this.Content.Substring(0, 50) : this.Content;
    }
}
