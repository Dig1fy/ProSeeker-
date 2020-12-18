namespace ProSeeker.Web.ViewModels.PrivateChat
{
    using System.Collections.Generic;

    public class AllMessagesViewModel
    {
        public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}
