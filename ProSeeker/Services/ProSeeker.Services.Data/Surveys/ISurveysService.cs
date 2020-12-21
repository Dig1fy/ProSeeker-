namespace ProSeeker.Services.Data.Quizz
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISurveysService
    {
        Task<T> GetSurveyByIdAsync<T>(string surveyId);

        Task<IEnumerable<T>> GetQuestionsBySurveyIdAsync<T>(string surveyId);

        Task<IEnumerable<T>> GetAnswersByQuestionIdAsync<T>(string questionId);

        Task AddUserToSurveyAsync(string userId, string surveyId);

        Task<bool> HasItBeenCompletedByThisUser(string userId);
    }
}
