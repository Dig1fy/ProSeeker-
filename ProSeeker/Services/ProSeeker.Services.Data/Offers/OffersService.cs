namespace ProSeeker.Services.Data.Offers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Offers;

    public class OffersService : IOffersService
    {
        private readonly IDeletableEntityRepository<Offer> offersRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Ad> adsRepository;

        public OffersService(
            IDeletableEntityRepository<Offer> offersRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Ad> adsRepository)
        {
            this.offersRepository = offersRepository;
            this.usersRepository = usersRepository;
            this.adsRepository = adsRepository;
        }

        public async Task<T> GetExistingOfferAsync<T>(string currentAdId, string userId, string specialistId)
        {
            var existingOffer = await this.offersRepository
                .AllAsNoTracking()
                .Where(x => x.AdId == currentAdId && x.ApplicationUserId == userId && x.SpecialistDetailsId == specialistId)
                .To<T>()
                .FirstOrDefaultAsync();

            return existingOffer;
        }

        public async Task<string> CreateFromAdAsync(CreateOfferInputModel inputModel)
        {
            var applicationUserId = this.adsRepository
                .All()
                .Where(x => x.Id == inputModel.AdId)
                .Select(u => u.UserId)
                .FirstOrDefault();

            var newOffer = new Offer
            {
                AdId = inputModel.AdId,
                ApplicationUserId = applicationUserId,
                Description = inputModel.Description,
                Price = inputModel.Price,
                SpecialistDetailsId = inputModel.SpecialistDetailsId,
                StartDate = inputModel.StartDate,
                ExpirationDate = inputModel.ExpirationDate,
            };

            await this.offersRepository.AddAsync(newOffer);
            await this.offersRepository.SaveChangesAsync();
            return newOffer.Id;
        }

        public async Task CreateFromInquiryAsync(CreateOfferInputModel inputModel)
        {
            var newOffer = new Offer
            {
                InquiryId = inputModel.InquiryId,
                ApplicationUserId = inputModel.ApplicationUserId,
                Description = inputModel.Description,
                Price = inputModel.Price,
                SpecialistDetailsId = inputModel.SpecialistDetailsId,
                StartDate = inputModel.StartDate,
                ExpirationDate = inputModel.ExpirationDate,
            };

            await this.offersRepository.AddAsync(newOffer);
            await this.offersRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var offer = await this.offersRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            this.offersRepository.Delete(offer);
            await this.offersRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllUserOffersAsync<T>(string userId)
        {
            var allMyOffers = await this.offersRepository
                .All()
                .Where(x => x.ApplicationUserId == userId)
                .To<T>()
                .ToListAsync();

            return allMyOffers;
        }

        public async Task<IEnumerable<T>> GetAllSpecialistOffersAsync<T>(string specialistId)
        {
            var allMyOffers = await this.offersRepository
                .All()
                .Where(x => x.SpecialistDetailsId == specialistId)
                .To<T>()
                .ToListAsync();

            return allMyOffers;
        }

        public async Task<T> GetDetailsByIdAsync<T>(string offerId)
        {
            var offer = await this.offersRepository
                .All()
                .Where(x => x.Id == offerId)
                .To<T>()
                .FirstOrDefaultAsync();

            return offer;
        }

        public int GetUnredOffersCount(string userId)
        {
            var unredOffersCount = this.offersRepository
                .AllAsNoTracking()
                .Where(x => x.ApplicationUserId == userId && x.IsRed == false)
                .Count();

            return unredOffersCount;
        }

        public bool IsThereUnredOffer(string userId)
        {
            var unredOffer = this.offersRepository
                .AllAsNoTracking()
                .Where(x => x.ApplicationUserId == userId && x.IsRed == false)
                .Any();
            return unredOffer;
        }

        public async Task MarkOfferAsRedAsync(string offerId)
        {
            var offer = await this.offersRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == offerId);

            offer.IsRed = true;
            this.offersRepository.Update(offer);
            await this.offersRepository.SaveChangesAsync();
        }

        public async Task AcceptOffer(string offerId)
        {
            var offer = await this.offersRepository.All().Where(x => x.Id == offerId).FirstOrDefaultAsync();
            offer.AcceptedOn = DateTime.UtcNow;
            offer.IsAccepted = true;
            this.offersRepository.Update(offer);
            await this.offersRepository.SaveChangesAsync();
        }
    }
}
