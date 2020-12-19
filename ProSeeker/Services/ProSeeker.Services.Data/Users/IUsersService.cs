namespace ProSeeker.Services.Data.UsersService
{
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<string> GetUserProfilePictureAsync(string userId);

        Task<string> GetUserFirstNameByIdAsync(string userId);

        Task<T> GetUserByIdAsync<T>(string id);

        Task<int> GetAllSpecialistsCountAsync();

        Task<int> GetAllClientsCountAsync();

        Task<string> GetUserIdBySpecialistIdAsync(string specialistId);
    }
}
