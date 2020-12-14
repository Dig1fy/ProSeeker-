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

        public InquiriesService(IDeletableEntityRepository<Inquiry> inquiriesRepository)
        {
            this.inquiriesRepository = inquiriesRepository;
        }

        public async Task CreateAsync(InquiryInputModel inputModel)
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
            await this.inquiriesRepository.GetByIdWithDeletedAsync(inquiryId);
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
