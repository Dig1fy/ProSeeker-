namespace ProSeeker.Services.Data.PrivateChat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models.Chat;

    public interface IPrivateChatService
    {
        Task AddUserToGroup(string groupId, string receiverUsername, string senderUsername);

        // return receiverId
        Task<string> SendMessageToUser(string groupName, string senderUsername, string receiverUsername, string message);

        Task<string> GetSenderId(string senderUsername);

        Task<ICollection<ChatMessage>> GetAllMessages(string group);

        Task<string> GetGroupNameById(string groupId);
    }
}
