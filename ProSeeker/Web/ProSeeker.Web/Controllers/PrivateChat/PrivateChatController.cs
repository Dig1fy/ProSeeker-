namespace ProSeeker.Web.Controllers.PrivateChatController
{
    using System.Threading.Tasks;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using ProSeeker.Data.Models;
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

        // chat landing page
        public async Task<IActionResult> Index(string receiverId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var receiver = await this.userManager.FindByIdAsync(receiverId);

            if (user.Id.Equals(receiver.Id))
            {
                return this.Redirect("/");
            }

            var conversationId = await this.privateChatService.GetConversationBySenderAndReceiverIdsAsync(user.Id, receiver.Id);
            var conversationMessages = await this.privateChatService.GetAllConversationMessagesAsync<MessageViewModel>(conversationId);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(string conversationId, string message, string receiverId)
        {
            var sender = await this.userManager.GetUserAsync(this.User);
            var newMessageModel = await this.privateChatService.SendMessageToUserAsync(message, receiverId, sender.Id, conversationId);
            System.Console.WriteLine("CONTROLLER");
            await this.hubContext.Clients.User(newMessageModel.ReceiverId)
                .SendAsync("SendMessage", newMessageModel);

            return this.Json(newMessageModel);
        }

        // TODO : LOGIC IS WORKING ON THE BACK END> HANDLE FRONT END !!!!











        [HttpPost]
        public async Task<IActionResult> JoinConversation(string connectionId, string conversationName)
        {
            await this.hubContext.Groups.AddToGroupAsync(connectionId, conversationName);
            return this.Ok();
        }

        //[HttpPost]
        //public async Task<IActionResult> LeaveConversation(string connectionId, string conversationName)
        //{
        //    await this.hubContext.Groups.RemoveFromGroupAsync(connectionId, conversationName);
        //    return this.Ok();
        //}
    }
}
