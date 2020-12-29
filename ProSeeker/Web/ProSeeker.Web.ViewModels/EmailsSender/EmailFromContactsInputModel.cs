namespace ProSeeker.Web.ViewModels.EmailsSender
{
    using System.ComponentModel.DataAnnotations;

    public class EmailFromContactsInputModel
    {
        [Required(ErrorMessage ="Моля, попълнете имената си")]
        [Display(Name = "Име и фамилия")]
        [MaxLength(100, ErrorMessage ="Името не може да бъде повече от 100 символа")]
        public string FromName { get; set; }

        [Required(ErrorMessage = "Моля, попълнете имейла си")]
        [Display(Name = "Електронна Ви поща (имейл)")]
        [EmailAddress(ErrorMessage ="Невалиден имейл.")]
        [MaxLength(50, ErrorMessage = "Имейлът не може да бъде повече от 50 символа")]
        public string FromEmail { get; set; }

        [Required(ErrorMessage = "Моля, попълнете темата на запитването Ви!")]
        [Display(Name = "Тема")]
        [MaxLength(150, ErrorMessage = "Темата не може да бъде повече от 100 символа")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Моля, попълнете съобщението!")]
        [Display(Name ="Вашето съобщение")]
        [MaxLength(5000, ErrorMessage = "Съобщението Ви не може да бъде повече от 5000 символа")]
        public string Content { get; set; }
    }
}
