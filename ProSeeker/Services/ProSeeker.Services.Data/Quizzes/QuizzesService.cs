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

    public class QuizzesService : IQuizzesService
    {
        private readonly IDeletableEntityRepository<Quiz> quizzesRepository;
        private readonly IDeletableEntityRepository<Question> questionsRepository;
        private readonly IDeletableEntityRepository<Answer> answersRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IRepository<UserQuiz> userQuizzesRepository;

        public QuizzesService(
            IDeletableEntityRepository<Quiz> quizzesRepository,
            IDeletableEntityRepository<Question> questionsRepository,
            IDeletableEntityRepository<Answer> answersRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<UserQuiz> userQuizzesRepository)
        {
            this.quizzesRepository = quizzesRepository;
            this.questionsRepository = questionsRepository;
            this.answersRepository = answersRepository;
            this.usersRepository = usersRepository;
            this.userQuizzesRepository = userQuizzesRepository;
        }

        public async Task AddUserToQuizAsync(string userId, string quizId)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(x => x.Id == userId);

            var userQuiz = new UserQuiz
            {
                User = user,
                UserId = user.Id,
                QuizId = quizId,
            };

            await this.userQuizzesRepository.AddAsync(userQuiz);
            await this.userQuizzesRepository.SaveChangesAsync();
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

        public async Task<bool> HasItBeenCompletedByThisUser(string userId)
        {
            var userQuiz = await this.userQuizzesRepository
                .All()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return userQuiz != null;
        }
    }
}
