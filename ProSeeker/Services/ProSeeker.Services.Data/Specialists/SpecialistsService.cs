namespace ProSeeker.Services.Data.Specialists
{
    using ProSeeker.Common;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using System.Collections.Generic;
    using System.Linq;

    public class SpecialistsService : ISpecialistsService
    {
        private readonly IDeletableEntityRepository<Specialist_Details> specialistsRepository;

        public SpecialistsService(IDeletableEntityRepository<Specialist_Details> specialistsRepository)
        {
            this.specialistsRepository = specialistsRepository;
        }

        public IEnumerable<T> GetAllSpecialistsPerCategory<T>(int categoryId, int page)
        {
            var specialistsToSkip = (page - 1) * GlobalConstants.SpecialistsPerPage;

            var specialists = this.specialistsRepository
                .AllAsNoTracking()
                .Where(x => x.JobCategoryId == categoryId)
                .Skip(specialistsToSkip)
                .Take(GlobalConstants.SpecialistsPerPage)
                .To<T>()
                .ToList();

            return specialists;
        }

        public int GetSpecialistsCountByCategory(int categoryId)
        {
            var specialistsCount = this.specialistsRepository
                .AllAsNoTracking()
                .Where(x => x.JobCategoryId == categoryId)
                .Count();

            return specialistsCount;
        }
    }
}
