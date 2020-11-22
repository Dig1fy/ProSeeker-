
namespace ProSeeker.Data.Models
{
    using System.Collections.Generic;

    using ProSeeker.Data.Common.Models;

    public class City : BaseModel<int>
    {
        public City()
        {
            this.ApplicationUsers = new HashSet<ApplicationUser>();
        }

        public string Name { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
