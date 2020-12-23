namespace ProSeeker.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class City : BaseModel<int>
    {
        public City()
        {
            this.ApplicationUsers = new HashSet<ApplicationUser>();
            this.Ads = new HashSet<Ad>();
        }

        [Required]
        [MaxLength(70)]
        public string Name { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
    }
}
