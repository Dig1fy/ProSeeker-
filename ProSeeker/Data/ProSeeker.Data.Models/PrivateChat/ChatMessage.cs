namespace ProSeeker.Data.Models.PrivateChat
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class ChatMessage : BaseDeletableModel<string>
    {
        public ChatMessage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Content { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public string ConversationId { get; set; }

        public Conversation Conversation { get; set; }
    }
}
