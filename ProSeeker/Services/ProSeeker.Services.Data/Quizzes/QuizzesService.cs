namespace ProSeeker.Services.Data.Quizz
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Services.Mapping;

    public class QuizzesService : IQuizzesService
    {
        private readonly IDeletableEntityRepository<Quiz> quizzesRepository;

        public QuizzesService(IDeletableEntityRepository<Quiz> quizzesRepository)
        {
            this.quizzesRepository = quizzesRepository;
        }

        public async Task<T> GetQuizByIdAsync<T>(string quizId)
        {
            var quiz = await this.quizzesRepository
                .All()
                .Where(x => x.Id == quizId)
                .To<T>()
                .FirstOrDefaultAsync();

            return quiz;
        }
    }
}
