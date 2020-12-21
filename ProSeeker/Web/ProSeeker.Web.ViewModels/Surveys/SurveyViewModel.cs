namespace ProSeeker.Web.ViewModels.Quizzes
{
    using System.Collections.Generic;

    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Services.Mapping;

    public class SurveyViewModel : IMapFrom<Survey>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public virtual IEnumerable<QuestionViewModel> Questions { get; set; }
    }
}
