namespace ProSeeker.Services.Data.Offers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Offers;

    public interface IOffersService
    {
        Task<string> CreateAsync(CreateOfferInputModel inputModel, string specialistId);

        Task<IEnumerable<T>> GetAllUserOffers<T>(string userId);
    }
}
