namespace ProSeeker.Web.ViewModels.Users
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Cities;

    public class SimpleUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePicture { get; set; }

        public CitySimpleViewModel City { get; set; }


    }
}
