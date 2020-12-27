namespace ProSeeker.Web.ViewModels.Surveys
{
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Models.Quiz;
    using ProSeeker.Services.Mapping;

    public class NewSurveyInputModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Моля, попълнете заглавие на анкетата!")]
        [MaxLength(250, ErrorMessage ="Заглавието на анкетата не може да бъде по-дълго от 250 символа.")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }
    }
}
