namespace ProSeeker.Web.Controllers.Quiz
{
    using Microsoft.AspNetCore.Mvc;

    public class QuizController : BaseController
    {
        public IActionResult Quiz()
        {
            return this.View();
        }
    }
}
