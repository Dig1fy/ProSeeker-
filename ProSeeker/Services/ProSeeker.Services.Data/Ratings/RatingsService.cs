namespace ProSeeker.Services.Data.Raitings
{
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;

    public class RatingsService : IRatingsService
    {
        private readonly IRepository<Rating> ratingsRepository;
        private readonly IDeletableEntityRepository<Specialist_Details> specialistsDetailsRepository;

        public RatingsService(
            IRepository<Rating> ratingsRepository,
            IDeletableEntityRepository<Specialist_Details> specialistsDetailsRepository)
        {
            this.ratingsRepository = ratingsRepository;
            this.specialistsDetailsRepository = specialistsDetailsRepository;
        }

        public double GetAverageRating(string specialistId)
        {
            var averageRating = this.ratingsRepository.AllAsNoTracking()
                .Where(x => x.SpecialistDetailsId == specialistId)
                .Average(a => a.Value);

            return averageRating;
        }

        public int GetRatingsCount(string specialistId)
        {
            var ratingsCount = this.ratingsRepository
                .AllAsNoTracking()
                .Where(x => x.SpecialistDetailsId == specialistId)
                 .ToList()
                 .Count();

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
