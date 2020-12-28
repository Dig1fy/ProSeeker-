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
    using ProSeeker.Web.ViewModels.Surveys;
    using ProSeeker.Web.ViewModels.Surveys.Answers;
    using ProSeeker.Web.ViewModels.Surveys.Questions;
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

        [Fact]
        public async Task ShouldReturnSurveyTitleByGivenSurveyId()
        {
            var surveyId = "1";
            var expectedTitle = "Go go Rangers!";

            var actualTitle = await this.service.GetSurveyTitleByIdAsync(surveyId);

            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public async Task ShouldReturnAsnwerByAnswerId()
        {
            AutoMapperConfig.RegisterMappings(typeof(AnswerViewModel).Assembly);

            var answerId = "2";
            var expectedText = "Third question's answer!";

            var answer = await this.service.GetSingleAnswerAsync<AnswerViewModel>(answerId);

            Assert.Equal(expectedText, answer.Text);
            Assert.Equal(answerId, answer.Id);
        }

        [Fact]
        public async Task ShouldReturnCorrectQuestionByGivenQuestionId()
        {
            AutoMapperConfig.RegisterMappings(typeof(QuestionViewModel).Assembly);

            var questionId = "1";
            var expectedText = "What's your favourite horror movie?";

            var question = await this.service.GetQuestionByIdAsync<QuestionViewModel>(questionId);

            Assert.Equal(expectedText, question.Text);
            Assert.Equal(questionId, question.Id);
        }

        [Fact]
        public async Task ShouldReturnCorrentNumberOfQuestionsByGivenSurveyId()
        {
            var surveyId = "1";
            var expectedQuestionsCount = 3;

            var actualCount = await this.service.GetQuestionNumberBySurveyIdAsync(surveyId);

            Assert.Equal(expectedQuestionsCount, actualCount);
        }

        [Fact]
        public async Task ShouldReturnAllSurveysProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SurveyViewModel).Assembly);
            var expectedSurveysCount = 1;

            var allSurveys = await this.service.GetAllSurveysAsync<SurveyViewModel>();

            Assert.Equal(expectedSurveysCount, allSurveys.Count());
            Assert.Equal("1", allSurveys.First().Id);
        }

        [Fact]
        public async Task ShouldReturnAllQuestionByGivenSurveyId()
        {
            AutoMapperConfig.RegisterMappings(typeof(QuestionViewModel).Assembly);
            var surveyId = "1";
            var expectedCount = 3;

            var allSurveyQuestions = await this.service.GetAllQuestionsBySurveyIdAsync<QuestionViewModel>(surveyId);

            Assert.Equal(expectedCount, allSurveyQuestions.Count());
        }

        [Fact]
        public async Task ShouldReturnAllAnswersByGivenQuestionId()
        {
            AutoMapperConfig.RegisterMappings(typeof(AnswerViewModel).Assembly);
            var questionId = "1";
            var expectedCount = 1;
            var expectedAnswerId = "1";

            var allQuestionAnswers = await this.service.GetAllAnswersByQuestionIdAsync<AnswerViewModel>(questionId);

            Assert.Equal(expectedCount, allQuestionAnswers.Count());
            Assert.Equal(expectedAnswerId, allQuestionAnswers.First().Id);
        }

        [Fact]
        public async Task ShouldEditQuestionByGivenIdAndNewText()
        {
            AutoMapperConfig.RegisterMappings(typeof(QuestionViewModel).Assembly);
            var questionId = "1";
            var newText = "xo-xo-xo";

            await this.service.EditQuestionAsync(questionId, newText);
            var edittedQuestion = await this.service.GetQuestionByIdAsync<QuestionViewModel>(questionId);

            Assert.Equal(newText, edittedQuestion.Text);
        }

        [Fact]
        public async Task ShouldEditAnswerByGivenAnswerIdAndNewText()
        {
            AutoMapperConfig.RegisterMappings(typeof(AnswerViewModel).Assembly);
            var answerId = "1";
            var newText = "xo-xo-xo";

            await this.service.EditAnswerAsync(answerId, newText);
            var edittedAnswer = await this.service.GetSingleAnswerAsync<AnswerViewModel>(answerId);

            Assert.Equal(newText, edittedAnswer.Text);
        }

        [Fact]
        public async Task ShouldEditSurveyByGivenIdAndTitle()
        {
            AutoMapperConfig.RegisterMappings(typeof(SurveyViewModel).Assembly);
            var surveyId = "1";
            var newTitle = "xo-xo-xo";

            await this.service.EditSurveyAsync(surveyId, newTitle);
            var edittedSurvey = await this.service.GetSurveyByIdAsync<SurveyViewModel>(surveyId);

            Assert.Equal(newTitle, edittedSurvey.Title);
        }

        [Fact]
        public async Task ShouldDeleteSurveyProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SurveyViewModel).Assembly);
            var surveyId = "1";

            await this.service.DeleteSurveyAsync(surveyId);
            var deletedSurvey = await this.service.GetSurveyByIdAsync<SurveyViewModel>(surveyId);

            Assert.Null(deletedSurvey);
        }

        [Fact]
        public async Task ShouldDeleteQuestionProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(QuestionViewModel).Assembly);
            var questionId = "1";

            await this.service.DeleteQuestionAsync(questionId);
            var deletedQuestion = await this.service.GetQuestionByIdAsync<QuestionViewModel>(questionId);

            Assert.Null(deletedQuestion);
        }

        [Fact]
        public async Task ShouldDeleteAnswerProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(AnswerViewModel).Assembly);
            var answerId = "1";

            await this.service.DeleteAnswerAsync(answerId);
            var deletedAnswer = await this.service.GetSingleAnswerAsync<AnswerViewModel>(answerId);

            Assert.Null(deletedAnswer);
        }

        [Fact]
        public async Task ShouldDeleteAllQuestionByGivenSurveyId()
        {
            AutoMapperConfig.RegisterMappings(typeof(QuestionViewModel).Assembly);
            var surveyId = "1";

            await this.service.DeleteAllQuestionsAsync(surveyId);
            var deletedSurvey = await this.service.GetAllQuestionsBySurveyIdAsync<QuestionViewModel>(surveyId);
            var expectedCount = 0;

            Assert.Equal(expectedCount, deletedSurvey.Count());
        }

        [Fact]
        public async Task ShouldCreateSurveyProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SurveyViewModel).Assembly);
            var newSurveyId = "555";
            var inputModel = new NewSurveyInputModel
            {
                Id = newSurveyId,
                Title = "20",
            };

            var surveyId = await this.service.CreateSurveyAsync(inputModel);
            var allSurveysCount = await this.service.GetAllSurveysAsync<SurveyViewModel>();
            Assert.Equal(2, allSurveysCount.Count());
        }

        [Fact]
        public async Task ShouldCreateNewQuestionProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(QuestionViewModel).Assembly);
            var surveyId = "1";
            var inputModel = new NewQuestionInputModel
            {
                Text = "20",
                SurveyId = surveyId,
            };

            var questionId = await this.service.CreateQuestionAsync(inputModel);
            var allQuestion = await this.service.GetAllQuestionsBySurveyIdAsync<QuestionViewModel>(surveyId);
            var expectedQuestionsCount = 4;

            Assert.Equal(expectedQuestionsCount, allQuestion.Count());
            Assert.NotNull(questionId);
        }

        [Fact]
        public async Task ShouldCreateNewAnswerCorrectly()
        {
            var questionId = "1";
            var newAnswersText = "xo-xo";

            var newAnswerId = await this.service.CreateAnswerAsync(questionId, newAnswersText);

            Assert.NotNull(newAnswerId);
        }

        [Fact]
        public async Task ShouldDeleteAllAnswersByGivenQuestionId()
        {
            AutoMapperConfig.RegisterMappings(typeof(AnswerViewModel).Assembly);
            var questionId = "1";

            await this.service.DeleteAllAnswersAsync(questionId);
            var allAnswers = await this.service.GetAllAnswersByQuestionIdAsync<AnswerViewModel>(questionId);
            var expectedAnswersCount = 0;

            Assert.Equal(expectedAnswersCount, allAnswers.Count());
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
