using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DPKS.Admin.Controllers
{
    public class AccountController : Controller
    {
        [Authorize(Roles = "ADMIN")]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
