namespace ProSeeker.Web.Controllers.Quiz
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Services.Data.Quizz;
    using ProSeeker.Web.ViewModels.Quizzes;

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

        public async Task<IActionResult> Start(string quizId)
        {
            var quiz = await this.quizzesService.GetQuizByIdAsync<QuizViewModel>(quizId);
            var quizQuestions = await this.quizzesService.GetQuestionsByQuizzIdAsync<QuestionViewModel>(quizId);
            quiz.Questions = quizQuestions;

            foreach (var question in quiz.Questions)
            {
                question.Answers = await this.quizzesService.GetAnswersByQuestionIdAsync<AnswerViewModel>(question.Id);
            }

            var quizModel = new QuizViewModel
            {
                Id = quizId,
                Title = quiz.Title,
                Questions = quiz.Questions,
            };

            return this.View(quizModel);
        }
    }
}
