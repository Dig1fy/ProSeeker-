namespace ProSeeker.Services.Data.Raitings
{
    using System.Threading.Tasks;

    public interface IRaitingsService
    {
        Task SetRaitingAsync(string specialistId, string userId, int raitingValue);

        double GetAverageRaiting(string specialistId);

        int GetRaitingsCount(string specialistId);
    }
}
