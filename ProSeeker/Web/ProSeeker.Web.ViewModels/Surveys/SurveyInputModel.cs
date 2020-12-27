namespace ProSeeker.Web.ViewModels.Quizzes
{
    using System.ComponentModel.DataAnnotations;

    public class SurveyInputModel
    {
        public int QuestionNumber { get; set; }

        [Required(ErrorMessage = "Моля, попълнете полето 'Отговор'!")]
        [MaxLength(250, ErrorMessage = "Отговорът не може да бъде по-дълъг от 250 символа.")]
        public string AnswerText { get; set; }
    }
}
