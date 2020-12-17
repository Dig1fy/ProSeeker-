namespace ProSeeker.Web.Hubs
{
    using System.Threading.Tasks;

    using Ganss.XSS;
    using Microsoft.AspNetCore.SignalR;
    using ProSeeker.Services.Data.PrivateChat;

    public class PrivateChatHub : Hub
    {
        private readonly IPrivateChatService privateChatService;

        public PrivateChatHub(IPrivateChatService privateChatService)
        {
            this.privateChatService = privateChatService;
        }

        public async Task AddToGroup(string groupId, string receiverUsername, string senderUsername)
        {
            await this.Groups
                .AddToGroupAsync(this.Context.ConnectionId, groupId);

            await this.privateChatService
                .AddUserToGroup(groupId, receiverUsername, senderUsername);
        }

        public async Task SendMessage(string groupId, string senderUsername, string receiverUsername, string message)
        {
            var receiverId = await this.privateChatService
                .SendMessageToUser(groupId, senderUsername, receiverUsername, message);

            await this.Clients
                .User(receiverId).SendAsync("ReceiveMessage", senderUsername, new HtmlSanitizer().Sanitize(message.Trim()));
        }

        public async Task ReceiveMessage(string fromUsername, string message, string group)
        {
            var senderId = await this.privateChatService.GetSenderId(fromUsername);
            var sanitizedMessage = new HtmlSanitizer().Sanitize(message);
            await this.Clients.User(senderId).SendAsync("SendMessage", fromUsername, sanitizedMessage.Trim());
        }
    }
}
