namespace ProSeeker.Web.ViewModels.Users.Specialists
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Services;

    public class SpecialistDetailsViewModel : IMapFrom<Specialist_Details>
    {
        public string Id { get; set; }

        public string AboutMe { get; set; }

        public string CompanyName { get; set; }

        public string Website { get; set; }

        public string Experience { get; set; }

        public string Qualification { get; set; }

        public ICollection<ServiceViewModel> Services { get; set; }

        public virtual JobCategory JobCategory { get; set; }
    }
}
