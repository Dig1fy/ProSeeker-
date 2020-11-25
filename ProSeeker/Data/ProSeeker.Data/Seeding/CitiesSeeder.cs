namespace ProSeeker.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Seeding.DTOs;

    public class CitiesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Cities.Any())
            {
                return;
            }
            
            var importCitiesDto = JsonConvert
                .DeserializeObject<ImportCitiesDto[]>(File.ReadAllText(@"../../Data/ProSeeker.Data/Seeding/Data/Cities.json"));

            var newCities = new List<City>();

            foreach (var city in importCitiesDto)
            {
                newCities.Add(new City
                {
                    Name = city.Name,
                });
            }

            await dbContext.AddRangeAsync(newCities);
        }
    }
}
