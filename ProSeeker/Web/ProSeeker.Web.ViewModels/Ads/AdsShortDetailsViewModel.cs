namespace ProSeeker.Web.ViewModels.Ads
{
    using Ganss.XSS;

    public class AdsShortDetailsViewModel : BaseAdViewModel
    {
        public string ShortTitle
           => this.Title.Length > 50
           ? this.Title.Substring(0, 50) + ". . ."
           : this.Title;

        public string SanitizedShortDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);

        private string ShortDescription
           => this.Description.Length > 200
           ? this.Description.Substring(0, 200) + ". . ."
           : this.Description;
    }
}
