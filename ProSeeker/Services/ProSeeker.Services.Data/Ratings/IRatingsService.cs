namespace ProSeeker.Services.Data.Raitings
{
    using System.Threading.Tasks;

    public interface IRatingsService
    {
        Task SetRatingAsync(string specialistId, string userId, int ratingValue);

        Task<double> GetAverageRatingAsync(string specialistId);

        Task<int> GetRatingsCountAsync(string specialistId);
    }
}
