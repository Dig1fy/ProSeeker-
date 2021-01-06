namespace ProSeeker.Services.Data.Tests.Votes
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Votes;
    using Xunit;

    public sealed class VotesServiceTests : BaseServiceTests
    {
        private readonly IVotesService service;

        private List<Vote> votes;

        public VotesServiceTests()
        {
            this.votes = new List<Vote>();

            var surveysRepository = new EfDeletableEntityRepository<Survey>(this.DbContext);

            var votesRepository = new EfRepository<Vote>(this.DbContext);
            this.service = new VotesService(votesRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task GetUpVotesAsync_ShouldReturnCorrectNumberOfUpvotes()
        {
            var desiredAdId = "1";
            var expectedCount = 1;

            var upVotesCount = await this.service.GetUpVotesAsync(desiredAdId);

            Assert.Equal(expectedCount, upVotesCount);
        }

        [Fact]
        public async Task GetDownVotesAsync_ShouldReturnCorrectNumberOfDownVotes()
        {
            var desiredAdId = "1";
            var expectedCount = 1;

            var downVotesCount = await this.service.GetDownVotesAsync(desiredAdId);

            Assert.Equal(expectedCount, downVotesCount);
        }

        [Fact]
        public async Task GetUpVotesAsync_ShouldAccountNewVoteCorrectly()
        {
            var desiredAdId = "1";
            var userId = "3";
            var isUpVote = true;
            var expectedVotesCountAfterNewUpVote = 2;

            await this.service.VoteAsync(desiredAdId, userId, isUpVote);
            var upVotesCount = await this.service.GetUpVotesAsync(desiredAdId);

            Assert.Equal(expectedVotesCountAfterNewUpVote, upVotesCount);
        }

        [Fact]
        public async Task GetUpVotesAsync_IfUserVotesManyTimesOnlyTheLastVoteShouldBeAccounted()
        {
            var desiredAdId = "1";
            var userId = "3";
            var isUpVote = true;
            var expectedVotesCountAfterNewUpVote = 2;
            var downVotesCountShouldBeTheSame = 1;

            await this.service.VoteAsync(desiredAdId, userId, isUpVote);
            await this.service.VoteAsync(desiredAdId, userId, false);
            await this.service.VoteAsync(desiredAdId, userId, isUpVote);
            await this.service.VoteAsync(desiredAdId, userId, isUpVote);

            var upVotesCount = await this.service.GetUpVotesAsync(desiredAdId);
            var downVotesCount = await this.service.GetDownVotesAsync(desiredAdId);

            Assert.Equal(expectedVotesCountAfterNewUpVote, upVotesCount);
            Assert.Equal(downVotesCountShouldBeTheSame, downVotesCount);
        }

        private void InitializeRepositoriesData()
        {
            this.votes.AddRange(new List<Vote>
            {
                new Vote
                {
                    Id = 1,
                    AdId = "1",
                    UserId = "1",
                    VoteType = VoteType.UpVote,
                },
                new Vote
                {
                    Id = 2,
                    AdId = "1",
                    UserId = "2",
                    VoteType = VoteType.DownVote,
                },
            });

            this.DbContext.AddRange(this.votes);
            this.DbContext.SaveChanges();
        }
    }
}
