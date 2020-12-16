namespace ProSeeker.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;

    using ProSeeker.Web.ViewModels.Cities;
    using ProSeeker.Web.ViewModels.Opinions;
    using ProSeeker.Web.ViewModels.Users.Specialists;

    public class UserViewModel : BaseUserViewModel
    {
        public CitySimpleViewModel City { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual SpecialistDetailsViewModel SpecialistDetails { get; set; }

        public virtual ICollection<OpinionViewModel> Opinions { get; set; }
    }
}
