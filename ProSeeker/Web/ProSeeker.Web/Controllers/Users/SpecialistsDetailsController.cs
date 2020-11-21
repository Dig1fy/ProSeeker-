namespace ProSeeker.Web.Controllers.Users
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Web.ViewModels.Users;

    public class SpecialistsDetailsController : BaseController
    {
        private readonly IUsersService usersService;

        public SpecialistsDetailsController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult GetProfile(string id)
        {
            var profile = this.usersService.GetUserById<UserViewModel>(id);

            if (profile == null)
            {
                return this.NotFound();
            }

            return this.View(profile);
        }
    }
}
