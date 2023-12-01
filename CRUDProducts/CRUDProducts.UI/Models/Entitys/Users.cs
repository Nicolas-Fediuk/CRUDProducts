using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using CRUDProducts.UI.Validaciones;

namespace CRUDProducts.UI.Models.Entitys
{
    public class Users
    {
        [Required(ErrorMessage = "El campo Email es requerido")]
        [CheckMail]
        public string MAIL_USR { get; set; }

        [Required(ErrorMessage = "El campo Password es requerido")]
        public string PASSWORD_USR { get; set; }    
    }
}
