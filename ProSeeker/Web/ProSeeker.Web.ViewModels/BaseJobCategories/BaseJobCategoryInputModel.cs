namespace ProSeeker.Web.ViewModels.BaseJobCategories
{
    using System.ComponentModel.DataAnnotations;

    public class BaseJobCategoryInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Моля, попълнете име на категорията")]
        [MaxLength(50, ErrorMessage ="Името на категория не може да бъде повече от 80 символа")]
        [Display(Name ="Име на категория")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Моля, попълнете име на категорията")]
        [MaxLength(250, ErrorMessage ="Описанието на категория не може да бъде повече от 80 символа")]
        [Display(Name = "Описание на категория")]
        public string Description { get; set; }
    }
}
