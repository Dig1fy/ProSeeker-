namespace ProSeeker.Services.Data.Offers
{
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Offers;

    public interface IOffersService
    {
        Task<string> CreateAsync(CreateOfferInputModel inputModel, string specialistId);
    }
}
