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

        public async Task<IEnumerable<T>> GetAllSpecialistsPerCategoryAsync<T>(int categoryId, int page)
        {
            var specialistsToSkip = (page - 1) * GlobalConstants.SpecialistsPerPage;

            var specialists = await this.specialistsRepository
                .AllAsNoTracking()
                .Where(x => x.JobCategoryId == categoryId)
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
    }
}
