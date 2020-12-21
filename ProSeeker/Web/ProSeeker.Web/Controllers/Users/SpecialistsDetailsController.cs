namespace ProSeeker.Web.Controllers.Users
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Web.ViewModels.Users;

    [Authorize]
    public class SpecialistsDetailsController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public SpecialistsDetailsController(
            IUsersService usersService,
            UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetProfile(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var profile = await this.usersService.GetUserByIdAsync<UserViewModel>(id);

            if (profile == null)
            {
                return this.NotFound();
            }

            // id parameter is the specialistId, therefore we need to get the user corresponding to this id.
            var profileUserId = await this.usersService.GetUserIdBySpecialistIdAsync(id);

            if (profileUserId == null)
            {
                return this.NotFound();
            }

            profile.UserId = profileUserId;
            profile.IsProfileOwner = currentUser.Id == profileUserId;

            return this.View(profile);
        }
    }
}
