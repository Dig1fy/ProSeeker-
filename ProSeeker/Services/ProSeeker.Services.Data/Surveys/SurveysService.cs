namespace ProSeeker.Services.Data.Quizz
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Quizzes;
    using ProSeeker.Web.ViewModels.Surveys;
    using ProSeeker.Web.ViewModels.Surveys.Questions;

    public class SurveysService : ISurveysService
    {
        private readonly IDeletableEntityRepository<Survey> surveysRepository;
        private readonly IDeletableEntityRepository<Question> questionsRepository;
        private readonly IDeletableEntityRepository<Answer> answersRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<UserSurvey> userSurveysRepository;

        public SurveysService(
            IDeletableEntityRepository<Survey> surveysRepository,
            IDeletableEntityRepository<Question> questionsRepository,
            IDeletableEntityRepository<Answer> answersRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<UserSurvey> userSurveysRepository)
        {
            this.surveysRepository = surveysRepository;
            this.questionsRepository = questionsRepository;
            this.answersRepository = answersRepository;
            this.usersRepository = usersRepository;
            this.userSurveysRepository = userSurveysRepository;
        }

        public async Task AddUserToSurveyAsync(string userId, string surveyId)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.Id == userId);

            var userSurvey = new UserSurvey
            {
                User = user,
                UserId = user.Id,
                SurveyId = surveyId,
            };

            await this.userSurveysRepository.AddAsync(userSurvey);
            await this.userSurveysRepository.SaveChangesAsync();
        }

        public async Task<string> CreateQuestionAsync(NewQuestionInputModel inputModel)
        {
            var newQuestion = new Question
            {
                Number = inputModel.Number,
                SurveyId = inputModel.SurveyId,
                Text = inputModel.Text,
            };

            await this.questionsRepository.AddAsync(newQuestion);
            await this.questionsRepository.SaveChangesAsync();

            return newQuestion.Id;
        }

        public async Task<string> CreateSurveyAsync(NewSurveyInputModel inputModel)
        {
            var newSurvey = new Survey
            {
                Title = inputModel.Title,
            };

            await this.surveysRepository.AddAsync(newSurvey);
            await this.surveysRepository.SaveChangesAsync();

            return newSurvey.Id;
        }

        public async Task<IEnumerable<T>> GetAllAnswersByQuestionIdAsync<T>(string questionId)
        {
            var answers = await this.answersRepository
                .All()
                .Where(x => x.QuestionId == questionId)
                .To<T>()
                .ToListAsync();

            return answers;
        }

        public async Task<IEnumerable<T>> GetAllQuestionsBySurveyIdAsync<T>(string surveyId)
        {
            var questions = await this.questionsRepository
                .All()
                .Where(x => x.SurveyId == surveyId)
                .To<T>()
                .ToListAsync();

            return questions;
        }

        public async Task<IEnumerable<T>> GetAllSurveysAsync<T>()
        {
            var surveys = await this.surveysRepository
                .All()
                .To<T>()
                .ToListAsync();

            return surveys;
        }

        public async Task<IEnumerable<T>> GetAnswersByQuestionIdAsync<T>(string questionId)
        {
            var answers = await this.answersRepository
                .All()
                .Where(x => x.QuestionId == questionId)
                .To<T>()
                .ToListAsync();

            return answers;
        }

        public Task<int> GetQuestionNumberBySurveyIdAsync(string surveyId)
        {
            var questionNumber = this.questionsRepository
                .All()
                .Where(x => x.SurveyId == surveyId)
                .CountAsync();

            return questionNumber;
        }

        public async Task<IEnumerable<T>> GetQuestionsBySurveyIdAsync<T>(string surveyId)
        {
            var quizQuestions = await this.questionsRepository
                .All()
                .Where(x => x.SurveyId == surveyId)
                .To<T>()
                .ToListAsync();

            return quizQuestions;
        }

        public async Task<string> GetQuestionTextByIdAsync(string questionId)
        {
            var questionText = await this.questionsRepository
                .All()
                .Where(x => x.Id == questionId)
                .Select(x => x.Text)
                .FirstOrDefaultAsync();

            return questionText;
        }

        public async Task<T> GetSurveyByIdAsync<T>(string surveyId)
        {
            var survey = await this.surveysRepository
                .All()
                .Where(x => x.Id == surveyId)
                .To<T>()
                .FirstOrDefaultAsync();

            return survey;
        }

        public async Task<string> GetSurveyTitleByIdAsync(string surveyId)
        {
            var surveyTitle = await this.surveysRepository
                .All()
                .Where(x => x.Id == surveyId)
                .Select(x => x.Title)
                .FirstOrDefaultAsync();

            return surveyTitle;
        }

        public async Task<bool> HasItBeenCompletedByThisUser(string userId)
        {
            var userSurvey = await this.userSurveysRepository
                .All()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return userSurvey != null;
        }
    }
}
