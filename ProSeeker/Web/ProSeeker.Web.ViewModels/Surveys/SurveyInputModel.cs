namespace ProSeeker.Web.ViewModels.Quizzes
{
    using System.ComponentModel.DataAnnotations;

    public class SurveyInputModel
    {
        public int QuestionNumber { get; set; }

        [Required]
        [MaxLength(250)]
        public string AnswerText { get; set; }
    }
}
