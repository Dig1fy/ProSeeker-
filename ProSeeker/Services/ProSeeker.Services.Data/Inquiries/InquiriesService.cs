namespace ProSeeker.Services.Data.Inquiries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Inquiries;

    public class InquiriesService : IInquiriesService
    {
        private readonly IDeletableEntityRepository<Inquiry> inquiriesRepository;
        private readonly IDeletableEntityRepository<Offer> offersRepository;

        public InquiriesService(IDeletableEntityRepository<Inquiry> inquiriesRepository, IDeletableEntityRepository<Offer> offersRepository)
        {
            this.inquiriesRepository = inquiriesRepository;
            this.offersRepository = offersRepository;
        }

        public async Task<T> CheckForExistingOfferAsync<T>(string inquiryId)
        {
            var existingOffer = await this.offersRepository
                .All()
                .Where(x => x.InquiryId == inquiryId)
                .To<T>()
                .FirstOrDefaultAsync();

            return existingOffer;
        }

        public async Task CreateAsync(CreateInquiryInputModel inputModel)
        {
            var inquiry = new Inquiry
            {
                Content = inputModel.Content,
                UserId = inputModel.UserId,
                SpecialistDetailsId = inputModel.SpecialistDetailsId,
                ValidUntil = inputModel.ValidUntil,
                CityId = inputModel.CityId,
            };

            await this.inquiriesRepository.AddAsync(inquiry);
            await this.inquiriesRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string inquiryId)
        {
            var inquiry = await this.inquiriesRepository.GetByIdWithDeletedAsync(inquiryId);
            this.inquiriesRepository.Delete(inquiry);
            await this.inquiriesRepository.SaveChangesAsync();
        }

        public async Task<T> GetDetailsByIdAsync<T>(string inquiryId)
        {
            var inquiry = await this.inquiriesRepository
                .All()
                .Where(x => x.Id == inquiryId)
                .To<T>()
                .FirstOrDefaultAsync();

            return inquiry;
        }

        public async Task<IEnumerable<T>> GetSpecialistEnquiriesAsync<T>(string specialistId)
        {
            var specialisEnquiries = await this.inquiriesRepository
                .All()
                .Where(x => x.SpecialistDetailsId == specialistId)
                .To<T>()
                .ToListAsync();

            return specialisEnquiries;
        }

        public async Task MarkInquiryAsRedAsync(string inquiryId)
        {
            var inquiry = await this.inquiriesRepository
               .All()
               .FirstOrDefaultAsync(x => x.Id == inquiryId);

            inquiry.IsRed = true;
            this.inquiriesRepository.Update(inquiry);
            await this.inquiriesRepository.SaveChangesAsync();
        }
    }
}
