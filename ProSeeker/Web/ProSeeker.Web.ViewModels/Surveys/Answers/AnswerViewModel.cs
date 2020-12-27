namespace ProSeeker.Web.ViewModels.Quizzes
{
    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Services.Mapping;

    public class AnswerViewModel : IMapFrom<Answer>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string QuestionId { get; set; }
    }
}
