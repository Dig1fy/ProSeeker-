namespace ProSeeker.Services.Data.Opinions
{
    using System.Threading.Tasks;

    public interface IOpinionsService
    {
        Task CreateAdOpinionAsync(string currentAdId, string userId, string content, int? parentId = null);

        Task<bool> IsInAdIdAsync(int opinionId, string currentAdId);

        Task CreateSpecOpinionAsync(string specialistId, string userId, string content, int? parentId = null);

        Task<bool> IsInSpecialistIdAsync(int opinionId, string specialistId);
    }
}
