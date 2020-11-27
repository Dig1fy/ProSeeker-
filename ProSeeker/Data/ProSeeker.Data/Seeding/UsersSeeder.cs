namespace ProSeeker.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using ProSeeker.Common;
    using ProSeeker.Data.Models;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var specialists = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                     UserName = "test2@test.com",
                     CityId = 120,
                     EmailConfirmed = true,
                     FirstName = "Георги",
                     LastName = "Георгиев",
                     ProfilePicture = GlobalConstants.DefaultProfileImagePath,
                     IsSpecialist = true,
                     SpecialistDetails = new Specialist_Details
                     {
                         JobCategoryId = 2,
                         AboutMe = "Добър майстор!",
                         CompanyName = "I/MeAndMyself",
                         Experience = "1. Правил съм това. " +
                         "2, Правил съм и това.",
                         Services = new List<Service>
                         {
                             new Service
                             {
                                 Name = "Правя това. ",
                                 Description = "Така, после така, накрая така",
                             },
                         },
                         Qualification = "Имам специлизация в сферата на...",
                     },
                },
                new ApplicationUser
                {
                     UserName = "specialist@specialist",
                     PasswordHash = "123123",
                     CityId = 120,
                     EmailConfirmed = true,
                     FirstName = "Иван",
                     LastName = "Иванов",
                     ProfilePicture = GlobalConstants.DefaultProfileImagePath,
                     IsSpecialist = true,
                     SpecialistDetails = new Specialist_Details
                     {
                         JobCategoryId = 2,
                         AboutMe = "Добър майстор!",
                         CompanyName = "SpecialistLTD",
                         Experience = "1. Правил съм това. " +
                         "2, Правил съм и това.",
                         Services = new List<Service>
                         {
                             new Service
                             {
                                 Name = "Това. ",
                                 Description = "Така, после така, накрая така",
                             },
                         },
                         Qualification = "Имам специлизация в сферата на...",
                     },
                },
                new ApplicationUser
                {
                     UserName = "user@user",
                     PasswordHash = "123123",
                     CityId = 120,
                     EmailConfirmed = true,
                     FirstName = "Стоян",
                     LastName = "Иванов",
                     ProfilePicture = GlobalConstants.DefaultProfileImagePath,
                     IsSpecialist = false,
                },
            };
            foreach (var specialist in specialists)
            {
                await userManager.CreateAsync(specialist, "123123");

                if (specialist.IsSpecialist)
                {
                    await userManager.AddToRoleAsync(specialist, GlobalConstants.SpecialistRoleName);
                }
                else
                {
                    await userManager.AddToRoleAsync(specialist, GlobalConstants.RegularUserRoleName);
                }
            }
        }
    }
}