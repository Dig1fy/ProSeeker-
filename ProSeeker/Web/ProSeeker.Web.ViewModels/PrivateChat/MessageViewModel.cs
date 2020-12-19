namespace ProSeeker.Web.ViewModels.PrivateChat
{
    using System;

    using ProSeeker.Data.Models.PrivateChat;
    using ProSeeker.Services.Mapping;

    public class MessageViewModel : IMapFrom<ChatMessage>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SenderId { get; set; }

        public string SenderUserName { get; set; }

        public string ReceiverId { get; set; }

        public string ConversationId { get; set; }
    }
}
