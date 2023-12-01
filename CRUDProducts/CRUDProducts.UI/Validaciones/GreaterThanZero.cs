using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CRUDProducts.UI.Validaciones
{
    public class GreaterThanZero : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            if (int.Parse(value.ToString()) < 0)
            {
                return new ValidationResult("Enter a number greater than 0");
            }

            return ValidationResult.Success;
        }
    }
}
