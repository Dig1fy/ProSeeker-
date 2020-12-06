namespace ProSeeker.Web.Controllers.Opinions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Opinions;
    using ProSeeker.Web.ViewModels.Opinions;
    using System.Threading.Tasks;

    public class OpinionsController : BaseController
    {
        private readonly IOpinionsService opinionsService;
        private readonly UserManager<ApplicationUser> userManager;

        public OpinionsController(
            IOpinionsService opinionsService,
            UserManager<ApplicationUser> userManager)
        {
            this.opinionsService = opinionsService;
            this.userManager = userManager;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateAdOpinion(CreateAdOpinionInputModel inputModel)
        {
            var parentId =
                inputModel.ParentId == 0 ?
                    (int?)null :
                    inputModel.ParentId;

            if (parentId.HasValue)
            {
                if (!this.opinionsService.IsInAdId(parentId.Value, inputModel.AdId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.opinionsService.CreateAdOpinion(inputModel.AdId, userId, inputModel.Content, parentId);
            return this.RedirectToAction("GetById", "Ads", new { Id = inputModel.AdId });
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateOpinionToSpecialist(CreateSpecialistOpinionInputModel inputModel)
        {
            var parentId =
                inputModel.ParentId == 0 ?
                    (int?)null :
                    inputModel.ParentId;

            if (parentId.HasValue)
            {
                if (!this.opinionsService.IsInSpecialistId(parentId.Value, inputModel.SpecialistId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.opinionsService.CreateSpecOpinion(inputModel.SpecialistId, userId, inputModel.Content, parentId);
            this.TempData["updatedProfile"] = "updated";
            return this.RedirectToAction("GetProfile", "SpecialistsDetails", new { Id = inputModel.SpecialistId });
        }
    }
}