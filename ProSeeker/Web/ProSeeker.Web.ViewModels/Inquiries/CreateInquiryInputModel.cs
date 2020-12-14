namespace ProSeeker.Web.ViewModels.Inquiries
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Common.CustomValidationAttributes;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Cities;

    public class CreateInquiryInputModel : IMapFrom<Inquiry>
    {
        [Required(ErrorMessage = "Моля, попълнете полето 'Подробно описание'")]
        [Display(Name = "Подробно описание")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Моля, посочете, до кога е валидно вашето запитване!")]
        [Display(Name = "Запитването Ви е валидно до:")]
        [DataType(DataType.Date)]
        [CustomFutureDateTimeValidationAttribute]
        public DateTime ValidUntil { get; set; }

        public string SpecialistDetailsId { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Моля, изберете град от падащото меню!")]
        public int CityId { get; set; }

        public IEnumerable<CitySimpleViewModel> Cities { get; set; }
    }
}
