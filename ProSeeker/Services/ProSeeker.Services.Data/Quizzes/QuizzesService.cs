namespace ProSeeker.Services.Data.Quizz
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data.Common.Repositories;
    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Services.Mapping;

    public class QuizzesService : IQuizzesService
    {
        private readonly IDeletableEntityRepository<Quiz> quizzesRepository;
        private readonly IDeletableEntityRepository<Question> questionsRepository;
        private readonly IDeletableEntityRepository<Answer> answersRepository;

        public QuizzesService(
            IDeletableEntityRepository<Quiz> quizzesRepository,
            IDeletableEntityRepository<Question> questionsRepository,
            IDeletableEntityRepository<Answer> answersRepository)
        {
            this.quizzesRepository = quizzesRepository;
            this.questionsRepository = questionsRepository;
            this.answersRepository = answersRepository;
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

        public async Task<IEnumerable<T>> GetQuestionsByQuizzIdAsync<T>(string quizId)
        {
            var quizQuestions = await this.questionsRepository
                .All()
                .Where(x => x.QuizId == quizId)
                .To<T>()
                .ToListAsync();

            return quizQuestions;
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
