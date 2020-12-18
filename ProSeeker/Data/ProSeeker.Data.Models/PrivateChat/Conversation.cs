namespace ProSeeker.Data.Models.PrivateChat
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using ProSeeker.Data.Common.Models;

    public class Conversation : BaseDeletableModel<string>
    {
        public Conversation()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ChatMessages = new HashSet<ChatMessage>();
            this.UsersConversations = new HashSet<UserConversation>();
        }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public virtual ICollection<UserConversation> UsersConversations { get; set; }
    }
}
