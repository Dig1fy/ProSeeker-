namespace ProSeeker.Common.CustomValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CustomFutureDateTimeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is DateTime))
            {
                return new ValidationResult("Невалидна дата! Моля, посочете попълнете датата в предоставения формат!");
            }

            DateTime result;
            DateTime.TryParse(value.ToString(), out result);

            if (result < DateTime.UtcNow)
            {
                return new ValidationResult("Невалидна дата! Моля, посочете бъдеща дата за валидност на офертата Ви!");
            }

            return ValidationResult.Success;
        }
    }

}
