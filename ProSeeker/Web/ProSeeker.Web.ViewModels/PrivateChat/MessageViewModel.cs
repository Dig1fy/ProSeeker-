namespace ProSeeker.Web.ViewModels.PrivateChat
{
    using ProSeeker.Data.Models.PrivateChat;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Users;
    using System;

    public class MessageViewModel : IMapFrom<ChatMessage>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        public string ConversationId { get; set; }
    }
}
