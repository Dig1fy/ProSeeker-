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

        public async Task<IEnumerable<T>> GetAnswersByQuestionIdAsync<T>(string questionId)
        {
            var answers = await this.answersRepository
                .All()
                .Where(x => x.QuestionId == questionId)
                .To<T>()
                .ToListAsync();

            return answers;
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

        public async Task<T> GetSurveyByIdAsync<T>(string surveyId)
        {
            var survey = await this.surveysRepository
                .All()
                .Where(x => x.Id == surveyId)
                .To<T>()
                .FirstOrDefaultAsync();

            return survey;
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
