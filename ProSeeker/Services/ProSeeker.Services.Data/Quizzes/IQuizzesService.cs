namespace ProSeeker.Services.Data.Quizz
{
    using System.Threading.Tasks;

    public interface IQuizzesService
    {
        Task<T> GetQuizByIdAsync<T>(string quizId);

        Task<T> GetQuestionsByQuizzIdAsync<T>(string quizId);
    }
}
