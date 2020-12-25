﻿namespace ProSeeker.Services.Data.Tests.PrivateChat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.PrivateChat;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.PrivateChat;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.PrivateChat;
    using Xunit;

    public sealed class PrivateChatServiceTests : IDisposable
    {
        private readonly IPrivateChatService service;

        private ApplicationDbContext dbContext;

        private List<ApplicationUser> users;
        private List<ChatMessage> messages;
        private List<Conversation> conversations;

        public PrivateChatServiceTests()
        {
            this.users = new List<ApplicationUser>();
            this.messages = new List<ChatMessage>();
            this.conversations = new List<Conversation>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new ApplicationDbContext(options);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(this.dbContext);
            var messagesRepository = new EfDeletableEntityRepository<ChatMessage>(this.dbContext);
            var conversationsRepository = new EfDeletableEntityRepository<Conversation>(this.dbContext);
            this.service = new PrivateChatService(usersRepository, messagesRepository, conversationsRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task MessageShouldBeSentProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(MessageViewModel).Assembly);

            var message = "Hey";
            var receiverId = "2";
            var senderId = "1";
            var conversationId = "1";

            var newMessageModel = await this.service.SendMessageToUserAsync(message, receiverId, senderId, conversationId);
            var allConversationMessages = await this.service.GetAllConversationMessagesAsync<MessageViewModel>(conversationId);
            var sentMessage = allConversationMessages.FirstOrDefault(m => m.Id == newMessageModel.Id);

            Assert.NotNull(sentMessage);
        }

        [Fact]
        public async Task AllConversationMessagesShouldBeReturnedCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(MessageViewModel).Assembly);
            var conversationId = "1";

            var allConversationMessages = await this.service.GetAllConversationMessagesAsync<MessageViewModel>(conversationId);
            var expectedMessagesCount = 2;
            var actualCount = allConversationMessages.Count();

            Assert.Equal(expectedMessagesCount, actualCount);
        }

        [Fact]
        public async Task ShouldReturnConversationIdIfThereIsExistingOne()
        {
            var senderId = "1";
            var receiverId = "2";

            var existingConversationId = await this.service.GetConversationBySenderAndReceiverIdsAsync(senderId, receiverId);
            var expectedConversationId = "1";

            Assert.Equal(expectedConversationId, existingConversationId);
        }

        [Fact]
        public async Task ShouldReturnNewConversationIdIfThereIsNotExistingOneCorrespondingToTheGivenIds()
        {
            var senderId = "3";
            var receiverId = "2";

            var newConversationId = await this.service.GetConversationBySenderAndReceiverIdsAsync(senderId, receiverId);
            var newConversationIdBySenderAndReceiver = await this.service.GetConversationBySenderAndReceiverIdsAsync(senderId, receiverId);

            Assert.Equal(newConversationIdBySenderAndReceiver, newConversationId);
        }

        [Fact]
        public async Task ShouldReturnAllUserConversationCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ConversationViewModel).Assembly);
            var userId = "2";

            var allUserConversation = await this.service.GetAllUserConversationsAsync<ConversationViewModel>(userId);
            var expectedCount = 1;
            var actualCount = allUserConversation.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task ShouldUpdateConversationsInfoProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ConversationViewModel).Assembly);
            var userId = "2";
            var expectedOtherUserId = "1";

            var allUserConversations = await this.service.GetAllUserConversationsAsync<ConversationViewModel>(userId);
            var updatedConversations = await this.service.UpdateusersInfoAsync(allUserConversations, userId);

            var checkFirstConversation = updatedConversations.FirstOrDefault();

            Assert.Equal(checkFirstConversation.OtherPersonsId, expectedOtherUserId);
        }

        [Fact]
        public async Task ShouldMarkAllUserMessagesAsSeenCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(ConversationViewModel).Assembly);
            var userId = "2";
            var conversationId = "1";

            await this.service.MarkAllMessagesOfTheCurrentUserAsSeenAsync(conversationId, userId);
            var userConversations = await this.service.GetAllUserConversationsAsync<ConversationViewModel>(userId);
            var firstConversationToCheck = userConversations.FirstOrDefault();

            Assert.True(firstConversationToCheck.IsSeen == true);
        }

        [Fact]
        public async Task ShouldReturnCorrectNumberOfUnseenMessages()
        {
            var userId = "2";
            var conversationId = "1";

            var expectedUnseenMessagesCount = 1;
            var actualNumberOfUnseenMessages = await this.service.CheckForUnseenMessagesAsync(conversationId, userId);

            Assert.Equal(expectedUnseenMessagesCount, actualNumberOfUnseenMessages);
        }

        public void Dispose()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        private void InitializeRepositoriesData()
        {
            this.users.AddRange(new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    FirstName = "Ivo",
                    LastName = "Ivov",
                    CityId = 1,
                    Email = "u@u",
                    ProfilePicture = "SomeProfilePicture",
                    IsSpecialist = false,
                },
                new ApplicationUser
                {
                    Id = "2",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    CityId = 1,
                    Email = "s@s",
                    ProfilePicture = "SpecProfilePicture",
                    IsSpecialist = true,
                    SpecialistDetailsId = "specialistId",
                },
                new ApplicationUser
                {
                    Id = "3",
                    FirstName = "E",
                    LastName = "E",
                    CityId = 1,
                    Email = "s@s",
                },
            });

            this.messages.AddRange(new List<ChatMessage>
            {
                new ChatMessage
                {
                    Id = "1",
                    ApplicationUserId = "1",
                    ConversationId = "1",
                    IsSeenByReceiver = false,
                    ReceiverId = "2",
                    Content = "Hello, World!",
                },
                new ChatMessage
                {
                    Id = "2",
                    ApplicationUserId = "2",
                    ConversationId = "1",
                    IsSeenByReceiver = true,
                    ReceiverId = "1",
                    Content = "Hello, Galaxy!",
                },
            });

            this.conversations.AddRange(new List<Conversation>
            {
                new Conversation
                {
                    Id = "1",
                    ReceiverId = "2",
                    SenderId = "1",
                    IsSeen = false,
                },
                new Conversation
                {
                    Id = "2",
                    ReceiverId = "1",
                    SenderId = "3",
                    IsSeen = true,
                },
            });

            this.dbContext.AddRange(this.users);
            this.dbContext.AddRange(this.messages);
            this.dbContext.AddRange(this.conversations);
            this.dbContext.SaveChanges();
        }
    }
}