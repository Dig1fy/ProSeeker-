namespace ProSeeker.Web.ViewModels.Users.Specialists
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Ganss.XSS;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Services;

    public class SpecialistDetailsViewModel : IMapFrom<Specialist_Details>, IHaveCustomMappings
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

        public double AverageRaiting { get; set; }

        public int RaitingsCount { get; set; }

        public ICollection<ServiceViewModel> Services { get; set; }

        public virtual JobCategory JobCategory { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Specialist_Details, SpecialistDetailsViewModel>()
                 .ForMember(x => x.AverageRaiting, opt =>
                   {
                       opt.MapFrom(m => m.Raitings.Average(v => v.Value));
                   })
                 .ForMember(y => y.RaitingsCount, opt =>
                  {
                      opt.MapFrom(m => m.Raitings.Count());
                  });
        }
    }
}
