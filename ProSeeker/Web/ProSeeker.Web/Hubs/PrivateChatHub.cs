namespace ProSeeker.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;
    using ProSeeker.Data.Models.PrivateChat;
    using ProSeeker.Services.Data.PrivateChat;

    public class PrivateChatHub : Hub
    {
        private readonly IPrivateChatService privateChatService;

        public PrivateChatHub(IPrivateChatService privateChatService)
        {
            this.privateChatService = privateChatService;
        }

        public string GetConnectionId() => this.Context.ConnectionId;

        public async Task Send(string message, string receiverId, string senderId, string conversationId)
        {
            var messageViewModel = await this.privateChatService.SendMessageToUserAsync(message, receiverId, senderId, conversationId);
            System.Console.WriteLine("FAFA");
            await this.Clients.User(messageViewModel.ReceiverId).SendAsync("SendMessage", messageViewModel);

            //await this.Clients.All.SendAsync(
            //    "ReceiveMessage",
            //    new ChatMessage { ApplicationUserId = "wwww", Content = message, });
        }
    }
}
