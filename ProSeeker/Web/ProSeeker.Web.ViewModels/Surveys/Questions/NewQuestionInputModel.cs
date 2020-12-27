namespace ProSeeker.Web.ViewModels.Surveys.Questions
{
    using System.ComponentModel.DataAnnotations;

    public class NewQuestionInputModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Моля, попълнете полето 'Текст'!")]
        [MaxLength(150, ErrorMessage ="Текстът на въпроса не може да бъде повече от 150 символа.")]
        [Display(Name = "Текст")]
        public string Text { get; set; }

        public int Number { get; set; }

        public string SurveyId { get; set; }

        public string SurveyTitle { get; set; }
    }
}
