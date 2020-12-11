﻿namespace ProSeeker.Web.Controllers.Offers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Web.ViewModels.Offers;

    public class OffersController : BaseController
    {
        private readonly IOffersService offersService;
        private readonly UserManager<ApplicationUser> userManager;

        public OffersController(IOffersService offersService, UserManager<ApplicationUser> userManager)
        {
            this.offersService = offersService;
            this.userManager = userManager;
        }

        //[Authorize(Roles = GlobalConstants.SpecialistRoleName)]
        public IActionResult Create(CreateOfferViewModel viewModel)
        {
            var inputModel = new CreateOfferInputModel
            {
                AdId = viewModel.Id,
            };

            return this.View(inputModel);
        }

        [HttpPost]
        //[Authorize(Roles = GlobalConstants.SpecialistRoleName)]
        public async Task<IActionResult> Create(CreateOfferInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var specialistId = user.SpecialistDetailsId;
            var newOfferId = await this.offersService.CreateAsync(inputModel, specialistId);

            return this.Redirect("/");
        }

        public async Task<IActionResult> UserOffers()
        {
            var userId = this.userManager.GetUserId(this.User);
            var allMyOffers = await this.offersService.GetAllUserOffers<UserOffersViewModel>(userId);
            var model = new AllMyOffersViewModel { Offers = allMyOffers };
            return this.View(model);
        }

        public async Task<IActionResult> Details(string offerId)
        {
            var offer = await this.offersService.GetDetailsByIdAsync<OfferDetailsViewModel>(offerId);

            if (offer is null)
            {
                return this.NotFound();
            }

            return this.View(offer);
        }
    }
}
