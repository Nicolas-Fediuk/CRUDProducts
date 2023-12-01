using System.ComponentModel.DataAnnotations;
using CRUDProducts.UI.Validaciones;

namespace CRUDProducts.UI.Models.Entitys
{
    public class Product
    {
        [Required(ErrorMessage = "El campo name es requerido")]
        public string NAME_PRO { get; set; }    

        public string USER_PRO { get; set; }

        [Required(ErrorMessage = "El campo stock es requerido")]
        [GreaterThanZero]
        public int STOCK_PRO { get; set; }

        [Required(ErrorMessage = "El campo value es requerido")]
        [GreaterThanZero]
        public decimal VALUE_PRO { get; set; }

        public int ORDER_PRO { get; set; }      
    }
}
