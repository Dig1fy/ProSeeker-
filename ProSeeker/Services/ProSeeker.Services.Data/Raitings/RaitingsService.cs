namespace ProSeeker.Services.Data.Raitings
{
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;

    public class RaitingsService : IRaitingsService
    {
        private readonly IRepository<Raiting> raitingsRepository;
        private readonly IDeletableEntityRepository<Specialist_Details> specialistsDetailsRepository;

        public RaitingsService(
            IRepository<Raiting> raitingsRepository,
            IDeletableEntityRepository<Specialist_Details> specialistsDetailsRepository)
        {
            this.raitingsRepository = raitingsRepository;
            this.specialistsDetailsRepository = specialistsDetailsRepository;
        }

        public double GetAverageRaiting(string specialistId)
        {
            var averageRaiting = this.raitingsRepository.All()
                .Where(x => x.SpecialistDetailsId == specialistId)
                .Average(a => a.Value);

            return averageRaiting;
        }

        public int GetRaitingsCount(string specialistId)
        {
            var raitingsCount = this.specialistsDetailsRepository.All()
                 .Select(x => x.Raitings)
                 .Count();

            return raitingsCount;
        }

        public async Task SetRaitingAsync(string specialistId, string userId, int raitingValue)
        {
            var raiting = this.raitingsRepository.All().Where(x => x.UserId == userId && x.SpecialistDetailsId == specialistId).FirstOrDefault();

            if (raiting == null)
            {
                raiting = new Raiting
                {
                    UserId = userId,
                    SpecialistDetailsId = specialistId,
                };

                await this.raitingsRepository.AddAsync(raiting);
            }

            raiting.Value = raitingValue;
            await this.raitingsRepository.SaveChangesAsync();
        }
    }
}
