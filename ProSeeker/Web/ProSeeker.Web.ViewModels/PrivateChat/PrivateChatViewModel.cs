namespace ProSeeker.Web.ViewModels.PrivateChat
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.Chat;

    public class PrivateChatViewModel
    {
        public ApplicationUser Sender { get; set; }

        public ApplicationUser Receiver { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();

        public string GroupId { get; set; }
    }

}
