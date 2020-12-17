namespace ProSeeker.Services.Data.PrivateChat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ganss.XSS;
    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.Chat;

    public class PrivateChatService : IPrivateChatService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Group> groupsRepository;
        private readonly IDeletableEntityRepository<ChatMessage> messagesRepository;

        public PrivateChatService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Group> groupsRepository,
            IDeletableEntityRepository<ChatMessage> messagesRepository)
        {
            this.usersRepository = usersRepository;
            this.groupsRepository = groupsRepository;
            this.messagesRepository = messagesRepository;
        }

        public async Task AddUserToGroup(string chatGroupId, string receiverUsername, string senderUsername)
        {
            var receiver = await this.usersRepository.All().FirstOrDefaultAsync(r => r.UserName == receiverUsername);

            var sender = await this.usersRepository.All().FirstOrDefaultAsync(s => s.UserName == senderUsername);

            var group = await this.groupsRepository.All().FirstOrDefaultAsync(g => g.Id == chatGroupId);

            // Only specialist-specialist / user-user / admin-user/specialist chats will be allowed
            if (receiver.IsSpecialist != sender.IsSpecialist)
            {
                return;
            }

            if (group == null)
            {
                group = new Group
                {
                    Name = chatGroupId,
                };

                var receiverToGroup = new UserGroup
                {
                    ApplicationUser = receiver,
                    ApplicationUserId = receiver.Id,
                    Group = group,
                };

                var senderToGroup = new UserGroup
                {
                    ApplicationUser = sender,
                    ApplicationUserId = sender.Id,
                    Group = group,
                };

                group.UsersGroups.Add(receiverToGroup);
                group.UsersGroups.Add(senderToGroup);

                await this.groupsRepository.AddAsync(group);
                await this.groupsRepository.SaveChangesAsync();
            }
        }

        public async Task<ICollection<ChatMessage>> GetAllMessages(string groupId)
        {
            var messages = await this.messagesRepository.All().Where(x => x.GroupId == groupId).OrderByDescending(d => d.CreatedOn).ToListAsync();

            foreach (var message in messages)
            {
                message.Sender = await this.usersRepository.All().FirstOrDefaultAsync(u => u.Id == message.SenderId);
            }

            return messages;
        }

        public Task<string> GetGroupNameById(string groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetSenderId(string senderUsername)
        {
            var senderId = await this.usersRepository.All().Where(x => x.UserName == senderUsername).Select(z => z.Id).FirstOrDefaultAsync();
            return senderId;
        }

        public async Task<string> SendMessageToUser(string groupId, string senderUsername, string receiverUsername, string message)
        {
            var receiver = await this.usersRepository.All().FirstOrDefaultAsync(r => r.UserName == receiverUsername);

            var sender = await this.usersRepository.All().FirstOrDefaultAsync(s => s.UserName == senderUsername);

            var group = await this.groupsRepository.All().FirstOrDefaultAsync(g => g.Id == groupId);

            var newMessage = new ChatMessage
            {
                Sender = sender,
                SenderId = sender.Id,
                CreatedOn = DateTime.UtcNow,
                ReceiverUsername = receiver.UserName,
                ReceiverId = receiver.Id,
                Content = new HtmlSanitizer().Sanitize(message.Trim()),
            };

            await this.messagesRepository.AddAsync(newMessage);
            await this.messagesRepository.SaveChangesAsync();
            return receiver.Id;
        }
    }
}
