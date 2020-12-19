namespace ProSeeker.Services.Data.PrivateChat
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ganss.XSS;
    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.PrivateChat;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.PrivateChat;

    public class PrivateChatService : IPrivateChatService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<ChatMessage> messageRepository;
        private readonly IDeletableEntityRepository<Conversation> conversationRepository;
        private readonly IRepository<UserConversation> userConversationRepository;

        public PrivateChatService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ChatMessage> messageRepository,
            IDeletableEntityRepository<Conversation> conversationRepository,
            IRepository<UserConversation> userConversationRepository)
        {
            this.usersRepository = usersRepository;
            this.messageRepository = messageRepository;
            this.conversationRepository = conversationRepository;
            this.userConversationRepository = userConversationRepository;
        }

        public async Task<MessageViewModel> SendMessageToUserAsync(string message, string receiverId, string senderId, string conversationId)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.Id == senderId);
            var conversation = await this.conversationRepository
                .All()
                .FirstOrDefaultAsync(c => (c.ReceiverId == receiverId && c.SenderId == senderId) || (c.ReceiverId == senderId && c.SenderId == receiverId));

            var newMessage = new ChatMessage
            {
                Content = new HtmlSanitizer().Sanitize(message),
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                ReceiverId = receiverId,
                Conversation = conversation,
                ConversationId = conversation.Id,
            };

            await this.messageRepository.AddAsync(newMessage);
            await this.messageRepository.SaveChangesAsync();

            return new MessageViewModel
            {
                Content = newMessage.Content,
                Id = newMessage.Id,
                ConversationId = newMessage.ConversationId,
                CreatedOn = newMessage.CreatedOn,
                ReceiverId = newMessage.ReceiverId,
                SenderId = newMessage.ApplicationUserId,
                SenderUserName = user.UserName,
            };
        }

        public async Task<IEnumerable<T>> GetAllConversationMessagesAsync<T>(string conversationId)
        {
            var messages = await this.messageRepository.All()
                .Where(x => x.ConversationId == conversationId)
                .OrderBy(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();

            return messages;
        }

        public async Task<string> GetConversationBySenderAndReceiverIdsAsync(string senderId, string receiverId)
        {
            var conversationId = await this.conversationRepository
                .All()
                .Where(c => (c.ReceiverId == receiverId && c.SenderId == senderId) || (c.ReceiverId == senderId && c.SenderId == receiverId))
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (conversationId == null)
            {
                var sender = await this.usersRepository.All().FirstOrDefaultAsync(s => s.Id == senderId);
                var receiver = await this.usersRepository.All().FirstOrDefaultAsync(r => r.Id == receiverId);

                var newConversation = new Conversation
                {
                    ReceiverId = receiver.Id,
                    SenderId = sender.Id,
                };

                await this.conversationRepository.AddAsync(newConversation);
                await this.conversationRepository.SaveChangesAsync();

                return newConversation.Id;
            }

            return conversationId;
        }

        public async Task<IEnumerable<T>> GetAllUserConversationsAsync<T>(string userId)
        {
            var conversations = await this.conversationRepository
                .All()
                .Where(x => x.SenderId == userId || x.ReceiverId == userId)
                .OrderBy(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();

            return conversations;
        }

        public async Task<IEnumerable<ConversationViewModel>> UpdateusersInfoAsync(IEnumerable<ConversationViewModel> conversations, string userId)
        {
            foreach (var conv in conversations)
            {
                conv.OtherPersonsId = conv.ReceiverId == userId ? conv.SenderId : conv.ReceiverId;
                var otherUser = await this.usersRepository.All().FirstOrDefaultAsync(x => x.Id == conv.OtherPersonsId);
                conv.OtherPersonsPicture = otherUser.ProfilePicture;
                conv.OtherPersonFullName = $"{otherUser.FirstName} {otherUser.LastName}";
            }

            return conversations;
        }
    }
}
