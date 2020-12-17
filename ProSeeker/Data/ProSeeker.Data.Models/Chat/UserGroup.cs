namespace ProSeeker.Data.Models.Chat
{
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class UserGroup : BaseDeletableModel<string>
    {
        [Required]
        public string GroupId { get; set; }

        public Group Group { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
