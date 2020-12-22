namespace ProSeeker.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Common;

    public class BaseController : Controller
    {
        public IActionResult CustomNotFound()
        {
            return this.RedirectToAction(
                    GlobalConstants.ErrorHandlerAction,
                    GlobalConstants.ErrorHandlerAction,
                    new { code = GlobalConstants.ErrorNotFound });
        }

        public IActionResult CustomAccessDenied()
        {
            return this.RedirectToAction(
                    GlobalConstants.ErrorHandlerAction,
                    GlobalConstants.ErrorHandlerAction,
                    new { code = GlobalConstants.ErrorAccessDenied });
        }

        public IActionResult CustomCommonError(string status = null)
        {
            return this.RedirectToAction(
                    GlobalConstants.ErrorHandlerAction,
                    GlobalConstants.ErrorHandlerAction,
                    new { code = status });
        }
    }
}
