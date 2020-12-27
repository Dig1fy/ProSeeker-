namespace ProSeeker.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Common;
    using ProSeeker.Data;
    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Services.Data.Quizz;
    using ProSeeker.Web.Controllers;
    using ProSeeker.Web.ViewModels.Quizzes;
    using ProSeeker.Web.ViewModels.Surveys;
    using ProSeeker.Web.ViewModels.Surveys.Answers;
    using ProSeeker.Web.ViewModels.Surveys.Questions;

    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class SurveysController : BaseController
    {
        private readonly ISurveysService surveysService;

        public SurveysController(ISurveysService surveysService)
        {
            this.surveysService = surveysService;
        }

        // =============================================== SURVEY ===============================================
        public async Task<IActionResult> SurveyIndex()
        {
            var allSurveys = await this.surveysService.GetAllSurveysAsync<SurveyViewModel>();

            foreach (var survey in allSurveys)
            {
                survey.Questions = await this.surveysService.GetAllQuestionsBySurveyIdAsync<QuestionViewModel>(survey.Id);

                foreach (var question in survey.Questions)
                {
                    question.Answers = await this.surveysService.GetAllAnswersByQuestionIdAsync<AnswerViewModel>(question.Id);
                }
            }

            var viewModel = new AllSurveysViewModel { Surveys = allSurveys };

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewSurveyInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var newSurveyId = await this.surveysService.CreateSurveyAsync(inputModel);

            return this.RedirectToAction(nameof(this.SurveyIndex));
        }

        public async Task<IActionResult> EditSurvey(string surveyId)
        {
            if (surveyId == null)
            {
                return this.CustomNotFound();
            }

            var inputModel = new NewSurveyInputModel();

            try
            {
                var survey = await this.surveysService.GetSurveyByIdAsync<SurveyViewModel>(surveyId);
                inputModel.Title = survey.Title;
                inputModel.Id = survey.Id;
            }
            catch (Exception)
            {
                return this.CustomCommonError();
            }

            return this.View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSurvey(NewSurveyInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.surveysService.EditSurveyAsync(inputModel.Id, inputModel.Title);
            }
            catch (Exception)
            {
                return this.CustomCommonError();
            }

            return this.RedirectToAction(nameof(this.SurveyIndex));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSurvey(string surveyId)
        {
            if (surveyId == null)
            {
                return this.CustomNotFound();
            }

            try
            {
                // If we want to delete a survey, first we will have to delete all answers (by given question id),
                // then all questions (by surveyId) and finally the survey itself
                var allSurveyQuestions = await this.surveysService.GetAllQuestionsBySurveyIdAsync<QuestionViewModel>(surveyId);

                foreach (var question in allSurveyQuestions)
                {
                    await this.surveysService.DeleteAllAnswersAsync(question.Id);
                }

                await this.surveysService.DeleteAllQuestionsAsync(surveyId);
                await this.surveysService.DeleteSurveyAsync(surveyId);
            }
            catch (Exception)
            {
                return this.CustomNotFound();
            }

            return this.RedirectToAction(nameof(this.SurveyIndex));
        }

        // =============================================== QUESTION ===============================================
        public async Task<IActionResult> CreateQuestion(string id)
        {
            var surveyTitle = await this.surveysService.GetSurveyTitleByIdAsync(id);

            if (surveyTitle == null)
            {
                return this.CustomNotFound();
            }

            var questionNumber = await this.surveysService.GetQuestionNumberBySurveyIdAsync(id);
            var nextQuestionNumber = questionNumber + 1;

            var model = new NewQuestionInputModel
            {
                SurveyId = id,
                SurveyTitle = surveyTitle,
                Number = nextQuestionNumber,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(NewQuestionInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var newQuestion = await this.surveysService.CreateQuestionAsync(inputModel);

            return this.RedirectToAction(nameof(this.SurveyIndex));
        }

        public async Task<IActionResult> EditQuestion(string questionId)
        {
            if (questionId == null)
            {
                return this.CustomNotFound();
            }

            var inputModel = new NewQuestionInputModel();

            try
            {
                var existingQuestion = await this.surveysService.GetQuestionByIdAsync<QuestionViewModel>(questionId);
                inputModel.Id = existingQuestion.Id;
                inputModel.Text = existingQuestion.Text;
                inputModel.Number = existingQuestion.Number;
            }
            catch (Exception)
            {
                return this.CustomCommonError();
            }

            return this.View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(NewQuestionInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.surveysService.EditQuestionAsync(inputModel.Id, inputModel.Text);
            }
            catch (Exception)
            {
                return this.CustomCommonError();
            }

            return this.RedirectToAction(nameof(this.SurveyIndex));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion(string questionId)
        {
            if (questionId == null)
            {
                return this.CustomNotFound();
            }

            try
            {
                // before deleting the question, we'll have to delete all of it's answers
                await this.surveysService.DeleteAllAnswersAsync(questionId);
                await this.surveysService.DeleteQuestionAsync(questionId);
            }
            catch (Exception)
            {
                return this.CustomNotFound();
            }

            return this.RedirectToAction(nameof(this.SurveyIndex));
        }

        // =============================================== Answer ===============================================
        public async Task<IActionResult> CreateAnswer(string surveyId, string questionId)
        {
            var surveyTitle = await this.surveysService.GetSurveyTitleByIdAsync(surveyId);
            var question = await this.surveysService.GetQuestionByIdAsync<QuestionViewModel>(questionId);
            var questionText = question.Text;

            if (surveyTitle == null || questionText == null)
            {
                return this.CustomNotFound();
            }

            var model = new NewAnswerInputModel
            {
                SurveyTitle = surveyTitle,
                SurveyId = surveyId,
                QuestionId = questionId,
                QuestionText = questionText,
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnswer(NewAnswerInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var newAnswerId = await this.surveysService.CreateAnswerAsync(inputModel.QuestionId, inputModel.Text);

            if (newAnswerId == null)
            {
                return this.CustomCommonError();
            }

            return this.RedirectToAction(nameof(this.SurveyIndex));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAnswer(string answerId)
        {
            if (answerId == null)
            {
                return this.CustomNotFound();
            }

            try
            {
                await this.surveysService.DeleteAnswerAsync(answerId);
            }
            catch (Exception)
            {
                return this.CustomNotFound();
            }

            return this.RedirectToAction(nameof(this.SurveyIndex));
        }

        public async Task<IActionResult> EditAnswer(string answerId)
        {
            if (answerId == null)
            {
                return this.CustomNotFound();
            }

            var inputModel = new NewAnswerInputModel();

            try
            {
                var existingAnswer = await this.surveysService.GetSingleAnswerAsync<AnswerViewModel>(answerId);
                inputModel.Id = existingAnswer.Id;
                inputModel.Text = existingAnswer.Text;
            }
            catch (Exception)
            {
                return this.CustomCommonError();
            }

            return this.View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnswer(NewAnswerInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.surveysService.EditAnswerAsync(inputModel.Id, inputModel.Text);
            }
            catch (Exception)
            {
                return this.CustomCommonError();
            }

            return this.RedirectToAction(nameof(this.SurveyIndex));
        }
    }
}
