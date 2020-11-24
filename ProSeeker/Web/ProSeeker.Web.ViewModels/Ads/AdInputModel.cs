namespace ProSeeker.Web.ViewModels.Ads
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class AdInputModel : IMapFrom<Ad>
    {
        [Required(ErrorMessage = "Моля, попълнете полето 'Заглавие на обява'")]
        [Display(Name = "Заглавие на обява")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Предвиден бюджет (свободен текст)")]
        public string PreparedBudget { get; set; }

        public int JobCategoryId { get; set; }

        [Required]
        public JobCategory JobCategory { get; set; }

        public int CityId { get; set; }

        [Required]
        public City City { get; set; }

        public bool IsVip { get; set; }

        public string UserId { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Opinion> Opinions { get; set; }
    }
}
