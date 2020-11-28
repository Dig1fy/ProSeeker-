namespace ProSeeker.Web.Controllers.Offers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Common;
    using ProSeeker.Web.ViewModels.Offers;

    public class OffersController : BaseController
    {

        //[Authorize(Roles = GlobalConstants.SpecialistRoleName)]
        public IActionResult Create(string currentAdId, string userId)
        {
            var model = new CreateOfferViewModel();
            model.AdId = currentAdId;
            model.ApplicationUserId = userId;
            return this.View(model);
        }

        [HttpPost]
        //[Authorize(Roles = GlobalConstants.SpecialistRoleName)]
        public IActionResult Create(CreateOfferInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }


        }
    }
}
