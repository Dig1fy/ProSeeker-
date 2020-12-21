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

    public class SurveyController : BaseController
    {
        private readonly ISurveysService surveysService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly IAdsService adsService;

        public SurveyController(
            ISurveysService surveysService,
            UserManager<ApplicationUser> userManager,
            IUsersService usersService,
            IAdsService adsService)
        {
            this.surveysService = surveysService;
            this.userManager = userManager;
            this.usersService = usersService;
            this.adsService = adsService;
        }

        public IActionResult Survey()
        {
            return this.View();
        }

        public async Task<IActionResult> Start(string surveyId)
        {
            // Check if the user has already completed this quiz
            var user = await this.userManager.GetUserAsync(this.User);
            var hasItBeenCompletedAlready = await this.surveysService.HasItBeenCompletedByThisUser(user.Id);

            if (hasItBeenCompletedAlready)
            {
                return this.Redirect("/");
            }

            // Get the quiz with all question/answers and return the view model
            var survey = await this.surveysService.GetSurveyByIdAsync<SurveyViewModel>(surveyId);
            var surveyQuestions = await this.surveysService.GetQuestionsBySurveyIdAsync<QuestionViewModel>(surveyId);

            if (survey == null || surveyQuestions.Count() == 0)
            {
                return this.NotFound();
            }

            survey.Questions = surveyQuestions;

            foreach (var question in survey.Questions)
            {
                question.Answers = await this.surveysService.GetAnswersByQuestionIdAsync<AnswerViewModel>(question.Id);
            }

            var surveyModel = new SurveyViewModel
            {
                Id = surveyId,
                Title = survey.Title,
                Questions = survey.Questions,
            };

            return this.View(surveyModel);
        }

        public async Task<IActionResult> End(string surveyId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            await this.usersService.MakeUserVip(user.Id);

            await this.surveysService.AddUserToSurveyAsync(user.Id, surveyId);

            if (!user.IsSpecialist)
            {
                await this.adsService.MakeAdsVipAsync(user.Id);
            }

            return this.Redirect("/");
        }
    }
}
