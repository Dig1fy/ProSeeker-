namespace ProSeeker.Web.ViewModels.PrivateChat
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models.PrivateChat;
    using ProSeeker.Services.Mapping;

    public class ConversationViewModel : IMapFrom<Conversation>
    {
        public string Id { get; set; }

        public string SenderId { get; set; }

        public bool IsSeen { get; set; }

        public int UnseenMessagesCount { get; set; }

        public string OtherPersonsPicture { get; set; }

        public string OtherPersonsId { get; set; }

        public string OtherPersonFullName { get; set; }

        public string ReceiverId { get; set; }

        public virtual IEnumerable<MessageViewModel> ChatMessages { get; set; }
    }
}
