namespace ProSeeker.Web.ViewModels.Quizzes
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Services.Mapping;

    public class QuestionViewModel : IMapFrom<Question>
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public int Number { get; set; }

        public virtual IEnumerable<AnswerViewModel> Answers { get; set; }

        public string SurveyId { get; set; }
    }
}
