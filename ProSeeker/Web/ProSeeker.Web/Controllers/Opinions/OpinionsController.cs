namespace ProSeeker.Web.Controllers.Opinions
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Opinions;
    using ProSeeker.Web.ViewModels.Opinions;

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
        [Authorize]
        public async Task<IActionResult> CreateAdOpinion(CreateAdOpinionInputModel inputModel)
        {
            var parentId =
                inputModel.ParentId == 0 ?
                    (int?)null :
                    inputModel.ParentId;

            if (parentId.HasValue)
            {
                if (!await this.opinionsService.IsInAdIdAsync(parentId.Value, inputModel.AdId))
                {
                    return this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.opinionsService.CreateAdOpinionAsync(inputModel.AdId, userId, inputModel.Content, parentId);
            return this.RedirectToAction("GetById", "Ads", new { Id = inputModel.AdId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOpinionToSpecialist(CreateSpecialistOpinionInputModel inputModel)
        {
            var parentId =
                inputModel.ParentId == 0 ?
                    (int?)null :
                    inputModel.ParentId;

            if (parentId.HasValue)
            {
                if (!await this.opinionsService.IsInSpecialistIdAsync(parentId.Value, inputModel.SpecialistId))
                {
                    return this.CustomCommonError(this.BadRequest().ToString());
                }
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.opinionsService.CreateSpecOpinionAsync(inputModel.SpecialistId, userId, inputModel.Content, parentId);
            this.TempData["updatedProfile"] = "updated";
            return this.RedirectToAction("GetProfile", "SpecialistsDetails", new { Id = inputModel.SpecialistId });
        }
    }
}
