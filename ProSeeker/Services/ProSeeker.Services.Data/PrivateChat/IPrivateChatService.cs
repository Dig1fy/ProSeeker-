namespace ProSeeker.Services.Data.PrivateChat
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models.Chat;

    public interface IPrivateChatService
    {
        Task AddUserToGroup(string groupName, string receiverUsername, string senderUsername);

        // return receiverId
        Task<string> SendMessageToUser(string groupName, string senderUsername, string receiverUsername, string message);

        //Task ReceiveNewMessage(string groupName, string senderUsername, string message);

        //Task<ICollection<ChatMessage>> GetAllMessages(string group);
    }
}
