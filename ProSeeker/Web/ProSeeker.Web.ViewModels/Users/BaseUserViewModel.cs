namespace ProSeeker.Web.ViewModels.Users
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public abstract class BaseUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicture { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";
    }
}
