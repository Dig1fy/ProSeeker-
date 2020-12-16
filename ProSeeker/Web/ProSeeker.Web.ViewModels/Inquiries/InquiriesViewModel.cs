namespace ProSeeker.Web.ViewModels.Inquiries
{
    using System;

    using Ganss.XSS;
    using ProSeeker.Common;
    using ProSeeker.Web.ViewModels.Users;

    public class InquiriesViewModel : BaseInquiryViewModel
    {
        public string ShortSanitizedContent => new HtmlSanitizer().Sanitize(this.ShortContent);

        public DateTime ValidUntil { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SentTimeSpan => GlobalMethods.CalculateElapsedTime(this.CreatedOn, false);

        public bool IsRed { get; set; }

        public string SpecialistDetailsId { get; set; }

        public string UserId { get; set; }

        public virtual SimpleUserViewModel User { get; set; }

        public int CityId { get; set; }

        private string ShortContent => this.Content.Length > 200 ? $"{this.Content.Substring(0, 200)}..." : this.Content;
    }
}
