namespace ProSeeker.Web.ViewModels.Surveys.Answers
{
    using System.ComponentModel.DataAnnotations;

    public class NewAnswerInputModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Моля, попълнете текста на отговора!")]
        [MaxLength(150, ErrorMessage = "Отговорът не може да бъде повече от 150 символа.")]
        [Display(Name = "Текст")]
        public string Text { get; set; }

        public string QuestionId { get; set; }

        public string QuestionText { get; set; }

        public string SurveyId { get; set; }

        public string SurveyTitle { get; set; }
    }
}
