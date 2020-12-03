namespace ProSeeker.Web.ViewModels.Opinions
{
    using System;

    using Ganss.XSS;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Users;

    public class OpinionViewModel : IMapFrom<Opinion>
    {
        public int Id { get; set; }

        public int? ParentOpinionId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public UserOpinionViewModel Creator { get; set; }

        public string AdId { get; set; }

        public string SpecialistDetailsId { get; set; }
    }
}
