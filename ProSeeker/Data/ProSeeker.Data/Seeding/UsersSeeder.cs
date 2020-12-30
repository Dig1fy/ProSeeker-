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

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                     UserName = "s@s",
                     Email = "s@s",
                     CityId = 5,
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
                     UserName = "s@s2",
                     Email = "s@s2",
                     CityId = 5,
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
                     UserName = "u@u",
                     Email = "u@u",
                     CityId = 5,
                     EmailConfirmed = true,
                     FirstName = "Стоян",
                     LastName = "Иванов",
                     ProfilePicture = GlobalConstants.DefaultProfileImagePath,
                     IsSpecialist = false,
                },
            };

            for (int i = 0; i < 20; i++)
            {
                users.Add(new ApplicationUser
                {
                    UserName = $"testUser@{i}s",
                    Email = $"testUser@{i}s",
                    CityId = 5,
                    EmailConfirmed = true,
                    FirstName = $"Георги{i}с",
                    LastName = $"Георгиев{i}в",
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
                });
            }

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "123123");

                if (user.IsSpecialist)
                {
                    user.SpecialistDetailsId = user.SpecialistDetails.Id;
                    await userManager.AddToRoleAsync(user, GlobalConstants.SpecialistRoleName);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.RegularUserRoleName);
                }
            }

            var adminUser = new ApplicationUser
            {
                UserName = "admin@admin",
                Email = "admin@admin",
                CityId = 5,
                EmailConfirmed = true,
                FirstName = "Админ",
                LastName = "Админ",
                ProfilePicture = GlobalConstants.DefaultProfileImagePath,
                IsSpecialist = false,
            };

            await userManager.CreateAsync(adminUser, "123123");
            await userManager.AddToRoleAsync(adminUser, GlobalConstants.AdministratorRoleName);
        }
    }
}
