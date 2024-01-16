using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using CRUDProducts.UI.Validaciones;

namespace CRUDProducts.UI.Models.Entitys
{
    public class Users
    {
        [Required(ErrorMessage = "The email es required")]
        [CheckMail]
        public string MAIL_USR { get; set; }

        [Required(ErrorMessage = "The password es required")]
        public string PASSWORD_USR { get; set; }

    }
}
