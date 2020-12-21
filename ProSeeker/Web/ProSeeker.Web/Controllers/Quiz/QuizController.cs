namespace ProSeeker.Web.Controllers.Quiz
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.Quiz;
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
            // Check if the user has already completed this quiz
            var user = await this.userManager.GetUserAsync(this.User);
            var hasItBeenCompletedAlready = await this.quizzesService.HasItBeenCompletedByThisUser(user.Id);

            if (hasItBeenCompletedAlready)
            {
                return this.Redirect("/");
            }

            // Get the quiz with all question/answers and return the view model
            var quiz = await this.quizzesService.GetQuizByIdAsync<QuizViewModel>(quizId);
            var quizQuestions = await this.quizzesService.GetQuestionsByQuizzIdAsync<QuestionViewModel>(quizId);

            if (quiz == null || quizQuestions.Count() == 0)
            {
                return this.NotFound();
            }

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

            await this.quizzesService.AddUserToQuizAsync(user.Id, quizId);

            if (!user.IsSpecialist)
            {
                await this.adsService.MakeAdsVipAsync(user.Id);
            }

            return this.Redirect("/");
        }
    }
}
