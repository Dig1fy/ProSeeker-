namespace ProSeeker.Data.Models.PrivateChat
{
    using System;
    using System.Collections.Generic;

    using ProSeeker.Data.Common.Models;

    public class Conversation : BaseDeletableModel<string>
    {
        public Conversation()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ChatMessages = new HashSet<ChatMessage>();
            this.UsersConversations = new HashSet<UserConversation>();
        }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public virtual ICollection<UserConversation> UsersConversations { get; set; }
    }
}
