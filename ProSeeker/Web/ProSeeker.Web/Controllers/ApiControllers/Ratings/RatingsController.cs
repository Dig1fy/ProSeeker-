namespace ProSeeker.Web.Controllers.ApiControllers.Ratings
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Raitings;
    using ProSeeker.Web.ViewModels.Raitings;

    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingsService ratingsService;
        private readonly UserManager<ApplicationUser> userManager;

        public RatingsController(IRatingsService ratingsService, UserManager<ApplicationUser> userManager)
        {
            this.ratingsService = ratingsService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PostRatingResponseViewModel>> Post(PostRatingInputModel inputModel)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.ratingsService.SetRatingAsync(inputModel.SpecialistDetailsId, userId, inputModel.Value);

            var averageRaiting = await this.ratingsService.GetAverageRatingAsync(inputModel.SpecialistDetailsId);
            var ratingsCount = await this.ratingsService.GetRatingsCountAsync(inputModel.SpecialistDetailsId);

            var model = new PostRatingResponseViewModel
            {
                AverageRatings = averageRaiting,
                RatingsCount = ratingsCount,
            };

            return model;
        }
    }
}
