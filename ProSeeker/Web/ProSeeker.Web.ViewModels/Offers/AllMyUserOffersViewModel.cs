namespace ProSeeker.Web.ViewModels.Offers
{
    using System.Collections.Generic;

    public class AllMyUserOffersViewModel
    {
        public IEnumerable<UserOffersViewModel> Offers { get; set; }
    }
}
