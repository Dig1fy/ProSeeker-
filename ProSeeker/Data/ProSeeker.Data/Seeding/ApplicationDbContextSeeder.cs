namespace ProSeeker.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class ApplicationDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ApplicationDbContextSeeder));

            var seeders = new List<ISeeder>
                          {
                              new RolesSeeder(),
                              new SettingsSeeder(),
                              new CategoriesSeeder(),
                              new CitiesSeeder(),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }

            // FOR YOUR CAR
            // tinsmith - тенекиджия
            // car painter
            // car mechanic
            // car gasman
            // carwash

            // FOR YOUR NEW HOME
            // interior designer
            // designer
            // surveyor
            // real estate agent
            // urbanist

            // OTHERS
            // lawyer
            // taxi driver
            // dentist
            // psychologist
            // photographer
            // accountant
            // hairdresser
        }
    }
}
