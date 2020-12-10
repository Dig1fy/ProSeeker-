namespace ProSeeker.Services.Data.Specialists
{
    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Common;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SpecialistsService : ISpecialistsService
    {
        private readonly IDeletableEntityRepository<Specialist_Details> specialistsRepository;

        public SpecialistsService(IDeletableEntityRepository<Specialist_Details> specialistsRepository)
        {
            this.specialistsRepository = specialistsRepository;
        }

        public async Task<IEnumerable<T>> GetAllSpecialistsPerCategoryAsync<T>(int categoryId, string sortBy, int cityId, int page)
        {
            sortBy = sortBy == null ? GlobalConstants.ByDateDescending : sortBy;
            var specialistsToSkip = (page - 1) * GlobalConstants.SpecialistsPerPage;
            var sortedSpecialists = this.SortSpecialists(categoryId, sortBy, cityId);

            var specialists = await sortedSpecialists
                .Skip(specialistsToSkip)
                .Take(GlobalConstants.SpecialistsPerPage)
                .To<T>()
                .ToListAsync();

            return specialists;
        }

        public async Task<int> GetSpecialistsCountByCategoryAsync(int categoryId, int cityId)
        {
            if (cityId == 0)
            {
                return await this.specialistsRepository
                .AllAsNoTracking()
                .Where(x => x.JobCategoryId == categoryId)
                .CountAsync();
            }
            else
            {
                return await this.specialistsRepository
                .AllAsNoTracking()
                .Where(x => x.JobCategoryId == categoryId && x.User.CityId == cityId)
                .CountAsync();
            }

            
        }

        // TODO: Try with reflection
        private IQueryable<Specialist_Details> SortSpecialists(int categoryId, string sortBy, int cityId)
        {
            var specialists = this.specialistsRepository
                .AllAsNoTracking()
                .Where(x => x.JobCategoryId == categoryId);

            if (cityId != 0)
            {
                specialists = specialists.Where(x => x.User.CityId == cityId);
            }

            return sortBy switch
            {
                GlobalConstants.ByDateDescending => specialists.OrderByDescending(x => x.CreatedOn),
                GlobalConstants.ByOpinionsDescending => specialists.OrderByDescending(x => x.Opinions.Count),
                GlobalConstants.ByRatingDesc => specialists.OrderByDescending(x => x.Ratings.Average(v => v.Value)),
                _ => specialists,
            };
        }
    }
}
