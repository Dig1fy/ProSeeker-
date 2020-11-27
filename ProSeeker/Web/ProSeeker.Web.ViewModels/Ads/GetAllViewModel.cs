namespace ProSeeker.Web.ViewModels.Ads
{
    using System.Collections.Generic;

    public class GetAllViewModel
    {
        public IEnumerable<AdsFullDetailsViewModel> Ads { get; set; }
    }
}
