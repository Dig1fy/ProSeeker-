namespace ProSeeker.Web.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Services.Data.Home;
    using ProSeeker.Web.ViewModels.Categories;

    public class NavbarSpecialistsViewComponent : ViewComponent
    {
        private readonly IHomeService homeService;

        public NavbarSpecialistsViewComponent(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await this.homeService.GetAllBaseCategoriesAsync<BaseJobCategoryViewModel>();
            return this.View(model);
        }
    }
}
