namespace ProSeeker.Services.Data.UsersService
{
    public interface IUsersService
    {
        string GetUserProfilePicture(string userId);

        string GetUserFirstNameById(string userId);

        T GetUserById<T>(string id);
    }
}
