namespace ProSeeker.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;
    using ProSeeker.Data.Models.PrivateChat;

    public class PrivateChatHub : Hub
    {
        public string GetConnectionId() => this.Context.ConnectionId;

        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync(
                "ReceiveMessage",
                new ChatMessage { ApplicationUserId = "wwww", Content = message, });
        }
    }
}
