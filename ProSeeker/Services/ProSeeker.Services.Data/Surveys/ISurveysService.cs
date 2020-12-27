namespace ProSeeker.Services.Data.Quizz
{
    using ProSeeker.Web.ViewModels.Quizzes;
    using ProSeeker.Web.ViewModels.Surveys;
    using ProSeeker.Web.ViewModels.Surveys.Questions;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISurveysService
    {
        Task<T> GetSurveyByIdAsync<T>(string surveyId);

        Task<IEnumerable<T>> GetQuestionsBySurveyIdAsync<T>(string surveyId);

        Task<IEnumerable<T>> GetAnswersByQuestionIdAsync<T>(string questionId);

        Task AddUserToSurveyAsync(string userId, string surveyId);

        Task<bool> HasItBeenCompletedByThisUser(string userId);

        Task<IEnumerable<T>> GetAllSurveysAsync<T>();

        Task<IEnumerable<T>> GetAllQuestionsBySurveyIdAsync<T>(string surveyId);

        Task<IEnumerable<T>> GetAllAnswersByQuestionIdAsync<T>(string questionId);

        Task<string> CreateSurveyAsync(NewSurveyInputModel inputModel);

        Task<int> GetQuestionNumberBySurveyIdAsync(string surveyId);

        Task<string> CreateQuestionAsync(NewQuestionInputModel inputModel);

        Task<string> GetSurveyTitleByIdAsync(string surveyId);

        Task<string> GetQuestionTextByIdAsync(string questionId);
    }
}
