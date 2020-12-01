namespace ProSeeker.Services.Data.Opinions
{
    using System.Threading.Tasks;

    public interface IOpinionsService
    {
        Task CreateAdOpinion(string currentAdId, string userId, string content, int? parentId = null);

        bool IsInAdId(int opinionId, string currentAdId);

        //Task CreateSpecOpinion(string specialistId, string userId, string content, int? parentId = null);

        //bool IsInSpecialistId(string specialistId, int postId);
    }
}
