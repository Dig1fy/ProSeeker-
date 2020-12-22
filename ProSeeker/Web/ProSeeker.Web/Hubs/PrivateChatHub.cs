namespace ProSeeker.Web.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using ProSeeker.Common;
    using ProSeeker.Data.Models;
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
            var messageViewModel = await this.privateChatService.SendMessageToUserAsync(message, receiverId, senderId, conversationId);
            var adjustedDateTime = messageViewModel.CreatedOn;
            messageViewModel.CreatedOn = DateTime.Parse(adjustedDateTime.ToString("O"));
            await this.Clients.Users(messageViewModel.ReceiverId).SendAsync(GlobalConstants.SendMessagePrivateChatMethod, messageViewModel);
        }
    }
}
