namespace ProSeeker.Services.Data.UsersService
{
    using System.Linq;

    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public string GetUserProfilePicture(string userId)
        {
            var user = this.usersRepository.All().Where(x => x.Id == userId).FirstOrDefault();

            return user.ProfilePicture;
        }
    }
}
