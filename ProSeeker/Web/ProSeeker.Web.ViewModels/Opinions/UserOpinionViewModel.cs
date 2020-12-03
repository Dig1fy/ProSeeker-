namespace ProSeeker.Web.ViewModels.Opinions
{
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class UserOpinionViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public string ProfilePicture { get; set; }
    }
}
