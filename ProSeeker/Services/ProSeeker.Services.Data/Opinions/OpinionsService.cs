namespace ProSeeker.Services.Data.Opinions
{
    using System.Linq;
    using System.Threading.Tasks;

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

        public async Task CreateAdOpinion(string currentAdId, string userId, string content, int? parentOpinionId = null)
        {
            var ad = this.adsRepository.All().FirstOrDefault(x => x.Id == currentAdId);

            var opinion = new Opinion
            {
                Content = content,
                Ad = ad,
                AdId = ad.Id,
                //Creator = user,
                CreatorId = userId,
                ParentOpinionId = parentOpinionId,
            };

            await this.opinionsRepository.AddAsync(opinion);
            await this.opinionsRepository.SaveChangesAsync();
        }

        public async Task CreateSpecOpinion(string specialistId, string userId, string content, int? parentId = null)
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

        public bool IsInAdId(int opinionId, string currentAdId)
        {
            var opinionAdId = this.opinionsRepository
                .AllAsNoTracking()
                .Where(o => o.SpecialistDetailsId == null)
                .Where(x => x.Id == opinionId)
                .Select(y => y.AdId)
                .FirstOrDefault();

            return opinionAdId == currentAdId;
        }

        public bool IsInSpecialistId(int opinionId, string currentSpecialistId)
        {
            var specOpinionId = this.opinionsRepository
             .AllAsNoTracking()
             .Where(o => o.AdId == null)
             .Where(x => x.Id == opinionId)
             .Select(z => z.SpecialistDetailsId)
             .FirstOrDefault();

            return specOpinionId == currentSpecialistId;
        }
    }
}
