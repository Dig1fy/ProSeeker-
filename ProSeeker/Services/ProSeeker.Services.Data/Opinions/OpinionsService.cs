namespace ProSeeker.Services.Data.Opinions
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;

    public class OpinionsService : IOpinionsService
    {
        private readonly IDeletableEntityRepository<Opinion> opinionsRepository;
        private readonly IDeletableEntityRepository<Ad> adsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Specialist_Details> specialistsRepository;

        public OpinionsService(
            IDeletableEntityRepository<Opinion> opinionsRepository,
            IDeletableEntityRepository<Ad> adsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Specialist_Details> specialistsRepository)
        {
            this.opinionsRepository = opinionsRepository;
            this.adsRepository = adsRepository;
            this.usersRepository = usersRepository;
            this.specialistsRepository = specialistsRepository;
        }

        public async Task CreateAdOpinionAsync(string currentAdId, string userId, string content, int? parentOpinionId = null)
        {
            var ad = this.adsRepository.All().FirstOrDefault(x => x.Id == currentAdId);

            var opinion = new Opinion
            {
                Content = content,
                Ad = ad,
                AdId = ad.Id,
                CreatorId = userId,
                ParentOpinionId = parentOpinionId,
            };

            await this.opinionsRepository.AddAsync(opinion);
            await this.opinionsRepository.SaveChangesAsync();
        }

        public async Task CreateSpecOpinionAsync(string specialistId, string userId, string content, int? parentId = null)
        {
            var specialist = this.specialistsRepository.All().FirstOrDefault(x => x.Id == specialistId);

            var opinion = new Opinion
            {
                Content = content,
                SpecialistDetails = specialist,
                SpecialistDetailsId = specialist.Id,
                CreatorId = userId,
                ParentOpinionId = parentId,
            };

            await this.opinionsRepository.AddAsync(opinion);
            await this.opinionsRepository.SaveChangesAsync();
        }

        public async Task<bool> IsInAdIdAsync(int opinionId, string currentAdId)
        {
            var opinionAdId = await this.opinionsRepository
                .AllAsNoTracking()
                .Where(o => o.SpecialistDetailsId == null)
                .Where(x => x.Id == opinionId)
                .Select(y => y.AdId)
                .FirstOrDefaultAsync();

            return opinionAdId == currentAdId;
        }

        public async Task<bool> IsInSpecialistIdAsync(int opinionId, string currentSpecialistId)
        {
            var specOpinionId = await this.opinionsRepository
             .AllAsNoTracking()
             .Where(o => o.AdId == null)
             .Where(x => x.Id == opinionId)
             .Select(z => z.SpecialistDetailsId)
             .FirstOrDefaultAsync();

            return specOpinionId == currentSpecialistId;
        }
    }
}
