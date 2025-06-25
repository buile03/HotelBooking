using DPKS.Common;
using DPKS.Common.Enum;
using DPKS.Common.Extensions;
using DPKS.Common.Result;
using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Model.Role;
using DPKS.Model.User;
using DPKS.Model.User.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface IUserService
    {

        //Admin
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
        Task<bool> LockUser(int userId); //khóa tài khoản
        Task<bool> UnlockUser(int userId); // mở tài khoản
        Task<bool> ResetPasswordByAdmin(int userId, string newPassword);

        Task<Result<int>> Create(UserCreateRequest request);
        Task<Result<int>> Update(UserUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<Result<PagedResult<DanhSachUserVm>>> GetPaging(UserSearchRequest request);
        Task<UserUpdateRequest> GetByIdAdmin(int id);



        // dùng chung 
        Task<IdentityResult> DangKy(ApplicationUser user, string password, string role);
        Task<IdentityResult> DangKy(ApplicationUser user, string password); //phương thức này dùng cho tạo tài khoản khách hàng, gán mặc định role là user
        Task<SignInResult> DangNhap(string usernameOrEmail, string password, bool rememberMe);
        Task<bool> GuiMaXacNhanEmail(string email);
        Task<bool> XacNhanEmail(string userId, string confirmationCode);
        Task<ApplicationUser> GetById(int id);
        Task<bool> AssignRole(ApplicationUser user, string role);
        Task Logout();

        Task<ApplicationUser> GetByUserName(string username);
        Task<IList<string>> GetRoles(ApplicationUser user);
        Task<bool> Update(ApplicationUser user);
        Task<bool> QuenMatKhau(string email);
        Task<bool> DatLaiMatKhau(string email, string resetCode, string newPassword);
        Task<bool> DoiMatKhau(int userId, string currentPassword, string newPassword);

        Task<bool> CapNhatAnhDaiDien(int userId, string photoName);
        Task<string> LayAnhDaiDien(int userId);
        Task<string> LuuAnhDaiDien(IFormFile avatarFile, int userId);
    }

    public class UserService : BaseService, IUserService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSenderService _emailSender;
        private readonly IDanhMucService _danhmucService;
        private readonly IStorageService _storageService;
        public UserService(
            AppDbContext context,
            IStorageService storageService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IEmailSenderService emailSender,
            IDanhMucService danhmucService) : base(context, storageService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _danhmucService = danhmucService;
            _storageService = storageService;
        }

        #region Admin
        // Lấy tất cả user
        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }
        // khoa user
        public async Task<bool> LockUser (int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            user.LockoutEnd = DateTimeOffset.MaxValue; //khoa vo thoi han
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        //mở khóa user
        public async Task<bool> UnlockUser (int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            user.LockoutEnd = null;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordByAdmin (int userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return result.Succeeded;
        }

        #endregion

        #region Đăng ký & Đăng nhập

        // Hàm gốc vẫn giữ nếu cần sử dụng cho admin
        public async Task<IdentityResult> DangKy(ApplicationUser user, string password, string role)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            if (role == enRoles.ADMIN.ToString())
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Không thể tự đăng ký quyền Admin."
                });
            }

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
                await AssignRole(user, role);

            return result;
        }

        // mặc định role là USER
        public Task<IdentityResult> DangKy(ApplicationUser user, string password)
        {
            return DangKy(user, password, enRoles.USER.ToString());
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

        public async Task<IList<string>> GetRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
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

        public async Task<bool> Update(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
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

        public async Task<string> LuuAnhDaiDien(IFormFile avatarFile, int userId)
        {
            if (avatarFile == null || avatarFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "user");

            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Tạo tên file duy nhất (vd: user_12345_timestamp.png)
            var fileExtension = Path.GetExtension(avatarFile.FileName);
            var fileName = $"user_{userId}_{DateTime.UtcNow.Ticks}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(stream);
            }

            return fileName;
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

        public async Task<Result<int>> Create(UserCreateRequest request)
        {
            try
            {
                

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    HoTen = request.HoTen,
                    PhoneNumber = request.PhoneNum,
                    GioiTinh = request.GioiTinh,
                    NgaySinh = request.Ngaysinh,
                    QuocGiaId = request.QuocGiaId,
                    TinhId = request.TinhId,
                    IsActive = true,
                    
                    CreatedAt = DateTime.UtcNow
                };
                if (await _userManager.FindByEmailAsync(request.Email) != null)
                    return Result<int>.Error("Email đã tồn tại!");

                if (request.Avatar != null)
                {
                    var extension = Path.GetExtension(request.Avatar.FileName);
                    var fileName = Guid.NewGuid().ToString() + extension;

                    using var stream = request.Avatar.OpenReadStream();
                    var savedPath = await _storageService.SaveFileAsync(stream, fileName, "uploads/user");

                    user.PhotoName = Path.GetFileName(savedPath);
                }

                var result = await _userManager.CreateAsync(user, request.Password ?? "123456Bui");

                return result.Succeeded
                    ? Result<int>.Success("Tạo người dùng thành công", user.Id)
                    : Result<int>.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<int>> Update(UserUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                var user = await _context.Users.FindAsync(id);
                if (user == null) return Result<int>.Error("Không tìm thấy người dùng!");

                user.HoTen = request.HoTen;
                user.PhoneNumber = request.PhoneNum;
                user.GioiTinh = request.GioiTinh;
                user.NgaySinh = request.Ngaysinh;
                user.QuocGiaId = request.QuocGiaId;
                user.TinhId = request.TinhId;
                
                user.ModifiedBy = request.UserId.ToString();
                user.LastModifiedDate = DateTime.UtcNow;


                if (request.Avatar != null && request.Avatar.Length > 0)
                {
                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(user.PhotoName))
                    {
                        await _storageService.DeleteFileAsync(user.PhotoName, "uploads/user");
                    }

                    var extension = Path.GetExtension(request.Avatar.FileName);
                    var fileName = $"{Guid.NewGuid()}{extension}";

                    using var stream = request.Avatar.OpenReadStream();
                    await _storageService.SaveFileAsync(stream, fileName, "uploads/user");

                    user.PhotoName = fileName;
                }

                _context.Users.Update(user);
                var result = await SaveChange();
                return result > 0
                    ? Result<int>.Success("Cập nhật người dùng thành công", user.Id)
                    : Result<int>.Error("Cập nhật thất bại");


            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> Delete(DeleteRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                var user = await _context.Users.FindAsync(id);
                if (user == null) return Result<int>.Error("Không tìm thấy người dùng!");

                user.IsDeleted = true;
                user.LastModifiedDate = DateTime.UtcNow;
                user.ModifiedBy = request.UserId.ToString();

                _context.Users.Update(user);
                var result = await SaveChange();
                return result > 0
                    ? Result<int>.Success("Xóa người dùng thành công", id)
                    : Result<int>.Error("Xóa thất bại");
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<PagedResult<DanhSachUserVm>>> GetPaging(UserSearchRequest request)
        {
            try
            {
                var query = _context.Users
                    .Where(u => !u.IsDeleted)
                    .Include(u => u.QuocGia)
                    .Include(u => u.Tinh)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    var keyword = request.Keyword.ToLower();
                    query = query.Where(u =>
                        u.HoTen.ToLower().Contains(keyword) ||
                        u.Email.ToLower().Contains(keyword));
                }

                int total = await query.CountAsync();
                var user = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(u => new DanhSachUserVm
                    {
                        Id = u.Id,
                        HoTen = u.HoTen,
                        Email = u.Email,
                        PhoneNum = u.PhoneNumber,
                        GioiTinh = u.GioiTinh,
                        NgaySinh = u.NgaySinh,
                        IsActive = u.IsActive,
                        QuocGia = u.QuocGia.Name,
                        Tinh = u.Tinh.Name
                    })
                    .ToListAsync();

                
                return Result<PagedResult<DanhSachUserVm>>.Success("Lấy danh sách vai trò thành công", new PagedResult<DanhSachUserVm>
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecords = total,
                    Items = user
                });
            }
            catch
            {
                throw;
            }
        }

        public async Task<UserUpdateRequest> GetByIdAdmin(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return null;

                return new UserUpdateRequest
                {
                    Id = user.Id.EncodeId(),
                    HoTen = user.HoTen,
                    PhoneNum = user.PhoneNumber,
                    GioiTinh = user.GioiTinh,
                    Ngaysinh = user.NgaySinh,
                    QuocGiaId = user.QuocGiaId,
                    TinhId = user.TinhId,
                    PhotoName = user.PhotoName
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
