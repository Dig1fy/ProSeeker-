using Microsoft.AspNetCore.Mvc;
using ProSeeker.Services.Data.ServicesService;
using System.Threading.Tasks;

namespace ProSeeker.Web.Controllers.Services
{
    public class ServicesController : BaseController
    {
        private readonly IServicesService servicesService;

        public ServicesController(IServicesService servicesService)
        {
            this.servicesService = servicesService;
        }

        public async Task<IActionResult> Delete(int id)
        {
            await this.servicesService.DeleteAsync(id);
            return this.Redirect("/");
        }
    }
}
