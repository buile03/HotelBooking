using DPKS.Common;
using DPKS.Common.Enum;
using DPKS.Common.Helper;
using DPKS.Common.Result;
using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Model.LoaiPhong;
using DPKS.Model.LoaiPhong.Request;
using DPKS.Model.TienNghi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface ILoaiPhongService
    {
        Task<PagedResult<ThongTinLoaiPhongVm>> GetPaging(LoaiPhongSearchRequest request);
        Task<Result<List<ThongTinLoaiPhongVm>>> GetAllLoaiPhong();
        Task<Result<LoaiPhongDetailVm>> GetPhongById(int loaiPhongId);
        Task<List<SelectListItem>> GetAllForDropdown();

        //ADMIN
        Task<Result<int>> Create(LoaiPhongCreateRequest request);
        Task<Result<int>> Update(LoaiPhongUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<LoaiPhongUpdateRequest> GetById(int id);
        Task<List<AnhLoaiPhongVm>> GetAnhLoaiPhong(int loaiPhongId);
        Task<Result<bool>> SetHinhAnhChinh(int loaiPhongId, int photoId, int userId);
        Task<Result<bool>> AddAnhLoaiPhong(int loaiPhongId, string ImageFile, int userId);
        Task<Result<bool>> DeleteAnhLoaiPhong(int photoId, int userId);
    }
    public class LoaiPhongService : BaseService, ILoaiPhongService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LoaiPhongService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string _imageDirectory = "images/loaiphong";
        public LoaiPhongService(AppDbContext context, IStorageService storageService, ILogger<LoaiPhongService> logger, IWebHostEnvironment webHostEnvironment) : base(context, storageService)
        {
            _context = context;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }


        #region KHÁCH HÀNG

        public async Task<PagedResult<ThongTinLoaiPhongVm>> GetPaging(LoaiPhongSearchRequest request)
        {
            try
            {
                var query = _context.LoaiPhongs
                    .Where(lp => lp.IsActive && !lp.IsDeleted)
                    .Include(lp => lp.anhLoaiPhongs)
                    .Include(lp => lp.tienNghis)
                    .Include(lp => lp.phongs.Where(p => p.IsActive && !p.IsDeleted))
                    .AsNoTracking()
                    .AsQueryable();

                // Lọc theo từ khóa
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    string keyword = $"%{request.Keyword.Trim()}%";
                    query = query.Where(lp =>
                        EF.Functions.Like(lp.Type, keyword) ||
                        (lp.Description != null && EF.Functions.Like(lp.Description, keyword))
                    );
                }

                int totalRecords = await query.CountAsync();

                var data = await query
                    .OrderBy(lp => lp.Id)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(lp => new ThongTinLoaiPhongVm
                    {
                        Id = lp.Id,
                        Type = lp.Type,
                        Description = lp.Description,
                        DienTich = lp.DienTich,
                        HinhAnhChinh = lp.HinhAnhChinh,
                        TongAnh = lp.anhLoaiPhongs.Count,
                        TongTienNghi = lp.tienNghis.Count,
                        SoLuongPhong = lp.phongs.Count
                    })
                    .ToListAsync();

                return new PagedResult<ThongTinLoaiPhongVm>
                {
                    TotalRecords = totalRecords,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách loại phòng: " + ex.Message, ex);
            }
        }

        public async Task<List<SelectListItem>> GetAllForDropdown()
        {
            return await _context.LoaiPhongs
                //.Where(x => !x.IsDeleted)
                .OrderBy(x => x.Type)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),  
                    Text = x.Type
                }).ToListAsync();
        }
        public async Task<Result<List<ThongTinLoaiPhongVm>>> GetAllLoaiPhong()
        {
            var loaiPhongs = await _context.LoaiPhongs
                .Include(lp => lp.phongs)
                    .ThenInclude(p => p.TrangThaiPhong)
                .Include(lp => lp.phongs)
                    .ThenInclude(p => p.anhPhongs)
                .Include(lp => lp.tienNghiTheoLoaiPhongs)
                    .ThenInclude(tn => tn.TienNghi)
                .Include(lp => lp.anhLoaiPhongs)
                .ToListAsync();

            if (loaiPhongs == null || !loaiPhongs.Any())
                return Result<List<ThongTinLoaiPhongVm>>.Error("Không có loại phòng để hiển thị");

            var result = loaiPhongs.Select(lp => new ThongTinLoaiPhongVm
            {
                Id = lp.Id,
                Type = lp.Type,
                Description = lp.Description,
                SoLuongPhong = lp.phongs.Count,
                SoLuongPhongTrong = lp.phongs.Count(p => p.TrangThaiPhongId == (int)enTrangThaiPhong.TRONG),
                GiaThapNhat = lp.phongs.Min(p => p.Gia),
                GiaCaoNhat = lp.phongs.Max(p => p.Gia),
                LoaiGiuong = lp.phongs.Select(p => p.loaiGiuong).Distinct().ToList(),
                LoaiView = lp.phongs.Select(p => p.loaiView).Distinct().ToList(),
                TienNghis = lp.tienNghiTheoLoaiPhongs
                    .Select(t => new TienNghiVm
                    {
                        Name = t.TienNghi.Name,
                        Icon = t.TienNghi.Icon
                    }).ToList(),
                HinhAnhChinh = lp.HinhAnhChinh,
                DanhSachHinhAnh = lp.anhLoaiPhongs.Select(a => a.PhotoName).ToList(),

                ThongTinPhongs = lp.phongs.Select(p => new ThongTinPhongVm
                {
                    PhongId = p.PhongId,
                    SoPhong = p.SoPhong,
                    Type = lp.Type,
                    Gia = p.Gia,
                    LoaiGiuong = p.loaiGiuong,
                    LoaiView = p.loaiView,
                    TrangThaiPhong = p.TrangThaiPhong != null
                        ? p.TrangThaiPhong.trangThaiPhong
                        : enTrangThaiPhong.KHONGKHADUNG,
                    SoNguoiLonToiDa = p.SoNguoiLonToiDa,
                    SoTreEmToiDa = p.SoTreEmToiDa,
                    AnhPhong = p.anhPhongs.Select(a => a.PhotoName).ToList(),
                    LoaiPhong = new List<string> { lp.Type }
                    // SoNguoiToiDa được tính tự động trong model
                }).ToList(),
                DienTich = lp.DienTich

            }).ToList();

            return Result<List<ThongTinLoaiPhongVm>>.Success($"Hiển thị {result.Count} loại phòng", result);
        }


        public async Task<Result<LoaiPhongDetailVm?>> GetPhongById(int loaiPhongId)
        {
            var loaiPhong = await _context.LoaiPhongs
            .Include(lp => lp.phongs)
                .ThenInclude(p => p.anhPhongs)
            .Include(lp => lp.tienNghiTheoLoaiPhongs)
                .ThenInclude(tn => tn.TienNghi)
            .FirstOrDefaultAsync(lp => lp.Id == loaiPhongId);

            if (loaiPhong == null)
                return Result<LoaiPhongDetailVm>.Error("Không tìm thấy loại phòng");

            var loaiPhongVm = new ThongTinLoaiPhongVm
            {
                Id = loaiPhong.Id,
                Type = loaiPhong.Type,
                Description = loaiPhong.Description,
                SoLuongPhong = loaiPhong.phongs.Count,
                SoLuongPhongTrong = loaiPhong.phongs.Count(p => p.TrangThaiPhongId == (int)enTrangThaiPhong.TRONG),
                GiaThapNhat = loaiPhong.phongs.Any() ? loaiPhong.phongs.Min(p => p.Gia) : 0,
                GiaCaoNhat = loaiPhong.phongs.Any() ? loaiPhong.phongs.Max(p => p.Gia) : 0,
                LoaiGiuong = loaiPhong.phongs.Select(p => p.loaiGiuong).Distinct().ToList(),
                LoaiView = loaiPhong.phongs.Select(p => p.loaiView).Distinct().ToList(),
                TienNghis = loaiPhong.tienNghiTheoLoaiPhongs
                .Select(t => new TienNghiVm
                {
                    Name = t.TienNghi.Name,
                    Icon = t.TienNghi.Icon
                })
                .ToList()
            };

            var listPhong = loaiPhong.phongs.Select(p => new ThongTinPhongVm
            {
                PhongId = p.PhongId,
                SoPhong = p.SoPhong,
                Gia = p.Gia,
                LoaiGiuong = p.loaiGiuong,
                LoaiView = p.loaiView,
                TrangThaiPhong = (enTrangThaiPhong)p.TrangThaiPhongId,
                Type = loaiPhong.Type,
                AnhPhong = p.anhPhongs.Select(a => a.PhotoName).ToList(),
                LoaiPhong = new List<string> { loaiPhong.Type },
                TienNghis = loaiPhong.tienNghiTheoLoaiPhongs.Select(t => t.TienNghi.Name).ToList()
            }).ToList();

            var detailVm = new LoaiPhongDetailVm
            {
                LoaiPhong = loaiPhongVm,
                Phongs = listPhong
            };

            return Result<LoaiPhongDetailVm>.Success("Lấy thông tin phòng thành công", detailVm);
        }

       
        #endregion



        #region ADMIN
        public async Task<Result<int>> Create(LoaiPhongCreateRequest request)
        {
            try
            {
                _action = $"Thêm loại phòng thành công";
                if (await _context.LoaiPhongs.AnyAsync(x => x.Type == request.Type))
                    return Result<int>.Error($"Loại phòng {request.Type} đã tồn tại!");

                var obj = new LoaiPhong()
                {
                    Type = request.Type,
                    Description = request.Description,
                    DienTich = request.DienTich,
                    HinhAnhChinh = request.HinhAnhChinh,
                    IsActive = true,
                    CreateAt = DateTime.Now,
                    CreateBy = "admin",
                    ModifiedBy = "admin",
                    LateModifiedDate = DateTime.Now,
                    IsDeleted = false
                };

                _context.LoaiPhongs.Add(obj);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                    return Result<int>.Success(_action, obj.Id);

                return Result<int>.Error("Cập nhật thất bại");

            }
            catch (Exception ex)
            {
                
                return Result<int>.Error("Lỗi khi thêm phòng: " + ex.Message);
            }
        }
        public async Task<Result<int>> Update(LoaiPhongUpdateRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id))
                    return Result<int>.Error("ID không hợp lệ!");

                int id = request.Id.DecodeId();

                var entity = await _context.LoaiPhongs.FindAsync(id);
                if (entity == null)
                    return Result<int>.Error("Không tìm thấy loại phòng!");

                // Check trùng Type
                bool isDuplicate = await _context.LoaiPhongs
                    .AnyAsync(x => x.Type == request.Type && x.Id != id && !x.IsDeleted);
                if (isDuplicate)
                    return Result<int>.Error("Tên loại phòng đã tồn tại!");
                
                entity.Type = request.Type;
                entity.Description = request.Description;
                entity.DienTich = request.DienTich ?? 0;
                entity.ModifiedBy = request.UserId.ToString();
                entity.LateModifiedDate = DateTime.Now;

                if (request.HinhAnhChinhFile != null)
                {
                    var uploadResult = await UploadImageAsync(request.HinhAnhChinhFile);
                    if (!uploadResult.IsSuccessed)
                        return Result<int>.Error(uploadResult.Message);

                    // Delete old main image
                    if (!string.IsNullOrEmpty(entity.HinhAnhChinh))
                    {
                        DeleteImageFile(entity.HinhAnhChinh);
                    }

                    entity.HinhAnhChinh = uploadResult.ResultObj;
                }



                _context.LoaiPhongs.Update(entity);
                var result = await SaveChange();

                return result > 0
                    ? Result<int>.Success("Cập nhật loại phòng thành công", id)
                    : Result<int>.Error("Cập nhật thất bại!");
            }
            catch (Exception ex)
            {
                return Result<int>.Error("Lỗi hệ thống khi cập nhật loại phòng!");
            }
        }

        public async Task<List<AnhLoaiPhongVm>> GetAnhLoaiPhong(int loaiPhongId)
        {
            try
            {
                var loaiPhong = await _context.LoaiPhongs.FindAsync(loaiPhongId);

                return await _context.AnhLoaiPhongs
                    .Where(x => x.LoaiPhongId == loaiPhongId && !x.IsDeleted)
                    .OrderByDescending(x => x.CreateAt)
                    .Select(x => new AnhLoaiPhongVm
                    {
                        PhotoId = x.PhotoId,
                        PhotoName = x.PhotoName,
                        IsMainPhoto = loaiPhong != null && loaiPhong.HinhAnhChinh == x.PhotoName,
                        CreatedDate = x.CreateAt
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting AnhLoaiPhong for LoaiPhongId: {LoaiPhongId}", loaiPhongId);
                return new List<AnhLoaiPhongVm>();
            }
        }
        public async Task<LoaiPhongUpdateRequest> GetById(int id)
        {
            try
            {
                var lp = await _context.LoaiPhongs.FindAsync(id);
                if (lp == null) return null;

                return new LoaiPhongUpdateRequest
                {
                    Id = lp.Id.EncodeId1(),
                    Type = lp.Type,
                    Description = lp.Description,
                    DienTich = lp.DienTich,
                    HinhAnhChinh = lp.HinhAnhChinh
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting LoaiPhong by ID: {Id}", id);
                return null;
            }
        }
        //public async Task<Result<bool>> AddAnhLoaiPhong(int loaiPhongId, string imageFile, int userId)
        //{
        //    try
        //    {
        //        // Validate input
        //        var validationResult = ValidateImageFile(imageFile);
        //        if (!validationResult.IsSuccessed)
        //            return validationResult;

        //        // Check if LoaiPhong exists
        //        var loaiPhong = await _context.LoaiPhongs.FindAsync(loaiPhongId);
        //        if (loaiPhong == null)
        //            return Result<bool>.Error("Không tìm thấy loại phòng!");

        //        // Upload image
        //        var uploadResult = await UploadImageAsync(imageFile);
        //        if (!uploadResult.IsSuccessed)
        //            return Result<bool>.Error(uploadResult.Message);

        //        // Save to database
        //        var anhLoaiPhong = new AnhLoaiPhong
        //        {
        //            LoaiPhongId = loaiPhongId,
        //            PhotoName = uploadResult.ResultObj,
        //            CreateAt = DateTime.Now,
        //            CreateBy = userId.ToString(),
        //            IsActive = true,
        //            IsDeleted = false
        //        };

        //        _context.AnhLoaiPhongs.Add(anhLoaiPhong);
        //        var result = await SaveChangeAsync();

        //        return result > 0
        //            ? Result<bool>.Success("Thêm ảnh thành công!")
        //            : Result<bool>.Error("Thêm ảnh thất bại!");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error adding AnhLoaiPhong for LoaiPhongId: {LoaiPhongId}", loaiPhongId);
        //        return Result<bool>.Error("Lỗi hệ thống khi thêm ảnh!");
        //    }
        //}
        public async Task<Result<bool>> AddAnhLoaiPhong(int loaiPhongId, string ImageFile, int userId)
        {
            try
            {
                // Kiểm tra loại phòng có tồn tại không
                var loaiPhong = await _context.LoaiPhongs.FindAsync(loaiPhongId);
                if (loaiPhong == null)
                    return Result<bool>.Error("Không tìm thấy loại phòng!");

                // Tạo bản ghi ảnh phụ
                var anhLoaiPhong = new AnhLoaiPhong
                {
                    LoaiPhongId = loaiPhongId,
                    PhotoName = ImageFile,
                    CreateAt = DateTime.Now,
                    CreateBy = userId.ToString(),
                    IsActive = true,
                    IsDeleted = false
                };

                _context.AnhLoaiPhongs.Add(anhLoaiPhong);
                var result = await SaveChangeAsync();

                return result > 0
                    ? Result<bool>.Success("Thêm ảnh thành công!")
                    : Result<bool>.Error("Thêm ảnh thất bại!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm ảnh phụ cho LoaiPhongId: {LoaiPhongId}", loaiPhongId);
                return Result<bool>.Error("Lỗi hệ thống khi thêm ảnh!");
            }
        }

        public async Task<Result<bool>> DeleteAnhLoaiPhong(int photoId, int userId)
        {
            try
            {
                var photo = await _context.AnhLoaiPhongs.FindAsync(photoId);
                if (photo == null || photo.IsDeleted)
                    return Result<bool>.Error("Không tìm thấy ảnh!");

                // Check if this is the main photo
                var loaiPhong = await _context.LoaiPhongs.FindAsync(photo.LoaiPhongId);
                if (loaiPhong != null && loaiPhong.HinhAnhChinh == photo.PhotoName)
                {
                    // Clear main photo reference
                    loaiPhong.HinhAnhChinh = null;
                    loaiPhong.ModifiedBy = userId.ToString();
                    loaiPhong.LateModifiedDate = DateTime.Now;
                    _context.LoaiPhongs.Update(loaiPhong);
                }

                // Delete physical file
                DeleteImageFile(photo.PhotoName);

                // Soft delete
                photo.IsDeleted = true;
                photo.ModifiedBy = userId.ToString();
                photo.LateModifiedDate = DateTime.Now;

                _context.AnhLoaiPhongs.Update(photo);
                var result = await SaveChangeAsync();

                return result > 0
                    ? Result<bool>.Success("Xóa ảnh thành công!")
                    : Result<bool>.Error("Xóa ảnh thất bại!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting AnhLoaiPhong with PhotoId: {PhotoId}", photoId);
                return Result<bool>.Error("Lỗi hệ thống khi xóa ảnh!");
            }
        }

        public async Task<Result<bool>> SetHinhAnhChinh(int loaiPhongId, int photoId, int userId)
        {
            try
            {
                var loaiPhong = await _context.LoaiPhongs.FindAsync(loaiPhongId);
                if (loaiPhong == null)
                    return Result<bool>.Error("Không tìm thấy loại phòng!");

                var photo = await _context.AnhLoaiPhongs
                    .FirstOrDefaultAsync(x => x.PhotoId == photoId && x.LoaiPhongId == loaiPhongId && !x.IsDeleted);

                if (photo == null)
                    return Result<bool>.Error("Không tìm thấy ảnh!");

                // Delete old main image if it's different and not in gallery
                if (!string.IsNullOrEmpty(loaiPhong.HinhAnhChinh) && loaiPhong.HinhAnhChinh != photo.PhotoName)
                {
                    var isInGallery = await _context.AnhLoaiPhongs
                        .AnyAsync(x => x.PhotoName == loaiPhong.HinhAnhChinh && x.LoaiPhongId == loaiPhongId && !x.IsDeleted);

                    if (!isInGallery)
                    {
                        DeleteImageFile(loaiPhong.HinhAnhChinh);
                    }
                }

                loaiPhong.HinhAnhChinh = photo.PhotoName;
                loaiPhong.ModifiedBy = userId.ToString();
                loaiPhong.LateModifiedDate = DateTime.Now;

                _context.LoaiPhongs.Update(loaiPhong);
                var result = await SaveChangeAsync();

                return result > 0
                    ? Result<bool>.Success("Đặt ảnh chính thành công!")
                    : Result<bool>.Error("Đặt ảnh chính thất bại!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting main photo for LoaiPhongId: {LoaiPhongId}, PhotoId: {PhotoId}", loaiPhongId, photoId);
                return Result<bool>.Error("Lỗi hệ thống khi đặt ảnh chính!");
            }
        }
        #endregion

        private Result<bool> ValidateImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Result<bool>.Error("File ảnh không hợp lệ!");

            var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
                return Result<bool>.Error("Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .gif)!");

            if (file.Length > _maxFileSize)
                return Result<bool>.Error("File ảnh không được vượt quá 5MB!");

            return Result<bool>.Success("File hợp lệ");
        }

        private async Task<Result<string>> UploadImageAsync(IFormFile file)
        {
            try
            {
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, _imageDirectory);

                // Create directory if not exists
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Result<string>.Success("Upload thành công", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image file: {FileName}", file.FileName);
                return Result<string>.Error("Lỗi khi upload file!");
            }
        }

        private void DeleteImageFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            try
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, _imageDirectory, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deleting image file: {FileName}", fileName);
                // Don't throw exception for file deletion errors
            }
        }

        private async Task<int> SaveChangeAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving changes to database");
                throw;
            }
        }

        public async Task<Result<int>> Delete(DeleteRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                var obj = await _context.LoaiPhongs.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error("Không tìm thấy phòng cần xóa!");

                obj.IsDeleted = true;

                obj.ModifiedBy = request.UserId.ToString();
                obj.LateModifiedDate = DateTime.Now;

                _context.LoaiPhongs.Update(obj);
                var result = await SaveChange();

                return result > 0
                    ? Result<int>.Success("Xóa phòng thành công", id)
                    : Result<int>.Error("Xóa thất bại!");
            }
            catch
            {

                return Result<int>.Error("Đã xảy ra lỗi khi xóa phòng.");
            }
        }
    }


}
