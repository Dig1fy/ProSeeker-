namespace ProSeeker.Web.ViewModels.Ads
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class CreateAdInputModel : BaseAdInputModel, IMapFrom<Ad>
    {
        public bool IsVip { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Opinion> Opinions { get; set; }
    }
}
