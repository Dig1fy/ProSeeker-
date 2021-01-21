namespace ProSeeker.Services.Data.UsersService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Cities;
    using ProSeeker.Web.ViewModels.Opinions;
    using ProSeeker.Web.ViewModels.Services;
    using ProSeeker.Web.ViewModels.Users;
    using ProSeeker.Web.ViewModels.Users.Specialists;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<City> citiesRepository;
        private readonly IDeletableEntityRepository<Specialist_Details> specialistRepository;
        private readonly IDeletableEntityRepository<JobCategory> jobCategoriesRepository;
        private readonly IDeletableEntityRepository<Service> servicesRepository;
        private readonly IDeletableEntityRepository<Opinion> opinionsRepository;
        private readonly IRepository<Rating> ratingsRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<City> citiesRepository,
            IDeletableEntityRepository<Specialist_Details> specialistRepository,
            IDeletableEntityRepository<JobCategory> jobCategoriesRepository,
            IDeletableEntityRepository<Service> servicesRepository,
            IDeletableEntityRepository<Opinion> opinionsRepository,
            IRepository<Rating> ratingsRepository)
        {
            this.usersRepository = usersRepository;
            this.citiesRepository = citiesRepository;
            this.specialistRepository = specialistRepository;
            this.jobCategoriesRepository = jobCategoriesRepository;
            this.servicesRepository = servicesRepository;
            this.opinionsRepository = opinionsRepository;
            this.ratingsRepository = ratingsRepository;
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

        // This won't get the regular user! It's retrieving only the specialist by specialistID
        public async Task<T> GetUserByIdAsync<T>(string id)
        {
            var user = await this.usersRepository
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

        public async Task<string> GetUserIdBySpecialistIdAsync(string specialistId)
        {
            var userId = await this.usersRepository
                .All()
                .Where(x => x.SpecialistDetailsId == specialistId)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            return userId;
        }

        public async Task MakeUserVip(string userId)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(u => u.Id == userId);
            user.IsVip = true;
            user.VipExpirationDate = DateTime.UtcNow.AddDays(7);
            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();
        }

        public async Task<UserViewModel> GetUserProfileAsync(string currentUserId, string specialistId)
        {
            // After migrating to net5.0, this entity just REFUSES to be mapped by AutoMapper... so we do it manually.
            // We retriever all the entities (with their data) which are needed for the UserViewModel
            var currentUser = await this.usersRepository.All().FirstOrDefaultAsync(x => x.Id == currentUserId);
            var profileOnwer = await this.usersRepository.All().Where(u => u.SpecialistDetailsId == specialistId).FirstOrDefaultAsync();
            var city = await this.citiesRepository.All().FirstOrDefaultAsync(x => x.Id == profileOnwer.CityId);
            var specialistDetails = await this.specialistRepository.All().FirstOrDefaultAsync(x => x.Id == specialistId);
            var jobCategory = await this.jobCategoriesRepository.All().FirstOrDefaultAsync(x => x.Id == specialistDetails.JobCategoryId);
            var specialistServices = await this.servicesRepository.All().Where(x => x.SpecialistDetailsId == specialistId).ToListAsync();
            var specialistOpinions = await this.opinionsRepository.All().Where(x => x.SpecialistDetailsId == specialistId).ToListAsync();
            var specialistRatings = await this.ratingsRepository.All().Where(x => x.SpecialistDetailsId == specialistId).ToListAsync();

            var allServicesViewModel = new List<ServiceViewModel>();
            var allOpinionsViewModel = new List<OpinionViewModel>();

            foreach (var opinion in specialistOpinions)
            {
                allOpinionsViewModel.Add(new OpinionViewModel
                {
                    AdId = opinion.AdId,
                    Content = opinion.Content,
                    CreatedOn = opinion.CreatedOn,
                    Creator = new UserOpinionViewModel
                    {
                        Id = currentUser.Id,
                        FirstName = currentUser.FirstName,
                        LastName = currentUser.LastName,
                        ProfilePicture = currentUser.ProfilePicture,
                    },
                    Id = opinion.Id,
                    ParentOpinionId = opinion.ParentOpinionId,
                    SpecialistDetailsId = opinion.SpecialistDetailsId,
                });
            }

            foreach (var service in specialistServices)
            {
                allServicesViewModel.Add(new ServiceViewModel
                {
                    Id = service.Id,
                    Description = service.Description,
                    Name = service.Name,
                });
            }

            // This is what we pass to the View
            var model = new UserViewModel
            {
                UserId = profileOnwer.Id,
                IsProfileOwner = profileOnwer.Id == currentUserId,
                FirstName = profileOnwer.FirstName,
                LastName = profileOnwer.LastName,
                ProfilePicture = profileOnwer.ProfilePicture,
                CreatedOn = profileOnwer.CreatedOn,
            };

            model.City = new CitySimpleViewModel
            {
                Id = city.Id,
                Name = city.Name,
            };

            var averageRating = specialistRatings.Average(v => v.Value);

            model.SpecialistDetails = new SpecialistDetailsViewModel
            {
                Id = specialistDetails.Id,
                AboutMe = specialistDetails.AboutMe,
                CompanyName = specialistDetails.CompanyName,
                Experience = specialistDetails.Experience,
                RatingsCount = specialistRatings.Count(),
                Website = specialistDetails.Website,
                Qualification = specialistDetails.Qualification,
                Opinions = allOpinionsViewModel,
                Services = allServicesViewModel,
                AverageRating = averageRating,
            };

            model.SpecialistDetails.JobCategory = new CategorySimpleViewModel
            {
                Id = jobCategory.Id,
                Name = jobCategory.Name,
            };

            return model;
        }

        public async Task UpdateUserPhoneNumberAsync(string userId, string phoneNumber)
        {
            var user = await this.usersRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == userId);

            user.PhoneNumber = phoneNumber;
            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();
        }
    }
}
