namespace ProSeeker.Services.Data.Tests.Surveys
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Quizz;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Quizzes;
    using Xunit;

    public sealed class SurveysServiceTests : BaseServiceTests
    {
        private readonly ISurveysService service;

        private List<ApplicationUser> users;
        private List<Survey> surveys;
        private List<Question> questions;
        private List<UserSurvey> usersSurveys;
        private List<Answer> answers;

        public SurveysServiceTests()
        {
            this.users = new List<ApplicationUser>();
            this.surveys = new List<Survey>();
            this.questions = new List<Question>();
            this.usersSurveys = new List<UserSurvey>();
            this.answers = new List<Answer>();

            var surveysRepository = new EfDeletableEntityRepository<Survey>(this.DbContext);
            var questionsRepository = new EfDeletableEntityRepository<Question>(this.DbContext);
            var answersRepository = new EfDeletableEntityRepository<Answer>(this.DbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(this.DbContext);
            var usersServeysRepository = new EfRepository<UserSurvey>(this.DbContext);
            this.service = new SurveysService(
                surveysRepository,
                questionsRepository,
                answersRepository,
                usersRepository,
                usersServeysRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task ShouldAddUserToSurveyProperly()
        {
            var userId = "1";
            var surveyId = "1";
            await this.service.AddUserToSurveyAsync(userId, surveyId);
            var isUserAdded = await this.service.HasItBeenCompletedByThisUser(userId);

            Assert.True(isUserAdded);
        }

        [Fact]
        public async Task ShouldReturnAllAnswersForGivenQuestionCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(AnswerViewModel).Assembly);

            var questionId = "1";
            var answers = await this.service.GetAnswersByQuestionIdAsync<AnswerViewModel>(questionId);

            Assert.Single(answers);
            Assert.Equal("1", answers.First().Id);
        }

        [Fact]
        public async Task ShouldReturnAllQuestionsByGivenSurveyIdProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(QuestionViewModel).Assembly);

            var surveyId = "1";
            var questions = await this.service.GetQuestionsBySurveyIdAsync<QuestionViewModel>(surveyId);
            var expectedQuestionsCount = 3;

            Assert.Equal(expectedQuestionsCount, questions.Count());
        }

        [Fact]
        public async Task ShouldReturnSurveyDetailsByGivenSurveyIdCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SurveyViewModel).Assembly);

            var surveyId = "1";
            var survey = await this.service.GetSurveyByIdAsync<SurveyViewModel>(surveyId);

            Assert.NotNull(survey);
            Assert.Equal("1", survey.Id);
            Assert.Equal(3, survey.Questions.Count());
        }

        private void InitializeRepositoriesData()
        {
            this.users.AddRange(new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "1",
                    FirstName = "Ivo",
                    LastName = "Ivov",
                    CityId = 1,
                    Email = "u@u",
                    ProfilePicture = "SomeProfilePicture",
                    IsSpecialist = false,
                },
                new ApplicationUser
                {
                    Id = "2",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    CityId = 1,
                    Email = "s@s",
                    ProfilePicture = "SpecProfilePicture",
                    IsSpecialist = true,
                    SpecialistDetailsId = "specialistId",
                },
                new ApplicationUser
                {
                    Id = "3",
                    FirstName = "E",
                    LastName = "E",
                    CityId = 1,
                    Email = "s@s",
                },
            });

            this.surveys.AddRange(new List<Survey>
            {
                new Survey
                {
                    Id = "1",
                    Title = "Go go Rangers!",
                    Questions = new List<Question>()
                    {
                        new Question
                        {
                            Id = "1",
                            Number = 1,
                            SurveyId = "1",
                            Text = "What's your favourite horror movie?",
                            Answers = new List<Answer>()
                            {
                                new Answer
                                {
                                    Id = "1",
                                    Text = "Yes",
                                    QuestionId = "1",
                                },
                            },
                        },
                    },
                },
            });

            this.questions.AddRange(new List<Question>
            {
                new Question
                {
                    Id = "2",
                    SurveyId = "1",
                    Number = 2,
                    Text = "Not nice, huh?",
                },
                new Question
                {
                    Id = "3",
                    SurveyId = "1",
                    Number = 3,
                    Text = "What?",
                },
            });

            this.answers.AddRange(new List<Answer>
            {
                new Answer
                {
                    Id = "2",
                    Text = "Third question's answer!",
                    QuestionId = "2",
                },
                new Answer
                {
                    Id = "3",
                    Text = "Third question's answer!",
                    QuestionId = "3",
                },
            });

            this.DbContext.AddRange(this.users);
            this.DbContext.AddRange(this.usersSurveys);
            this.DbContext.AddRange(this.surveys);
            this.DbContext.AddRange(this.questions);
            this.DbContext.AddRange(this.answers);
            this.DbContext.SaveChanges();
        }
    }
}
