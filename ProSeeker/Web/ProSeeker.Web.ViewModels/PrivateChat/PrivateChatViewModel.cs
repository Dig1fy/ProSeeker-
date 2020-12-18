namespace ProSeeker.Web.ViewModels.PrivateChat
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models;

    public class PrivateChatViewModel
    {
        public ApplicationUser Sender { get; set; }

        public ApplicationUser Receiver { get; set; }

        public ICollection<MessageViewModel> ChatMessages { get; set; } = new HashSet<MessageViewModel>();

        public string ConversationId { get; set; }
    }
}
