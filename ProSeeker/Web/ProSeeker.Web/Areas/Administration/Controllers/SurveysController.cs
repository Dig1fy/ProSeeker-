namespace ProSeeker.Web.Areas.Administration.Controllers
{
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

        // =============================================== QUESTION =============================================== 
        //public async Task<IActionResult> QuestionIndex()
        //{
        //    //var allSurveys = await this.surveysService.GetAllSurveysAsync<SurveyViewModel>();

        //    //foreach (var survey in allSurveys)
        //    //{
        //    //    survey.Questions = await this.surveysService.GetAllQuestionsBySurveyIdAsync<QuestionViewModel>(survey.Id);

        //    //    foreach (var question in survey.Questions)
        //    //    {
        //    //        question.Answers = await this.surveysService.GetAllAnswersByQuestionIdAsync<AnswerViewModel>(question.Id);
        //    //    }
        //    //}

        //    //var viewModel = new AllSurveysViewModel { Surveys = allSurveys };

        //    return this.View();
        //}

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

        // =============================================== Answer ===============================================
        public async Task<IActionResult> CreateAnswer(string surveyId, string questionId)
        {
            var surveyTitle = await this.surveysService.GetSurveyTitleByIdAsync(surveyId);
            var questionText = await this.surveysService.GetQuestionTextByIdAsync(questionId);

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



        // GET: Administration/Surveys/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var survey = await _context.Surveys.FindAsync(id);
        //    if (survey == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(survey);
        //}

        // POST: Administration/Surveys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Title,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Survey survey)
        //{
        //    if (id != survey.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(survey);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SurveyExists(survey.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(survey);
        //}

        //// GET: Administration/Surveys/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var survey = await _context.Surveys
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (survey == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(survey);
        //}

        //// POST: Administration/Surveys/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var survey = await _context.Surveys.FindAsync(id);
        //    _context.Surveys.Remove(survey);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool SurveyExists(string id)
        //{
        //    return _context.Surveys.Any(e => e.Id == id);
        //}
    }
}
