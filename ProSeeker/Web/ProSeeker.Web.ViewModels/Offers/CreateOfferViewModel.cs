namespace ProSeeker.Web.ViewModels.Offers
{
    using ProSeeker.Web.ViewModels.Inquiries;

    public class CreateOfferViewModel : BaseOfferViewModel
    {
        public string AdId { get; set; }

        public string InquiryId { get; set; }

        public virtual InquirySimpleInfoViewModel Inquiry { get; set; }

        public string StartDate { get; set; }
    }
}
