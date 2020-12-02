namespace ProSeeker.Web.ViewModels.Ads
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Cities;

    public class CreateAdInputModel : IMapFrom<Ad>
    {
        [Required(ErrorMessage = "Моля, попълнете полето 'Заглавие на обява'")]
        [Display(Name = "Заглавие на обява")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Моля, попълнете описание на обявата!")]
        [StringLength(25000, ErrorMessage = "Описанието трябва да бъде поне 20 символа.", MinimumLength = 20)]
        //[MinLength(20, ErrorMessage = "Описанието трябва да бъде поне 20 символа.")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Моля, попълнете полето 'Предвиден бюджет'")]
        [Display(Name = "Предвиден бюджет (свободен текст)")]
        public string PreparedBudget { get; set; }

        [Required(ErrorMessage = "Моля, изберете категория от падащото меню!")]
        [Display(Name = "Какъв професионалист Ви е нужен?")]
        public int JobCategoryId { get; set; }

        public JobCategory JobCategory { get; set; }

        [Required(ErrorMessage = "Моля, изберете град от падащото меню!")]
        public int CityId { get; set; }

        public City City { get; set; }

        public bool IsVip { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Opinion> Opinions { get; set; }

        public IEnumerable<CitySimpleViewModel> Cities { get; set; }

        public IEnumerable<CategorySimpleViewModel> Categories { get; set; }
    }
}
