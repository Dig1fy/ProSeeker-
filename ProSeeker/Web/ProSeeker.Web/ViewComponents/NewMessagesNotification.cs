namespace ProSeeker.Web.ViewComponents
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.PrivateChat;
    using ProSeeker.Web.ViewModels.PrivateChat;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class NewMessagesNotification : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPrivateChatService privateChatService;

        public NewMessagesNotification(
            UserManager<ApplicationUser> userManager,
            IPrivateChatService privateChatService)
        {
            this.userManager = userManager;
            this.privateChatService = privateChatService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = this.userManager.GetUserId((ClaimsPrincipal)this.User);

            if (currentUserId == null)
            {
                return this.View();
            }

            var userConversations = await this.privateChatService.GetAllUserConversationsAsync<ConversationViewModel>(currentUserId);
            var totalUnseenMessages = 0;

            foreach (var conversation in userConversations)
            {
                var convMessages = await this.privateChatService.CheckForUnseenMessagesAsync(conversation.Id, currentUserId);
                if (convMessages != 0)
                {
                    totalUnseenMessages++;
                }
            }

            var model = new NewMessagesNotificationViewModel
            {
                Count = totalUnseenMessages,
                HaveNewMessages = totalUnseenMessages > 0,
            };

            return this.View(model);
        }
    }
}
