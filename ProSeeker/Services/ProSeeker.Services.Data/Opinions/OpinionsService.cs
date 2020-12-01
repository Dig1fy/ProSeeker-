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

        public OpinionsService(
            IDeletableEntityRepository<Opinion> opinionsRepository,
            IDeletableEntityRepository<Ad> adsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.opinionsRepository = opinionsRepository;
            this.adsRepository = adsRepository;
            this.usersRepository = usersRepository;
        }

        public async Task CreateAdOpinion(string currentAdId, string userId, string content, int? parentId = null)
        {
            var ad = this.adsRepository.All().FirstOrDefault(x => x.Id == currentAdId);
            var user = this.usersRepository.All().FirstOrDefault(x => x.Id == userId);

            var opinion = new Opinion
            {
                Ad = ad,
                AdId = ad.Id,
                Creator = user,
                CreatorId = user.Id,
                Content = content,
            };

            await this.opinionsRepository.AddAsync(opinion);
            await this.opinionsRepository.SaveChangesAsync();
        }

        public bool IsInAdId(int opinionId, string currentAdId)
        {
            var opinionAdId = this.opinionsRepository
                .All()
                .Where(x => x.Id == opinionId)
                .Select(y => y.AdId)
                .FirstOrDefault();

            return opinionAdId == currentAdId;
        }
    }
}
