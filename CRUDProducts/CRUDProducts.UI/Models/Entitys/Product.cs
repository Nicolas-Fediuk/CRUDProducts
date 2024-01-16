using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRUDProducts.UI.Validaciones;

namespace CRUDProducts.UI.Models.Entitys
{
    public class Product
    {
        [Required(ErrorMessage = "The name es requerido")]
        public string NAME_PRO { get; set; }    

        public string USER_PRO { get; set; }

        [Required(ErrorMessage = "The stock es requerido")]
        [GreaterThanZero]
        public int STOCK_PRO { get; set; }

        [Required(ErrorMessage = "The value es requerido")]
        [RegularExpression(@"^-?\d+(\.\d+)?(,\d+)?$", ErrorMessage = "Enter only numbers")]
        public string VALUE_PRO { get; set; }

        public int ORDER_PRO { get; set; }      
    }
}
