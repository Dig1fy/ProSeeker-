namespace ProSeeker.Services.Data.Votes
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        // If isUpVote -> up vote, else - down vote
        Task VoteAsync(string currentAdId, string userId, bool isUpVote);

        Task<int> GetUpVotesAsync(string currentAdId);

        Task<int> GetDownVotesAsync(string currentAdId);
    }
}
