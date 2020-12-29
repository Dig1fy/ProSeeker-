namespace ProSeeker.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Web.ViewModels.PrivateChat;

    public class PrivateChatViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string receiverId, string senderId, string conversationId)
        {
            var inputModel = new MessageViewModel
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ConversationId = conversationId,
            };

            return this.View(inputModel);
        }
    }
}
