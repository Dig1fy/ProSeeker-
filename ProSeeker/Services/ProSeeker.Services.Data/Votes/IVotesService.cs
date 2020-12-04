namespace ProSeeker.Services.Data.Votes
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        // If isUpVote -> up vote, else - down vote
        Task VoteAsync(string currentAdId, string userId, bool isUpVote);

        int GetUpVotes(string currentAdId);

        int GetDownVotes(string currentAdId);
    }
}
