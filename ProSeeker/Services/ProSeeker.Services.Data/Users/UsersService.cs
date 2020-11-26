namespace ProSeeker.Services.Data.UsersService
{
    using System.Linq;

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

        public string GetUserFirstNameById(string userId)
        {
            return this.usersRepository
                .All()
                .Where(user => user.Id == userId)
                .Select(user => user.FirstName)
                .FirstOrDefault();
        }

        public string GetUserProfilePicture(string userId)
        {
            return this.usersRepository
                .All()
                .Where(user => user.Id == userId)
                .Select(user => user.ProfilePicture)
                .FirstOrDefault();
        }

        public T GetUserById<T>(string id)
        {
            var user =
                this.usersRepository
                    .All()
                    .Where(x => x.SpecialistDetails.Id == id)
                    .To<T>()
                    .FirstOrDefault();

            return user;
        }

        public int GetAllSpecialistsCount()
        {
            var allSpecialists = this.usersRepository
                .All()
                .Where(x => x.IsSpecialist == true)
                .Count();

            return allSpecialists;
        }

        public int GetAllClientsCount()
        {
            var allClients = this.usersRepository
                .All()
                .Where(x => x.IsSpecialist == false)
                .Count();


            //var all2 = allClients.Where(x => !x.IsSpecialist).ToArray();
            //var all3 = all2.Count();
            //.Select(x => !x.IsSpecialist)
            //.Count();

            return allClients;
        }
    }
}
