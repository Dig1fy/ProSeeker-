namespace ProSeeker.Services.Data.PrivateChat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models.PrivateChat;

    public interface IPrivateChatService
    {
        Task<ChatMessage> CreateNewMessage(string message, string userId, string receiverId);

        Task<string> GetConversationBySenderAndReceiverIdsAsync(string senderId, string receiverId);

        Task<IEnumerable<T>> GetAllConversationMessagesAsync<T>(string conversationId);
    }
}
