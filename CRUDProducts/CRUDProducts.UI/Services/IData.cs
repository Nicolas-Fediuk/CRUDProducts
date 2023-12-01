using CRUDProducts.UI.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDProducts.Services
{
    public interface IData
    {
        Task AddNewProduct(Product product);
        Task AddNewUser(Users user);
        Task<bool> CheckMail(Users user);
        Task<bool> CheckPass(Users user);
        Task EditProductExist(Product product);
        Task<Product> GetProduct(string name);
        Task<IEnumerable<Product>> LoadProducts();
    }
}
