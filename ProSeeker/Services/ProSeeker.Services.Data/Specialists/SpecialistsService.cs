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

        public async Task<IEnumerable<T>> GetAllSpecialistsPerCategoryAsync<T>(int categoryId, string sortBy, int page)
        {
            sortBy = sortBy == null ? GlobalConstants.ByDateDescending : sortBy;
            var specialistsToSkip = (page - 1) * GlobalConstants.SpecialistsPerPage;
            var sortedSpecialists = this.SortSpecialists(categoryId, sortBy);

            var specialists = await sortedSpecialists
                .Skip(specialistsToSkip)
                .Take(GlobalConstants.SpecialistsPerPage)
                .To<T>()
                .ToListAsync();

            return specialists;
        }

        public async Task<int> GetSpecialistsCountByCategoryAsync(int categoryId)
        {
            var specialistsCount = await this.specialistsRepository
                .AllAsNoTracking()
                .Where(x => x.JobCategoryId == categoryId)
                .CountAsync();

            return specialistsCount;
        }

        // TODO: Try with reflection
        private IQueryable<Specialist_Details> SortSpecialists(int categoryId, string sortBy)
        {
            var specialists = this.specialistsRepository
                .AllAsNoTracking()
                .Where(x => x.JobCategoryId == categoryId);

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
