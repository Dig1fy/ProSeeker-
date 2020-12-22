namespace ProSeeker.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Home;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Web.ViewModels;
    using ProSeeker.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;
        private readonly IUsersService usersService;
        private readonly IOffersService offersSerivice;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IHomeService homeService,
            IUsersService usersService,
            IOffersService offersSerivice,
            UserManager<ApplicationUser> userManager)
        {
            this.homeService = homeService;
            this.usersService = usersService;
            this.offersSerivice = offersSerivice;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel()
            {
                BaseCategories = await this.homeService.GetAllBaseCategoriesAsync<BaseJobCategoryViewModel>(),
                Counters = new IndexCountersViewModel
                {
                    AllClients = await this.usersService.GetAllClientsCountAsync(),
                    AllSpecialists = await this.usersService.GetAllSpecialistsCountAsync(),
                },
            };

            return this.View(viewModel);
        }
    }
}
