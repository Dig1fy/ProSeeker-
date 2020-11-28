namespace ProSeeker.Services.Data.Offers
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Web.ViewModels.Offers;

    public class OffersService : IOffersService
    {
        private readonly IRepository<Offer> offersRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public OffersService(
            IRepository<Offer> offersRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.offersRepository = offersRepository;
            this.usersRepository = usersRepository;
        }

        public async Task<string> CreateAsync(CreateOfferInputModel inputModel, string specialistId)
        {
            var newOffer = new Offer
            {
                AdId = inputModel.AdId,
                ApplicationUserId = inputModel.ApplicationUserId,
                Description = inputModel.Description,
                Price = inputModel.Price,
                SpecialistDetailsId = specialistId,
                StartDate = inputModel.StartDate,
                ExpirationDate = inputModel.ExpirationDate,
            };

            await this.offersRepository.AddAsync(newOffer);
            await this.offersRepository.SaveChangesAsync();
            return newOffer.Id;
        }
    }
}
