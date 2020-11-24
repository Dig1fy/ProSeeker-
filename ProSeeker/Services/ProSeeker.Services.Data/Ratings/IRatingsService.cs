namespace ProSeeker.Services.Data.Raitings
{
    using System.Threading.Tasks;

    public interface IRatingsService
    {
        Task SetRatingAsync(string specialistId, string userId, int ratingValue);

        double GetAverageRating(string specialistId);

        int GetRatingsCount(string specialistId);
    }
}
