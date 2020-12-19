namespace ProSeeker.Services.Data.PrivateChat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models.PrivateChat;
    using ProSeeker.Web.ViewModels.PrivateChat;

    public interface IPrivateChatService
    {
        Task<MessageViewModel> SendMessageToUserAsync(string message, string receiverId, string senderId, string conversationId);

        Task<string> GetConversationBySenderAndReceiverIdsAsync(string senderId, string receiverId);

        Task<IEnumerable<T>> GetAllConversationMessagesAsync<T>(string conversationId);

        //Task ReceiveNewMessage(string message, string receiverId, string senderId, string conversationId);
    }
}
