namespace ProSeeker.Services.Data.Quizz
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Surveys;
    using ProSeeker.Web.ViewModels.Surveys.Questions;

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

        Task<string> CreateAnswerAsync(string questionId, string answerText);

        Task DeleteSurveyAsync(string surveyId);

        Task DeleteQuestionAsync(string questionId);

        Task DeleteAnswerAsync(string answerId);

        Task DeleteAllQuestionsAsync(string surveyId);

        Task DeleteAllAnswersAsync(string questionId);
    }
}
