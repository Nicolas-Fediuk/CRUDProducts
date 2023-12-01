using CRUDProducts.Services;
using CRUDProducts.UI.Encript;
using CRUDProducts.UI.Models.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection.Metadata.Ecma335;

namespace CRUDProducts.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IData data;

        public LoginController(IData data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Users user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.PASSWORD_USR = Encrypt.GetSHA256(user.PASSWORD_USR).ToString();

            bool ExistMail = await data.CheckMail(user);

            if (!ExistMail)
            {
                ModelState.AddModelError(nameof(user.MAIL_USR), $"The mail {user.MAIL_USR} doesn't exist");
                return View(user);
            }

            bool CkeckPass = await data.CheckPass(user);

            if (!CkeckPass)
            {
                ModelState.AddModelError(nameof(user.PASSWORD_USR), $"The password is invalid");
                return View(user);
            }

            Response.Cookies.Append("mailUser", user.MAIL_USR, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                HttpOnly = true, 
            });

            return RedirectToAction("Index", "Products");
            
        }

        public IActionResult NewUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewUser(Users user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            bool ExistMail = await data.CheckMail(user);

            if (ExistMail)
            {
                ModelState.AddModelError(nameof(user.MAIL_USR), $"The mail {user.MAIL_USR} already exist");
                return View(user);
            }
            else
            {
                user.PASSWORD_USR = Encrypt.GetSHA256(user.PASSWORD_USR);
                await data.AddNewUser(user);
            }

            return RedirectToAction("Index", "Login");
        }
    }
}
