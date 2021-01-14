namespace ProSeeker.Web.ViewModels.Offers
{
    using System.ComponentModel.DataAnnotations;

    public class MissingOfferInputModel
    {
        public string OfferId { get; set; }

        [Required (ErrorMessage = "Моля, попълнете полето 'Актуален телефонен номер'!")]
        [Display(Name = "Актуален телефонен номер")]
        public string UserPhoneNumber { get; set; }
    }
}
