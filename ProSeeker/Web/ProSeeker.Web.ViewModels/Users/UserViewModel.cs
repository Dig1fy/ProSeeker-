namespace ProSeeker.Web.ViewModels.Users
{
    using System;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Users.Specialists;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public City City { get; set; }

        public string ProfilePicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual SpecialistDetailsViewModel SpecialistDetails { get; set; }
    }
}
