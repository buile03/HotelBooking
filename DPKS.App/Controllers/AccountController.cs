using DPKS.Data.Entites;
using DPKS.Model.User;
using DPKS.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static DPKS.Model.User.PasswordVm;
using Microsoft.AspNetCore.Identity;
using Stripe;

namespace DPKS.APP.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDanhMucService _danhmucService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(IUserService userService
            , IDanhMucService danhMucService
            , UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _danhmucService = danhMucService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Trang đăng nhập
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.DangNhap(model.UserName, model.Password, model.RememberMe);
            if (result.Succeeded)
            {
                var user = await _userService.GetByUserName(model.UserName); // Lấy thông tin user

                // Tạo danh sách claims
                var claims = new List<Claim>
{
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    
                    // Nếu bạn có role thì thêm:
                    // new Claim(ClaimTypes.Role, "Admin") // hoặc lấy từ Identity Role
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                // Tạo identity và principal
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Đăng nhập và tạo cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(model);
        }

        // Login bằng Google
        [HttpGet]
        public IActionResult LoginWithGoogle(string returnUrl = "/")
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        // Login bằng Facebook
        [HttpGet]
        public IActionResult LoginWithFacebook(string returnUrl = "/")
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return Challenge(properties, "Facebook");
        }
        
        // Xử lý sau khi xác thực từ gg/fb
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
                return LocalRedirect(returnUrl);

            // Nếu chưa có tài khoản, tạo mới
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                QuocGiaId = 242,
                TinhId = 5,
                IsActive = true
            };

            var identityResult = await _userManager.CreateAsync(user);
            if (identityResult.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in identityResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View("Login");
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new RegisterVm
            {
                DanhSachQuocGia = await _danhmucService.GetDanhSachQuocGiaAsync()
            };

            if (model.DanhSachQuocGia.Any())
            {
                var quocGiaId = int.Parse(model.DanhSachQuocGia.First().Value);
                model.DanhSachTinh = await _danhmucService.GetDanhSachTinhTheoQuocGiaAsync(quocGiaId);
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


        [HttpPost]
        public async Task<IActionResult> GuiMaXacNhan(string email)
        {
            var success = await _userService.GuiMaXacNhanEmail(email);
            if (!success)
                return BadRequest("Email không tồn tại hoặc lỗi gửi email.");

            return Ok("Đã gửi mã xác nhận thành công.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _userService.QuenMatKhau(model.Email);
            if (success)
            {
                ViewBag.Message = "Đã gửi mã khôi phục đến email.";
                return View();
            }

            ModelState.AddModelError("", "Không tìm thấy tài khoản với email này.");
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _userService.DatLaiMatKhau(model.Email, model.ResetCode, model.NewPassword);
            if (success)
                return RedirectToAction("Login");

            ModelState.AddModelError("", "Mã đặt lại không hợp lệ hoặc đã hết hạn.");
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect đến login thay vì Profile
            }

            var userId = int.Parse(userIdClaim.Value);
            var success = await _userService.DoiMatKhau(userId, model.CurrentPassword, model.NewPassword);

            if (success)
            {
                ViewBag.Message = "Đổi mật khẩu thành công.";
                return View();
            }

            ModelState.AddModelError("", "Mật khẩu hiện tại không đúng.");
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect đến login thay vì Profile
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = await _userService.GetById(userId);
            if (user == null)
                return NotFound();

            // Load data cho dropdown
            ViewBag.QuocGiaList = await _danhmucService.GetDanhSachQuocGiaAsync();
            ViewBag.TinhList = await _danhmucService.GetDanhSachTinhTheoQuocGiaAsync(user.QuocGiaId);

            var model = new UpdateProfileVm
            {
                UserId = user.Id,
                NgaySinh = user.NgaySinh,
                GioiTinh =user.GioiTinh,
                FullName = user.HoTen,
                UserName = user.UserName,
                QuocGiaId = user.QuocGiaId,
                TinhId = user.TinhId,
                PhotoName = user.PhotoName,
                Email = user.Email
                
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(UpdateProfileVm model, IFormFile? avatarFile)
        {
            if (!ModelState.IsValid)
            {
                // Reload dropdown data khi có lỗi
                ViewBag.QuocGiaList = await _danhmucService.GetDanhSachQuocGiaAsync();
                ViewBag.TinhList = await _danhmucService.GetDanhSachTinhTheoQuocGiaAsync(model.QuocGiaId);
                return View(model);
            }

            try
            {
                var user = await _userService.GetById(model.UserId);
                if (user == null)
                    return NotFound();

                //// Xử lý upload avatar nếu có
                if (avatarFile != null && avatarFile.Length > 0)
                {
                    var fileName = await _userService.LuuAnhDaiDien(avatarFile, model.UserId);
                    model.PhotoName = fileName;

                    // Cập nhật lại vào ApplicationUser
                    await _userService.CapNhatAnhDaiDien(model.UserId, fileName);
                }
                user.HoTen = model.FullName;
                user.UserName = model.UserName;
                user.QuocGiaId = model.QuocGiaId;
                user.TinhId = model.TinhId;
                user.PhotoName = model.PhotoName;
                user.NgaySinh = model.NgaySinh;
                user.GioiTinh = model.GioiTinh;
                var success = await _userService.Update(user);
                if (success)
                {
                    ViewBag.Message = "Cập nhật thông tin thành công!";
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công.");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin.");
                ViewBag.QuocGiaList = await _danhmucService.GetDanhSachQuocGiaAsync();
                ViewBag.TinhList = await _danhmucService.GetDanhSachTinhTheoQuocGiaAsync(model.QuocGiaId);
                return View(model);
            }
        }
        // Lỗi quyền
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }



    }
}
