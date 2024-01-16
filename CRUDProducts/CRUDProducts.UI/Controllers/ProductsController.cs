using CRUDProducts.Services;
using CRUDProducts.UI.Models.Entitys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection;

namespace CRUDProducts.UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IData data;
        public ProductsController(IData data)
        {
            this.data = data;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await data.LoadProducts();
            return View(products);
        }

        [HttpGet]
        public IActionResult NewProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewProduct(Product product)
        {

            product.USER_PRO = Request.Cookies["mailUser"].ToString();

            product.ORDER_PRO = await data.MaxOrden();

            if (await data.checkProductExist(product.NAME_PRO))
            {
                return Json(new { success = false});
            }

            await data.AddNewProduct(product);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(string name)
        {
            var product = await data.GetProduct(name);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {

            product.USER_PRO = Request.Cookies["mailUser"].ToString();

            product.ORDER_PRO = await data.MaxOrden();

            ModelState.Clear();

            TryValidateModel(product);

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            await data.EditProductExist(product);

            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct([FromBody] string name)
        {
            try
            {
                await data.Delete(name);

                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false });
            }

        }
    }
}
