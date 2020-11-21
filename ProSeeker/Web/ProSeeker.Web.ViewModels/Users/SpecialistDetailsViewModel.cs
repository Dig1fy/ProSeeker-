namespace ProSeeker.Web.ViewModels.Users.Specialists
{
    using System.Collections.Generic;

    using Ganss.XSS;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Services;

    public class SpecialistDetailsViewModel : IMapFrom<Specialist_Details>
    {
        public string Id { get; set; }

        public string AboutMe { get; set; }

        public string SanitezedAboutMe => new HtmlSanitizer().Sanitize(this.AboutMe);

        public string CompanyName { get; set; }

        public string Website { get; set; }

        public string Experience { get; set; }

        public string SanitizedExperience => new HtmlSanitizer().Sanitize(this.Experience);

        public string Qualification { get; set; }

        public string SanitizedQualification => new HtmlSanitizer().Sanitize(this.Qualification);

        public ICollection<ServiceViewModel> Services { get; set; }

        public virtual JobCategory JobCategory { get; set; }
    }
}
