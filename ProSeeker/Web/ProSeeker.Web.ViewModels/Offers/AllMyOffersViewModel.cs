namespace ProSeeker.Web.ViewModels.Offers
{
    using System.Collections.Generic;

    public class AllMyOffersViewModel
    {
        public IEnumerable<UserOffersViewModel> Offers { get; set; }
    }
}
