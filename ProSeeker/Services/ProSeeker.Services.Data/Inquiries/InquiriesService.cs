namespace ProSeeker.Services.Data.Inquiries
{
    using System.Threading.Tasks;

    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
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
    }
}
