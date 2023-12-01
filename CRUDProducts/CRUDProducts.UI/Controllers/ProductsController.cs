using CRUDProducts.Services;
using CRUDProducts.UI.Models.Entitys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            if(ModelState.IsValid)
            {
                return View(product);
            }

            var correo = Request.Cookies["mailUser"];

            product.USER_PRO = correo.ToString();

            await data.AddNewProduct(product);

            return RedirectToAction("Index", "Products");

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
            await data.EditProductExist(product);
            return RedirectToAction("Index", "Products");
        }
    }
}
