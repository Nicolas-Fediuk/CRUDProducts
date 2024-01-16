using CRUDProducts.Services;
using CRUDProducts.UI.Encript;
using CRUDProducts.UI.Models.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

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

        public IActionResult Load()
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
        public async Task<IActionResult> NewUser(Users user, string password)
        {

            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(password))
                {
                    ViewBag.confirmPassword = "Confirm passwords is required";
                    return View(user);
                }

                return View(user);
            }

            bool ExistMail = await data.CheckMail(user);

            if (ExistMail)
            {
                ModelState.AddModelError(nameof(user.MAIL_USR), $"The mail {user.MAIL_USR} already exist");
                return View(user);
            }

            if (!OkPassword(user.PASSWORD_USR))
            {
                ModelState.AddModelError(nameof(user.PASSWORD_USR), "The password must contain: 8 characters, a capital letter, a numerical digit and a special character");
                return View(user);
            }

            if (user.PASSWORD_USR != password)
            {
                ViewBag.confirmPassword = "Passwords do not match";
                return View(user);
            }
            else
            {
                user.PASSWORD_USR = Encrypt.GetSHA256(user.PASSWORD_USR);
                await data.AddNewUser(user);
            }

            return RedirectToAction("Index", "Login");
        }

        private bool OkPassword(string password)
        {

            if (password.Length > 8 &&
               password.Any(char.IsUpper) &&
               password.Any(char.IsDigit) &&
               Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
            {
                return true;
            }

            return false;
        }
    }
}
