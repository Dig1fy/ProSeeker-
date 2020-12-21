namespace ProSeeker.Web.Controllers.Quiz
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Ads;
    using ProSeeker.Services.Data.Quizz;
    using ProSeeker.Services.Data.UsersService;
    using ProSeeker.Web.ViewModels.Quizzes;

    public class QuizController : BaseController
    {
        private readonly IQuizzesService quizzesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly IAdsService adsService;

        public QuizController(
            IQuizzesService quizzesService,
            UserManager<ApplicationUser> userManager,
            IUsersService usersService,
            IAdsService adsService)
        {
            this.quizzesService = quizzesService;
            this.userManager = userManager;
            this.usersService = usersService;
            this.adsService = adsService;
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

        public async Task<IActionResult> End(string quizId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            await this.usersService.MakeUserVip(user.Id);

            if (!user.IsSpecialist)
            {
                await this.adsService.MakeAdsVipAsync(user.Id);
            }

            return this.Redirect("/");
        }
    }
}
