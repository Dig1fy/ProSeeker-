namespace ProSeeker.Services.Data.Quizz
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IQuizzesService
    {
        Task<T> GetQuizByIdAsync<T>(string quizId);

        Task<IEnumerable<T>> GetQuestionsByQuizzIdAsync<T>(string quizId);

        Task<IEnumerable<T>> GetAnswersByQuestionIdAsync<T>(string questionId);

        Task AddUserToQuizAsync(string userId, string quizId);

        Task<bool> HasItBeenCompletedByThisUser(string userId);
    }
}
