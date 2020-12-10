namespace ProSeeker.Web.Controllers.Errors
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorsController : BaseController
    {
        public IActionResult AccessDenied()
        {
            return this.View();
        }
    }
}
