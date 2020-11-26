namespace ProSeeker.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Services.Data.Home;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Web.ViewModels;
    using ProSeeker.Web.ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly IHomeService homeService;
        private readonly IUsersService usersService;

        public HomeController(
            IHomeService homeService,
            IUsersService usersService)
        {
            this.homeService = homeService;
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                BaseCategories = this.homeService.GetAllBaseCategories<BaseJobCategoryViewModel>(),
                Counters = new IndexCountersViewModel
                {
                    AllClients = this.usersService.GetAllClientsCount(),
                    AllSpecialists = this.usersService.GetAllSpecialistsCount(),
                },
            };

            return this.View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
