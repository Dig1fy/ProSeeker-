namespace ProSeeker.Web.Controllers.Votes
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Votes;
    using ProSeeker.Web.ViewModels.Votes;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;
        private readonly UserManager<ApplicationUser> userManager;

        public VotesController(
            IVotesService votesService,
            UserManager<ApplicationUser> userManager)
        {
            this.votesService = votesService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<VotesCountModel>> Post(VoteInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.votesService.VoteAsync(input.AdId, userId, input.IsUpVote);
            var upVotes = await this.votesService.GetUpVotesAsync(input.AdId);
            var downVotes = await this.votesService.GetDownVotesAsync(input.AdId);

            return new VotesCountModel
            {
                UpVotesCount = upVotes,
                DownVotesCount = downVotes,
            };
        }
    }
}
