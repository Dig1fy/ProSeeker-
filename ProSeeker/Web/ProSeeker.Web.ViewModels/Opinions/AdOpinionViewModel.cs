namespace ProSeeker.Web.ViewModels.Opinions
{
    using System;

    using Ganss.XSS;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Users;

    public class AdOpinionViewModel : IMapFrom<Opinion>
    {
        public int Id { get; set; }

        public int? ParentOpinionId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public UserViewModel Creator { get; set; }
        //public string CreatorFirstName { get; set; }

        //public string CreatorLastName { get; set; }

        //public string CreatorFullName => $"{this.CreatorFirstName} {this.CreatorLastName}";

        //public string CreatorProfilePicture { get; set; }
    }
}
