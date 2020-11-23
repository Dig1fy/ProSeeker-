namespace ProSeeker.Web.Controllers.Raitings
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Raitings;
    using ProSeeker.Web.ViewModels.Raitings;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class RaitingsController : ControllerBase
    {
        private readonly IRaitingsService raitingsService;
        private readonly UserManager<ApplicationUser> userManager;

        public RaitingsController(IRaitingsService raitingsService, UserManager<ApplicationUser> userManager)
        {
            this.raitingsService = raitingsService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostRaitingResponseViewModel>> Post(PostRaitingInputModel inputModel)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.raitingsService.SetRaitingAsync(inputModel.SpecialistDetailsId, userId, inputModel.Value);

            var averageRaiting = this.raitingsService.GetAverageRaiting(inputModel.SpecialistDetailsId);
            var raitingsCount = this.raitingsService.GetRaitingsCount(inputModel.SpecialistDetailsId);

            var model = new PostRaitingResponseViewModel
            {
                AverageRaitings = averageRaiting,
                RaitingsCount = raitingsCount,
            };

            return model;
        }
    }
}
