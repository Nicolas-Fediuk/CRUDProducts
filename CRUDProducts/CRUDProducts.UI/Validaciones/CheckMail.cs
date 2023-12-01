using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CRUDProducts.UI.Validaciones
{
    public class CheckMail : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            if (!OkMail(value.ToString()))
            {
                return new ValidationResult("Mail invalido");
            }

            return ValidationResult.Success;
        }

        private bool OkMail(string mail)
        {
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(mail, patron);
        }
    }
}
