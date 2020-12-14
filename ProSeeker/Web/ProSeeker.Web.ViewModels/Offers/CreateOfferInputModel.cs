namespace ProSeeker.Web.ViewModels.Offers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Common.CustomValidationAttributes;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;

    public class CreateOfferInputModel : IMapFrom<Offer>
    {
        public string AdId { get; set; }

        [Required(ErrorMessage = "Моля, попълнете полето 'Описание на оферта'")]
        [Display(Name = "Описание / допълнителна инфомация")]
        [MinLength(25, ErrorMessage ="Описанието на офертата трябва да бъде поне 25 символа!")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Моля, попълнете офертната цена!")]
        [Display(Name = "Офертна цена /български левове/")]
        [RegularExpression(@"^[1-9][\.\d]*(,\d+)?$", ErrorMessage = "Цената трябва да бъде попълнена и да не съдържа букви. Примери за валидна цена: '2500', '1200.25', '10.55'")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Моля, попълнете, кога можете да започнете работа /в свободен текст/")]
        [Display(Name = "Кога можете да започнете работа (свободен текст)")]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "Моля, посочете, до кога е валидна офертата Ви!")]
        [Display(Name = "Офертата Ви е валидна до:")]
        [DataType(DataType.Date)]
        [CustomFutureDateTimeValidationAttribute]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = "Актуален телефонен номер:")]
        [Required(ErrorMessage ="Моля, попълнете телефонния си номер!")]
        public string PhoneNumber { get; set; }

        public string SpecialistDetailsId { get; set; }

        public virtual Specialist_Details SpecialistDetails { get; set; }
    }
}
