using DPKS.Model.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DPKS.Service;
using DPKS.Data.Entites;

namespace DPKS.Admin.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDanhMucService _danhMucService;
        public AccountController(IUserService userService, IDanhMucService danhMucService)
        {
            _userService = userService;
            _danhMucService = danhMucService;
        }


        
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.DangNhap(model.UserName, model.Password, model.RememberMe);
            if (result.Succeeded)
            {
                var user = await _userService.GetByUserName(model.UserName); // Lấy thông tin user
                var roles = await _userService.GetRoles(user);

                if (!roles.Contains("Admin"))
                {
                    // Nếu không phải ADMIN thì hủy đăng nhập
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    ModelState.AddModelError("", "Tài khoản không có quyền truy cập trang quản trị.");
                    return View(model);
                }

                // Tạo danh sách claims
                var claims = new List<Claim>
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("PhotoName", user.PhotoName ?? "user.png"),
                    new Claim(ClaimTypes.Role, "Admin"), // Đúng tên role trong DB
                    new Claim("FullName", user.HoTen ?? "") // lưu họ tên 
                };

                // Thêm tất cả roles vào claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }   

                // Tạo identity và principal
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                
                // Đăng nhập và tạo cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterVm
            {
                DanhSachQuocGia = await _danhMucService.GetDanhSachQuocGiaAsync()
            };

            if (model.DanhSachQuocGia.Any())
            {
                var quocGiaId = int.Parse(model.DanhSachQuocGia.First().Value);
                model.DanhSachTinh = await _danhMucService.GetDanhSachTinhTheoQuocGiaAsync(quocGiaId);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                QuocGiaId = 242,
                TinhId = 5,
                IsActive = true
            };

            var result = await _userService.DangKy(user, model.Password);

            if (result.Succeeded)
                return RedirectToAction("Login");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);


            return View(model);
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
