using Microsoft.AspNetCore.Mvc;

namespace ProSeeker.Web.Controllers.Quiz
{
    public class QuizController : BaseController
    {
        public IActionResult Quiz()
        {
            return this.View();
        }
    }
}
