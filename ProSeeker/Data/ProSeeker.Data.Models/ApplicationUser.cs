// ReSharper disable VirtualMemberCallInConstructor
namespace ProSeeker.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;
    using ProSeeker.Data.Common.Models;
    using ProSeeker.Data.Models.PrivateChat;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Votes = new HashSet<Vote>();
            this.Ratings = new HashSet<Rating>();
            this.Opinions = new HashSet<Opinion>();
            this.Ads = new HashSet<Ad>();
            this.Offers = new HashSet<Offer>();
            this.Inquiries = new HashSet<Inquiry>();
            this.ChatMessages = new HashSet<ChatMessage>();
            this.UserConversations = new HashSet<UserConversation>();

            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
        }

        public string FirstName { get; set; }

        [Required(ErrorMessage = "Моля, попълнете полето 'Име'!")]
        [MaxLength(20)]
        public string LastName { get; set; }

        public bool IsOnline { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }

        public DateTime LastVisit { get; set; }

        public bool IsSpecialist { get; set; }

        public string ProfilePicture { get; set; }

        public string SpecialistDetailsId { get; set; }

        public virtual Specialist_Details SpecialistDetails { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<Opinion> Opinions { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }

        public virtual ICollection<Inquiry> Inquiries { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public virtual ICollection<UserConversation> UserConversations { get; set; }
    }
}
