namespace ProSeeker.Web.Controllers.Errors
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Web.ViewModels;

    public class ErrorController : BaseController
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string code)
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier, StatusCode = code });
        }
    }
}
