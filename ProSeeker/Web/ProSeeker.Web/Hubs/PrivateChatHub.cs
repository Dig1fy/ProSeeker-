namespace ProSeeker.Web.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.PrivateChat;
    using ProSeeker.Services.Data.PrivateChat;

    public class PrivateChatHub : Hub
    {
        private readonly IPrivateChatService privateChatService;
        private readonly UserManager<ApplicationUser> userManager;

        public PrivateChatHub(
            IPrivateChatService privateChatService, 
            UserManager<ApplicationUser> userManager)
        {
            this.privateChatService = privateChatService;
            this.userManager = userManager;
        }

        public string GetConnectionId() => this.Context.ConnectionId;

        public async Task Send(string message, string receiverId, string senderId, string conversationId)
        {
            var senderUser = await this.userManager.FindByIdAsync(senderId);
            var messageViewModel = await this.privateChatService.SendMessageToUserAsync(message, receiverId, senderId, conversationId);

            //await this.Clients.User(messageViewModel.ReceiverId).SendAsync("SendMessage", messageViewModel);
            await this.Clients.All.SendAsync("SendMessage", messageViewModel);
        }

        //public async Task Receive(string message, string receiverId, string senderId, string conversationId)
        //{
        //    await this.Clients.User(receiverId).SendAsync("SendMessage", message);
        //    //await this.privateChatService.ReceiveNewMessage(message, receiverId, senderId, conversationId);
        //}
    }
}
