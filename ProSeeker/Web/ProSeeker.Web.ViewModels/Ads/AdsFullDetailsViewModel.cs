namespace ProSeeker.Web.ViewModels.Ads
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Ganss.XSS;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Opinions;

    public class AdsFullDetailsViewModel : BaseAdViewModel, IHaveCustomMappings
    {
        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string UserId { get; set; }

        public bool IsOwnerOfAd { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<OpinionViewModel> Opinions { get; set; }
    }
}
