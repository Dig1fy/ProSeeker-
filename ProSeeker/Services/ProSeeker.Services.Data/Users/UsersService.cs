namespace ProSeeker.Services.Data.UsersService
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<string> GetUserFirstNameByIdAsync(string userId)
        {
            return await this.usersRepository
                .All()
                .Where(user => user.Id == userId)
                .Select(user => user.FirstName)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetUserProfilePictureAsync(string userId)
        {
            return await this.usersRepository
                .All()
                .Where(user => user.Id == userId)
                .Select(user => user.ProfilePicture)
                .FirstOrDefaultAsync();
        }

        public async Task<T> GetUserByIdAsync<T>(string id)
        {
            var user =
               await this.usersRepository
                    .All()
                    .Where(x => x.SpecialistDetailsId == id)
                    .To<T>()
                    .FirstOrDefaultAsync();

            return user;
        }

        public async Task<int> GetAllSpecialistsCountAsync()
        {
            var allSpecialists = await this.usersRepository
                .All()
                .Where(x => x.IsSpecialist == true)
                .CountAsync();

            return allSpecialists;
        }

        public async Task<int> GetAllClientsCountAsync()
        {
            var allClients = await this.usersRepository
                .All()
                .Where(x => x.IsSpecialist == false)
                .CountAsync();

            return allClients;
        }
    }
}
