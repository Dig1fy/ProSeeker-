namespace ProSeeker.Web.ViewComponents
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.PrivateChat;
    using ProSeeker.Web.ViewModels.PrivateChat;

    public class UserMessagesViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPrivateChatService privateChatService;

        public UserMessagesViewComponent(
            UserManager<ApplicationUser> userManager,
            IPrivateChatService privateChatService)
        {
            this.userManager = userManager;
            this.privateChatService = privateChatService;
        }

        // Refferences are a bit broken... We call this VC in side nav bar (loginPartial) for all active conversations
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync((ClaimsPrincipal)this.User);

            var conversations = await this.privateChatService.GetAllUserConversationsAsync<ConversationViewModel>(user.Id);
            var updatedConversation = await this.privateChatService.UpdateusersInfoAsync(conversations, user.Id);

            var allConversationsModel = new AllMyConversationsViewModel();
            allConversationsModel.Conversations = updatedConversation;

            return this.View(allConversationsModel);
        }
    }
}
