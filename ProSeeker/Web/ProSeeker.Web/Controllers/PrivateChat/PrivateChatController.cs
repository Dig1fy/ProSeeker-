namespace ProSeeker.Web.Controllers.PrivateChatController
{
    using System.Threading.Tasks;

    using Ganss.XSS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using ProSeeker.Common;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.PrivateChat;
    using ProSeeker.Web.Hubs;
    using ProSeeker.Web.ViewModels.PrivateChat;

    [Authorize]
    public class PrivateChatController : BaseController
    {
        private readonly IHubContext<PrivateChatHub> hubContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPrivateChatService privateChatService;

        public PrivateChatController(
            IHubContext<PrivateChatHub> hubContext,
            UserManager<ApplicationUser> userManager,
            IPrivateChatService privateChatService)
        {
            this.hubContext = hubContext;
            this.userManager = userManager;
            this.privateChatService = privateChatService;
        }

        // Refference: opening a chat in the nav bar (LoginPartial / Съобщения)
        public async Task<IActionResult> Index(string receiverId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var receiver = await this.userManager.FindByIdAsync(receiverId);

            if (user.Id.Equals(receiver.Id))
            {
                return this.Redirect(GlobalConstants.HomePageRedirect);
            }

            var conversationId = await this.privateChatService.GetConversationBySenderAndReceiverIdsAsync(user.Id, receiver.Id);

            if (conversationId == null)
            {
                return this.CustomNotFound();
            }

            var conversationMessages = await this.privateChatService.GetAllConversationMessagesAsync<MessageViewModel>(conversationId);
            await this.privateChatService.MarkAllMessagesOfTheCurrentUserAsSeenAsync(conversationId, user.Id);

            foreach (var message in conversationMessages)
            {
                message.Content = new HtmlSanitizer().Sanitize(message.Content);
            }

            var model = new PrivateChatViewModel
            {
                Sender = user,
                Receiver = receiver,
                ConversationId = conversationId,
                ChatMessages = conversationMessages,
            };

            return this.View(model);
        }
    }
}
