namespace ProSeeker.Web.ViewModels.PrivateChat
{
    using System.Collections.Generic;

    public class AllMyConversationsViewModel
    {
        public virtual IEnumerable<ConversationViewModel> Conversations { get; set; }
    }
}
