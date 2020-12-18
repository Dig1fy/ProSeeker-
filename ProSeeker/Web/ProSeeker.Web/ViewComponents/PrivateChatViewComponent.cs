namespace ProSeeker.Web.ViewComponents
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Web.ViewModels.PrivateChat;

    public class PrivateChatViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;

        public PrivateChatViewComponent(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

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
