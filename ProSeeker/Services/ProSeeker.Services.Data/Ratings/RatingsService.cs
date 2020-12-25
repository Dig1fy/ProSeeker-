namespace ProSeeker.Services.Data.Raitings
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;

    public class RatingsService : IRatingsService
    {
        private readonly IRepository<Rating> ratingsRepository;

        public RatingsService(IRepository<Rating> ratingsRepository)
        {
            this.ratingsRepository = ratingsRepository;
        }

        public async Task<double> GetAverageRatingAsync(string specialistId)
        {
            var averageRating = await this.ratingsRepository.AllAsNoTracking()
                .Where(x => x.SpecialistDetailsId == specialistId)
                .AverageAsync(a => a.Value);

            return averageRating;
        }

        public async Task<int> GetRatingsCountAsync(string specialistId)
        {
            var ratingsCount = await this.ratingsRepository
                .AllAsNoTracking()
                .Where(x => x.SpecialistDetailsId == specialistId)
                 .CountAsync();

            return ratingsCount;
        }

        public async Task SetRatingAsync(string specialistId, string userId, int ratingValue)
        {
            var rating = this.ratingsRepository.All().Where(x => x.UserId == userId && x.SpecialistDetailsId == specialistId).FirstOrDefault();

            if (rating == null)
            {
                rating = new Rating
                {
                    UserId = userId,
                    SpecialistDetailsId = specialistId,
                };

                await this.ratingsRepository.AddAsync(rating);
            }

            rating.Value = ratingValue;
            await this.ratingsRepository.SaveChangesAsync();
        }
    }
}
