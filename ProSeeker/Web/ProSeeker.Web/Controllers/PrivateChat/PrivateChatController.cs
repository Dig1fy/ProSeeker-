namespace ProSeeker.Web.Controllers.PrivateChatController
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.PrivateChat;
    using ProSeeker.Services.Data.PrivateChat;
    using ProSeeker.Web.Hubs;
    using ProSeeker.Web.ViewModels.PrivateChat;

    [Authorize]
    //[Route("[controller]")]
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

        public async Task<IActionResult> Index (string receiverId, string group)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var receiver = await this.userManager.FindByIdAsync(receiverId);
            var conversationId = await this.privateChatService.GetConversationBySenderAndReceiverIdsAsync(user.Id, receiver.Id);
            var conversationMessages = await this.privateChatService.GetAllConversationMessagesAsync<MessageViewModel>(conversationId);

            var model = new PrivateChatViewModel
            {
                Sender = user,
                Receiver = receiver,
                ConversationId = conversationId,
                ChatMessages = conversationMessages,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> JoinConversation(string connectionId, string conversationName)
        {
            await this.hubContext.Groups.AddToGroupAsync(connectionId, conversationName);
            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LeaveConversation(string connectionId, string conversationName)
        {
            await this.hubContext.Groups.RemoveFromGroupAsync(connectionId, conversationName);
            return this.Ok();
        }

        public async Task<IActionResult> SendMessage(string conversationId, string message, string conversationName, string receiverId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var newMessage = this.privateChatService.CreateNewMessage(message, user.Id, receiverId);

            await this.hubContext.Clients.Group(conversationName).SendAsync("ReceiveMessage", newMessage);
            return this.Ok();
        }
    }
}
