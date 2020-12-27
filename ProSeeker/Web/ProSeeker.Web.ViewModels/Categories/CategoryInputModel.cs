namespace ProSeeker.Web.ViewModels.Categories
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Web.ViewModels.BaseJobCategories;

    public class CategoryInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Моля, попълнете име на категорията")]
        [MaxLength(70, ErrorMessage = "Името на категория не може да бъде повече от 70 символа")]
        [Display(Name = "Име на категория")]
        public string Name { get; set; }

        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "Моля, попълнете име на категорията")]
        [MaxLength(500, ErrorMessage = "Описанието на категория не може да бъде повече от 500 символа")]
        [Display(Name = "Описание на категория")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Моля, изберете базова категория!")]
        [Display(Name = "Базова категория")]
        public int BaseJobCategoryId { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IEnumerable<SimpleBaseJobCategoryViewModel> BaseJobCategories { get; set; }
    }
}
