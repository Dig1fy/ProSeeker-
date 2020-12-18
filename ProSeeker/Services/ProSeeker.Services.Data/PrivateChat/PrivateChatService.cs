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

    public class PrivateChatService : IPrivateChatService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<ChatMessage> messageRepository;
        private readonly IDeletableEntityRepository<Conversation> conversationRepository;

        public PrivateChatService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ChatMessage> messageRepository,
            IDeletableEntityRepository<Conversation> conversationRepository)
        {
            this.usersRepository = usersRepository;
            this.messageRepository = messageRepository;
            this.conversationRepository = conversationRepository;
        }

        public async Task<ChatMessage> CreateNewMessage(string message, string senderId, string receiverId)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.Id == senderId);

            var newMessage = new ChatMessage
            {
                Content = new HtmlSanitizer().Sanitize(message),
                ApplicationUser = user,
                ApplicationUserId = user.Id,
                ReceiverId = receiverId,
            };

            await this.messageRepository.AddAsync(newMessage);
            await this.messageRepository.SaveChangesAsync();

            return newMessage;
        }

        public async Task<ICollection<T>> GetAllConversationMessagesAsync<T>(string conversationId)
        {
            var messages = await this.conversationRepository.All().Where(x => x.Id == conversationId)
                .Select(m => m.ChatMessages)
                .To<T>()
                .ToListAsync();

            return messages;
        }

        public async Task<string> GetConversationBySenderAndReceiverIdsAsync(string senderId, string receiverId)
        {
            var conversationId = await this.conversationRepository
                .All()
                .Where(x => x.SenderId == senderId && x.ReceiverId == receiverId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (conversationId == null)
            {
                var sender = await this.usersRepository.All().FirstOrDefaultAsync(s => s.Id == senderId);
                var receiver = await this.usersRepository.All().FirstOrDefaultAsync(r => r.Id == receiverId);

                var newConversation = new Conversation();

                newConversation.UsersConversations.Add(new UserConversation
                {
                    Conversation = newConversation,
                    ConversationId = newConversation.Id,
                    ApplicationUser = sender,
                });

                newConversation.UsersConversations.Add(new UserConversation
                {
                    Conversation = newConversation,
                    ConversationId = newConversation.Id,
                    ApplicationUser = receiver,
                });

                await this.conversationRepository.AddAsync(newConversation);
                await this.conversationRepository.SaveChangesAsync();

                return newConversation.Id;
            }

            return conversationId;
        }
    }
}
