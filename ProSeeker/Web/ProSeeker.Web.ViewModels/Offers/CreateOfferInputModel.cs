namespace ProSeeker.Web.ViewModels.Offers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class CreateOfferInputModel : IMapFrom<Offer>
    {
        public string AdId { get; set; }

        [Required(ErrorMessage = "Моля, попълнете полето 'Описание на оферта'")]
        [Display(Name = "Описание / допълнителна инфомация")]
        public string Description { get; set; }

        [Display(Name = "Офертна цена /български левове/")]
        [RegularExpression(@"^[1-9][\.\d]*(,\d+)?$", ErrorMessage = "Цената не може да съдържа букви. Примери за валидна цена: '2500', '1200.25', '10.55'")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Кога можете да започнете работа (свободен текст)")]
        public string StartDate { get; set; }

        [Required]
        [Display(Name = "Офертата Ви е валидна до:")]
        public DateTime ExpirationDate { get; set; }

        public string SpecialistDetailsId { get; set; }

        public virtual Specialist_Details SpecialistDetails { get; set; }
    }
}
