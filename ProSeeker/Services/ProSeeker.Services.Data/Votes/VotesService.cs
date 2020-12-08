namespace ProSeeker.Services.Data.Votes
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;

    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> votesRepository;

        public VotesService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public async Task<int> GetUpVotesAsync(string currentAdId)
        {
            var votes = await this.votesRepository.All()
               .Where(x => x.AdId == currentAdId)
               .Where(x => x.VoteType == VoteType.UpVote)
               .Select(x => x.VoteType)
               .CountAsync();

            return votes;
        }

        public async Task<int> GetDownVotesAsync(string currentAdId)
        {
            var votes = await this.votesRepository.All()
               .Where(x => x.AdId.Equals(currentAdId))
               .Where(x => x.VoteType == VoteType.DownVote)
               .Select(x => x.VoteType)
               .CountAsync();

            return votes;
        }

        public async Task VoteAsync(string currentAdId, string userId, bool isUpVote)
        {
            var vote = this.votesRepository.All()
                .FirstOrDefault(x => x.AdId.Equals(currentAdId) && x.UserId == userId);

            // If the user has already voted and wants to change his vote type
            if (vote != null)
            {
                vote.VoteType = isUpVote ? VoteType.UpVote : VoteType.DownVote;
            }
            else
            {
                vote = new Vote
                {
                    AdId = currentAdId,
                    UserId = userId,
                    VoteType = isUpVote ? VoteType.UpVote : VoteType.DownVote,
                };

                await this.votesRepository.AddAsync(vote);
            }

            await this.votesRepository.SaveChangesAsync();
        }
    }
}
