namespace ProSeeker.Services.Data.Specialists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Common;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Categories;

    public class SpecialistsService : ISpecialistsService
    {
        private readonly IDeletableEntityRepository<Specialist_Details> specialistsRepository;
        private readonly IRepository<Rating> ratingsRepository;

        public SpecialistsService(
            IDeletableEntityRepository<Specialist_Details> specialistsRepository,
            IRepository<Rating> ratingsRepository)
        {
            this.specialistsRepository = specialistsRepository;
            this.ratingsRepository = ratingsRepository;
        }

        public async Task<IEnumerable<SpecialistsInCategoryViewModel>> GetSpecialistsFullDetailsPerCategoryAsync(int categoryId, string sortBy, int cityId, int page)
        {
            // After migrating to net5.0, the AutoMapper stopped mapping Specialists_Details (returns null when using generics).
            // It has something to do with the underscore. After 27 different approaches with refactoring, nothing worked. Therefore, we will just return viewModel
            // and do the mapping manually.
            page = page == 0 ? 1 : page;
            sortBy = sortBy == null ? GlobalConstants.ByDateDescending : sortBy;
            var specialistsToSkip = (page - 1) * GlobalConstants.SpecialistsPerPage;
            var sortedSpecialists = this.SortSpecialists(categoryId, sortBy, cityId);

            var specialists = await sortedSpecialists
                .Skip(specialistsToSkip)
                .Take(GlobalConstants.SpecialistsPerPage)
                .Select(x => new SpecialistsInCategoryViewModel
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn,
                    JobCategoryName = x.JobCategory.Name,
                    UserFirstName = x.User.FirstName,
                    UserCityName = x.User.City.Name,
                    UserIsVip = x.User.IsVip,
                    UserLastName = x.User.LastName,
                    UserProfilePicture = x.User.ProfilePicture,
                    UserUserName = x.User.UserName,
                })
                .ToListAsync();

            foreach (var specialist in specialists)
            {
                specialist.RatingsCount = await this.GetSpecialistRatingsCountByGivenSpecialistIdAsync(specialist.Id);
                specialist.AverageRating = await this.GetSpecialistAverageRatingByGivenSpecialistIdAsync(specialist.Id);
            }

            return specialists;
        }

        public async Task<IEnumerable<T>> GetAllSpecialistsPerCategoryAsync<T>(int categoryId, string sortBy, int cityId, int page)
        {
            page = page == 0 ? 1 : page;
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
                .All()
                .Where(x => x.JobCategoryId == categoryId);

            if (cityId != 0)
            {
                specialists = specialists.Where(x => x.User.CityId == cityId);
            }

            return sortBy switch
            {
                GlobalConstants.ByDateDescending => specialists.OrderByDescending(x => x.User.IsVip == true).ThenByDescending(x => x.CreatedOn),
                GlobalConstants.ByOpinionsDescending => specialists.OrderByDescending(x => x.User.IsVip == true).ThenByDescending(x => x.Opinions.Count),
                GlobalConstants.ByRatingDesc => specialists.OrderByDescending(x => x.User.IsVip == true).ThenByDescending(x => x.Ratings.Average(v => v.Value)),
                _ => specialists,
            };
        }

        private async Task<double> GetSpecialistAverageRatingByGivenSpecialistIdAsync(string specialistId)
        {
            var allRatings = await this.ratingsRepository.All().Where(x => x.SpecialistDetailsId == specialistId).ToListAsync();
            if (allRatings.Count() == 0)
            {
                return 0;
            }

            var averageRating = allRatings.Average(a => a.Value);
            return averageRating;
        }

        private async Task<int> GetSpecialistRatingsCountByGivenSpecialistIdAsync(string specialistId)
        {
            var ratingsCount = await this.ratingsRepository.All().Where(x => x.SpecialistDetailsId == specialistId).CountAsync();
            return ratingsCount;
        }
    }
}
