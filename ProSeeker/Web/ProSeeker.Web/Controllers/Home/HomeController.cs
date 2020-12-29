namespace ProSeeker.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Ads;
    using ProSeeker.Services.Data.BaseJobCategories;
    using ProSeeker.Services.Data.Home;
    using ProSeeker.Services.Data.Offers;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;
        private readonly IUsersService usersService;
        private readonly IOffersService offersSerivice;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdsService adsService;
        private readonly IBaseJobCategoriesService baseJobCategoriesService;

        public HomeController(
            IHomeService homeService,
            IUsersService usersService,
            IOffersService offersSerivice,
            UserManager<ApplicationUser> userManager,
            IAdsService adsService,
            IBaseJobCategoriesService baseJobCategoriesService)
        {
            this.homeService = homeService;
            this.usersService = usersService;
            this.offersSerivice = offersSerivice;
            this.userManager = userManager;
            this.adsService = adsService;
            this.baseJobCategoriesService = baseJobCategoriesService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel()
            {
                Counters = new IndexCountersViewModel
                {
                    AllBaseJobCategories = await this.baseJobCategoriesService.GetAllBaseJobCategoriesCountAsync(),
                    AllAds = await this.adsService.GetAllAdsCountAsync(),
                    AllClients = await this.usersService.GetAllClientsCountAsync(),
                    AllSpecialists = await this.usersService.GetAllSpecialistsCountAsync(),
                },
            };

            return this.View(viewModel);
        }
    }
}
