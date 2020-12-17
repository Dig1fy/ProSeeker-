namespace ProSeeker.Data.Models.Chat
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using ProSeeker.Data.Common.Models;

    public class ChatMessage : BaseDeletableModel<string>
    {
        public ChatMessage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Content { get; set; }

        [Required]
        [ForeignKey(nameof(Group))]
        public string GroupId { get; set; }

        public Group Group { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string SenderId { get; set; }

        public ApplicationUser Sender { get; set; }

        [Required]
        public string ReceiverUsername { get; set; }
    }
}
