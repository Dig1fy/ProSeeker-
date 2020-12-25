namespace ProSeeker.Web.ViewModels.Users
{
    using ProSeeker.Web.ViewModels.Cities;

    public class SimpleUserViewModel : BaseUserViewModel
    {
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string SpecialistDetailsId { get; set; }

        public CitySimpleViewModel City { get; set; }
    }
}
