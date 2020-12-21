namespace ProSeeker.Web.Controllers.Quiz
{
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Services.Data.Quizz;
    using ProSeeker.Web.ViewModels.Quizzes;
    using System.Threading.Tasks;

    public class QuizController : BaseController
    {
        private readonly IQuizzesService quizzesService;

        public QuizController(IQuizzesService quizzesService)
        {
            this.quizzesService = quizzesService;
        }

        public IActionResult Quiz()
        {
            return this.View();
        }

        public async Task<IActionResult> Start (string quizId)
        {
            var quiz = await this.quizzesService.GetQuizByIdAsync<QuizViewModel>(quizId);
            var quizQuestions = await this.quizzesService.GetQuestionsByQuizzIdAsync<QuestionViewModel>(quizId);
        }
    }
}
