using DPKS.Data.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface IUserService
    {
        Task<IdentityResult> DangKy(ApplicationUser user, string password, string role);
        Task<SignInResult> DangNhap(string usernameOrEmail, string password, bool rememberMe);
        Task<bool> GuiMaXacNhanEmail(string email);
        Task<bool> XacNhanEmail(string userId, string confirmationCode);
        Task<ApplicationUser> GetById(int id);
        Task<bool> AssignRole(ApplicationUser user, string role);
        Task Logout();

        Task<ApplicationUser> GetByUserName(string username);

        Task<bool> QuenMatKhau(string email);
        Task<bool> DatLaiMatKhau(string email, string resetCode, string newPassword);
        Task<bool> DoiMatKhau(int userId, string currentPassword, string newPassword);

        Task<bool> CapNhatAnhDaiDien(int userId, string photoName);
        Task<string> LayAnhDaiDien(int userId);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSenderService _emailSender;
        private readonly IDanhMucService _danhmucService;
        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IEmailSenderService emailSender,
            IDanhMucService danhmucService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _danhmucService = danhmucService;
        }

        #region Đăng ký & Đăng nhập

        public async Task<IdentityResult> DangKy(ApplicationUser user, string password, string role)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true; 

            // user.EmailConfirmationCode = GenerateRandomCode();
            // user.ConfirmationCodeExpiry = DateTime.UtcNow.AddHours(24);

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
                await AssignRole(user, role);

            return result;
        }

        public async Task<SignInResult> DangNhap(string usernameOrEmail, string password, bool rememberMe)
        {
            var user = await _userManager.FindByNameAsync(usernameOrEmail)
                       ?? await _userManager.FindByEmailAsync(usernameOrEmail);

            if (user == null || !user.IsActive)
                return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUser> GetByUserName(string username)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> XacNhanEmail(string userId, string confirmationCode)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.EmailConfirmationCode != confirmationCode ||
                user.ConfirmationCodeExpiry < DateTime.UtcNow)
                return false;

            user.EmailConfirmed = true;
            user.IsActive = true;
            user.EmailConfirmationCode = null;
            user.ConfirmationCodeExpiry = null;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        public async Task<bool> GuiMaXacNhanEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            user.EmailConfirmationCode = GenerateRandomCode();
            user.ConfirmationCodeExpiry = DateTime.UtcNow.AddHours(24);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return false;

            string subject = "Xác nhận email của bạn";
            string body = $@"
                        <p>Xin chào {user.UserName},</p>
                        <p>Mã xác nhận email của bạn là: <strong>{user.EmailConfirmationCode}</strong></p>
                        <p>Mã có hiệu lực trong 24 giờ.</p>";

            await _emailSender.SendEmailAsync(user.Email, subject, body);

            return true;
        }


        #endregion

        #region Thông tin người dùng & ảnh đại diện

        public async Task<ApplicationUser> GetById(int id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> CapNhatAnhDaiDien(int userId, string photoName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            user.PhotoName = photoName;
            user.LastModifiedDate = DateTime.UtcNow;

            return (await _userManager.UpdateAsync(user)).Succeeded;
        }

        public async Task<string> LayAnhDaiDien(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return user?.PhotoName;
        }

        #endregion

        #region Mật khẩu

        public async Task<bool> QuenMatKhau(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            user.ResetPasswordCode = GenerateRandomCode();
            user.ResetPasswordCodeExpiry = DateTime.UtcNow.AddHours(1);

            return (await _userManager.UpdateAsync(user)).Succeeded;
        }

        public async Task<bool> DatLaiMatKhau(string email, string resetCode, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.ResetPasswordCode != resetCode || user.ResetPasswordCodeExpiry < DateTime.UtcNow)
                return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                user.ResetPasswordCode = null;
                user.ResetPasswordCodeExpiry = null;
                await _userManager.UpdateAsync(user);
            }

            return result.Succeeded;
        }

        public async Task<bool> DoiMatKhau(int userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        #endregion

        #region Vai trò (Role)

        public async Task<bool> AssignRole(ApplicationUser user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new ApplicationRole { Name = role });

            return (await _userManager.AddToRoleAsync(user, role)).Succeeded;
        }

        #endregion

        #region Helper

        private string GenerateRandomCode(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion
    }
}
