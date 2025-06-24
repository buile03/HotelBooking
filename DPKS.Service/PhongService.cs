using DPKS.Data.EF;
using DPKS.Model.Phong;
using Microsoft.AspNetCore.Http;
using DPKS.Data.Entites;
using DPKS.Model.TienNghi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPKS.Common.Result;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DPKS.Common.Enum;
using DPKS.Common.System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DPKS.Model.Phong.Request;
using DPKS.Common;
using DPKS.Common.Helper;


namespace DPKS.Service
{
    public interface IPhongService
    {
        Task<PagedResult<ThongTinDanhSachPhongVm>> GetPagings(PhongSearchRequest request);
        Task<Result<List<ThongTinDanhSachPhongVm>>> GetAllPhongAsync(PhongSearchRequest request);
        Task<Result<ChiTietPhongVm>> GetPhongById(int Id);
        Task<Result<List<ThongTinDanhSachPhongVm>>> GetAvailablePhongsAsync(PhongSearchRequest request);
        Task<Result<decimal>> CalculateTotalPriceAsync(int phongId, DateTime ngayNhanPhong, DateTime ngayTraPhong);

        //ADMIN
        Task<Result<int>> Create(PhongCreateRequest request);
        Task<Result<int>> Update(PhongUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);

        Task<PhongUpdateRequest> GetById(int id);

    }
    public class PhongService : BaseService, IPhongService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public PhongService(AppDbContext context
            , IStorageService storageService
            , IHttpContextAccessor httpContextAccessor) : base(context, storageService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor; 
        }
        #region KHÁCH HÀNG
        public async Task<PagedResult<ThongTinDanhSachPhongVm>> GetPagings(PhongSearchRequest request)
        {
            try
            {
                var query = _context.Phongs
                    .Where(p => p.IsActive && !p.IsDeleted)
                    .Include(p => p.LoaiPhong)
                    .Include(p => p.TrangThaiPhong)
                    .Include(p => p.anhPhongs)
                    .Include(p => p.LoaiPhong).ThenInclude(lp => lp.tienNghiTheoLoaiPhongs).ThenInclude(tn => tn.TienNghi)
                    .AsQueryable();

                // lọc theo yêu cầu
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    var keyword = request.Keyword.ToLower();
                    query = query.Where(p =>
                        p.SoPhong.ToLower().Contains(keyword) ||
                        p.LoaiPhong.Type.ToLower().Contains(keyword));
                }
                ////if (request.LoaiPhongId.HasValue)
                ////    query = query.Where(p => p.LoaiPhongId == request.LoaiPhongId.Value);
                ////if (request.GiaTu.HasValue)
                ////    query = query.Where(p => p.Gia >= request.GiaTu.Value);
                ////if (request.GiaDen.HasValue)
                ////    query = query.Where(p => p.Gia <= request.GiaDen.Value);
                ////if (request.LoaiGiuong.HasValue)
                ////    query = query.Where(p => p.loaiGiuong == request.LoaiGiuong.Value);
                ////if (request.LoaiView.HasValue)
                ////    query = query.Where(p => p.loaiView == request.LoaiView.Value);
                ////if (request.SoLuongKhach.HasValue)
                ////    query = query.Where(p => p.LoaiPhong.tienNghiTheoLoaiPhongs.Any(tn =>
                ////        tn.TienNghi.Name.Contains("Sức chứa") && tn.TienNghi.Description.Contains(request.SoLuongKhach.Value.ToString())));
                ////if (request.TienNghi != null && request.TienNghi.Any())
                ////    query = query.Where(p => p.LoaiPhong.tienNghiTheoLoaiPhongs.Any(tn => request.TienNghi.Contains(tn.TienNghi.Name)));
                ////if (request.NgayNhanPhong.HasValue && request.NgayTraPhong.HasValue)
                ////    query = query.Where(p => !_context.DatPhongs.Any(dp => dp.PhongId == p.PhongId &&
                ////        dp.NgayNhanPhong < request.NgayTraPhong && dp.NgayTraPhong > request.NgayNhanPhong));


                //// Sắp xếp
                //if (!string.IsNullOrEmpty(request.SortBy))
                //{
                //    query = request.SortBy.ToLower() switch
                //    {
                //        "gia-asc" => query.OrderBy(p => p.Gia),
                //        "gia-desc" => query.OrderByDescending(p => p.Gia),
                //        _ => query.OrderBy(p => p.PhongId)
                //    };
                //}

                // Đếm tổng số bản ghi
                int totalRecords = await query.CountAsync();

                // Ánh xạ trực tiếp trong truy vấn
                var data = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(p => new ThongTinDanhSachPhongVm
                    {
                        PhongId = p.PhongId,
                        SoPhong = p.SoPhong,
                        Gia = p.Gia,
                        Type = p.LoaiPhong.Type,
                        LoaiGiuong = p.loaiGiuong,
                        LoaiView = p.loaiView,
                        TrangThaiPhong = p.TrangThaiPhong.trangThaiPhong,
                        //SoLuongKhach = p.LoaiPhong.tienNghiTheoLoaiPhongs
                        //    .Where(tn => tn.TienNghi.Name.Contains("Sức chứa"))
                        //    .Select(tn =>
                        //    {
                        //        var str = tn.TienNghi.Description.Replace(" người", "").Trim();
                        //        return int.TryParse(str, out int val) ? val : 0;
                        //    })
                        //    .FirstOrDefault(),
                        AnhPhong = p.anhPhongs.Select(ap => ap.PhotoName).ToList(),
                        TienNghis = p.LoaiPhong.tienNghiTheoLoaiPhongs.Select(tn => tn.TienNghi.Name).ToList()
                    })
                    .ToListAsync();

                // Tạo PagedResult
                var pagedResult = new PagedResult<ThongTinDanhSachPhongVm>
                {
                    TotalRecords = totalRecords,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };

                return pagedResult;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phòng: " + ex.Message, ex);
            }
        }
        public async Task<Result<List<ThongTinDanhSachPhongVm>>> GetAllPhongAsync(PhongSearchRequest request)
        {
            try
            {

                var query = _context.Phongs
                    .Where(p => p.IsActive && !p.IsDeleted)
                    .Include(p => p.LoaiPhong)
                    .Include(p => p.TrangThaiPhong)
                    .Include(p => p.anhPhongs)
                    .Include(p => p.LoaiPhong).ThenInclude(lp => lp.tienNghiTheoLoaiPhongs).ThenInclude(tn => tn.TienNghi)
                    .AsQueryable();

               
                if (request.LoaiPhongId.HasValue)
                    query = query.Where(p => p.LoaiPhongId == request.LoaiPhongId.Value);
                if (request.GiaTu.HasValue)
                    query = query.Where(p => p.Gia >= request.GiaTu.Value);
                if (request.GiaDen.HasValue)
                    query = query.Where(p => p.Gia <= request.GiaDen.Value);
                if (request.LoaiGiuong.HasValue)
                    query = query.Where(p => p.loaiGiuong == request.LoaiGiuong.Value);
                if (request.LoaiView.HasValue)
                    query = query.Where(p => p.loaiView == request.LoaiView.Value);
                if (request.SoLuongKhach.HasValue)
                    query = query.Where(p => p.LoaiPhong.tienNghiTheoLoaiPhongs.Any(tn =>
                        tn.TienNghi.Name.Contains("Sức chứa") && tn.TienNghi.Description.Contains(request.SoLuongKhach.Value.ToString())));
                var data = await query
                    .Select(p => new
                    {
                        p.PhongId,
                        p.SoPhong,
                        p.Gia,
                        p.LoaiPhong.Type,
                        p.loaiGiuong,
                        p.loaiView,
                        TrangThai = p.TrangThaiPhong.trangThaiPhong,
                        TienNghiList = p.LoaiPhong.tienNghiTheoLoaiPhongs.Select(tn => new
                        {
                            Name = tn.TienNghi.Name,
                            Description = tn.TienNghi.Description
                        }).ToList(),
                        AnhPhong = p.anhPhongs.Select(ap => ap.PhotoName).ToList(),
                        TienNghiNames = p.LoaiPhong.tienNghiTheoLoaiPhongs.Select(tn => tn.TienNghi.Name).ToList()
                    })
                    .ToListAsync(); // Lúc này EF thực thi và chuyển thành in-memory

                var result = data.Select(p => new ThongTinDanhSachPhongVm
                {
                    PhongId = p.PhongId,
                    SoPhong = p.SoPhong,
                    Gia = p.Gia,
                    Type = p.Type,
                    LoaiGiuong = p.loaiGiuong,
                    LoaiView = p.loaiView,
                    TrangThaiPhong = p.TrangThai,
                    SoLuongKhach = p.TienNghiList
                        .Where(tn => tn.Name.Contains("Sức chứa"))
                        .Select(tn =>
                        {
                            var str = tn.Description.Replace(" người", "").Trim();
                            return int.TryParse(str, out int val) ? val : 0;
                        })
                        .FirstOrDefault(),
                    AnhPhong = p.AnhPhong,
                    TienNghis = p.TienNghiNames
                }).ToList();



                if (!result.Any())
                    return Result<List<ThongTinDanhSachPhongVm>>.Success("Không có phòng nào phù hợp", new List<ThongTinDanhSachPhongVm>());

                return Result<List<ThongTinDanhSachPhongVm>>.Success($"Tìm thấy {result.Count} phòng", result);
            }
            catch (Exception ex)
            {
                return Result<List<ThongTinDanhSachPhongVm>>.Error($"Lỗi khi lấy danh sách phòng: {ex.Message}");
            }
        }

        public async Task<Result<ChiTietPhongVm>> GetPhongById(int Id)
        {
            try
            {
                var phongEntity = await _context.Phongs
                    .Where(p => p.IsActive && p.PhongId == Id)
                    .Include(p => p.LoaiPhong).ThenInclude(lp => lp.tienNghiTheoLoaiPhongs).ThenInclude(tn => tn.TienNghi)
                    .Include(p => p.TrangThaiPhong)
                    .Include(p => p.anhPhongs)

                    .FirstOrDefaultAsync();

                var feedback = await _context.FeedBacks
                    .Include(f => f.User)
                    .Where(f => f.DatPhong.PhongId == Id)
                    .ToListAsync();

                var diemTB = feedback.Any() ? feedback.Average(f => f.DanhGia) : 0;

                var phongTuongTus = await _context.Phongs
                    .Where(p => p.LoaiPhongId == phongEntity.LoaiPhongId && p.PhongId != phongEntity.PhongId && p.IsActive)
                    .OrderBy(p => Guid.NewGuid()) // random nếu muốn
                    .Take(3)
                    .Select(p => new PhongLienQuanVm
                    {
                        PhongId = p.PhongId,
                        SoPhong = p.SoPhong,
                        Gia = p.Gia,
                        Type = p.LoaiPhong.Type,
                        AnhDaiDien = p.anhPhongs.Select(a => a.PhotoName).FirstOrDefault() ?? "default.png"
                    }).ToListAsync();


                if (phongEntity == null)
                    return Result<ChiTietPhongVm>.Error("Phòng không tồn tại");

                var tienNghiSucChua = phongEntity.LoaiPhong.tienNghiTheoLoaiPhongs
                    .FirstOrDefault(tn => tn.TienNghi.Name.Contains("Sức chứa"));

                int sucChua = 0;
                if (tienNghiSucChua != null)
                {
                    int.TryParse(tienNghiSucChua.TienNghi.Description.Replace(" người", ""), out sucChua);
                }

                // Tạo ViewModel
                var phong = new ChiTietPhongVm
                {
                    PhongId = phongEntity.PhongId,
                    SoPhong = phongEntity.SoPhong,
                    Gia = phongEntity.Gia,
                    Type = phongEntity.LoaiPhong.Type,
                    BinhLuan = phongEntity.LoaiPhong.Description ?? "",
                    LoaiGiuong = phongEntity.loaiGiuong,
                    LoaiView = phongEntity.loaiView,
                    TrangThaiPhong = phongEntity.TrangThaiPhong.trangThaiPhong,
                    SoLuongKhach = sucChua,
                    AnhPhong = phongEntity.anhPhongs.Select(ap => ap.PhotoName).ToList(),
                    TienNghis = phongEntity.LoaiPhong.tienNghiTheoLoaiPhongs.Select(tn => tn.TienNghi.Name).ToList(),
                    Feedbacks = feedback.Select(f => new FeedbackItemVm
                    {
                        TenNguoiDung = f.User.HoTen,
                        DanhGia = f.DanhGia,
                        BinhLuan = f.BinhLuan,
                        Ngay = f.CreateAt
                    }).ToList(),
                    DiemTrungBinh = diemTB,
                    PhongLienQuan = phongTuongTus

                };

                return Result<ChiTietPhongVm>.Success("Lấy thông tin phòng thành công", phong);
            }
            catch (Exception ex)
            {
                return Result<ChiTietPhongVm>.Error($"Lỗi khi lấy chi tiết phòng: {ex.Message}");
            }
        }



        
        public async Task<Result<List<ThongTinDanhSachPhongVm>>> GetAvailablePhongsAsync(PhongSearchRequest request)
        {
            try
            {
                if (!request.NgayNhanPhong.HasValue || !request.NgayTraPhong.HasValue)
                    return Result<List<ThongTinDanhSachPhongVm>>.Error("Vui lòng cung cấp ngày nhận và trả phòng");

                if (request.NgayNhanPhong.Value >= request.NgayTraPhong.Value)
                    return Result<List<ThongTinDanhSachPhongVm>>.Error("Ngày nhận phòng phải nhỏ hơn ngày trả phòng");

                // Lấy danh sách phòng đã đặt
                var bookedPhongIds = await _context.DatPhongs
                    .Where(dp => dp.NgayNhanPhong <= request.NgayTraPhong.Value
                              && dp.NgayTraPhong >= request.NgayNhanPhong.Value
                              && dp.TrangThaiDatPhongId != (int)enTrangThaiDatPhong.DAHUY)
                    .Select(dp => dp.PhongId)
                    .Distinct()
                    .ToListAsync();

                // Truy vấn chính
                var query = _context.Phongs
                    .Where(p => p.IsActive
                             && !bookedPhongIds.Contains(p.PhongId)
                             && p.TrangThaiPhong != null
                             && p.TrangThaiPhong.trangThaiPhong == enTrangThaiPhong.TRONG)
                    .AsQueryable();

                // Lọc theo yêu cầu
                if (request.LoaiPhongId.HasValue)
                    query = query.Where(p => p.LoaiPhongId == request.LoaiPhongId.Value);
                if (request.GiaTu.HasValue)
                    query = query.Where(p => p.Gia >= request.GiaTu.Value);
                if (request.GiaDen.HasValue)
                    query = query.Where(p => p.Gia <= request.GiaDen.Value);
                if (request.LoaiGiuong.HasValue)
                    query = query.Where(p => p.loaiGiuong == request.LoaiGiuong.Value);
                if (request.LoaiView.HasValue)
                    query = query.Where(p => p.loaiView == request.LoaiView.Value);
                if (request.TienNghi != null && request.TienNghi.Any())
                    query = query.Where(p => p.LoaiPhong.tienNghiTheoLoaiPhongs.Any(tn => request.TienNghi.Contains(tn.TienNghi.Name)));

                // Sắp xếp (nếu dùng SortBy)
                if (!string.IsNullOrEmpty(request.SortBy))
                {
                    query = request.SortBy switch
                    {
                        "gia-asc" => query.OrderBy(p => p.Gia),
                        "gia-desc" => query.OrderByDescending(p => p.Gia),
                        //"danhgia-desc" => query.OrderByDescending(p => p.DanhGia),
                        _ => query.OrderBy(p => p.Gia)
                    };
                }
                else
                {
                    query = query.OrderBy(p => p.Gia); // Mặc định
                }

                // Tách Select để giảm độ phức tạp
                var result = await query
                    .Select(p => new ThongTinDanhSachPhongVm
                    {
                        PhongId = p.PhongId,
                        SoPhong = p.SoPhong,
                        Gia = p.Gia,
                        Type = p.LoaiPhong.Type,
                        LoaiGiuong = p.loaiGiuong,
                        LoaiView = p.loaiView,
                        TrangThaiPhong = p.TrangThaiPhong.trangThaiPhong,
                        //DanhGia = p.DanhGia,
                        //BinhLuan = p.BinhLuan,
                        AnhPhong = p.anhPhongs.Select(ap => ap.PhotoName).ToList(),
                        TienNghis = p.LoaiPhong.tienNghiTheoLoaiPhongs.Select(tn => tn.TienNghi.Name).ToList()
                    })
                    .ToListAsync();

                if (!result.Any())
                    return Result<List<ThongTinDanhSachPhongVm>>.Success("Không tìm thấy phòng trống", new List<ThongTinDanhSachPhongVm>());

                return Result<List<ThongTinDanhSachPhongVm>>.Success($"Tìm thấy {result.Count} phòng trống", result);
            }
            catch (Exception ex)
            {
                return Result<List<ThongTinDanhSachPhongVm>>.Error($"Lỗi khi tìm kiếm phòng: {ex.Message}");
            }
        }

        // Tính tổng giá dựa trên số đêm
        public async Task<Result<decimal>> CalculateTotalPriceAsync(int phongId, DateTime ngayNhanPhong, DateTime ngayTraPhong)
        {
            try
            {
                // Chuẩn hóa ngày (bỏ giờ)
                ngayNhanPhong = ngayNhanPhong.Date;
                ngayTraPhong = ngayTraPhong.Date;

                if (ngayNhanPhong >= ngayTraPhong)
                    return Result<decimal>.Error("Ngày nhận phòng phải trước ngày trả phòng");

                var phong = await _context.Phongs
                    .Where(p => p.IsActive && p.PhongId == phongId)
                    .FirstOrDefaultAsync();

                if (phong == null)
                    return Result<decimal>.Error("Phòng không tồn tại hoặc không hoạt động");

                // Kiểm tra phòng đã đặt
                var isBooked = await _context.DatPhongs
                    .AnyAsync(dp => dp.PhongId == phongId
                                 && dp.NgayNhanPhong < ngayTraPhong
                                 && dp.NgayTraPhong > ngayNhanPhong
                                 && dp.TrangThaiDatPhongId != (int)enTrangThaiDatPhong.DAHUY);

                if (isBooked)
                    return Result<decimal>.Error("Phòng đã được đặt trong khoảng thời gian này");

                var soDem = (ngayTraPhong - ngayNhanPhong).Days;
                if (soDem <= 0)
                    return Result<decimal>.Error("Số đêm phải lớn hơn 0");

                var tongTien = phong.Gia * soDem;

                return Result<decimal>.Success($"Tổng giá cho {soDem} đêm", tongTien);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Error($"Lỗi khi tính tổng giá: {ex.Message}");
            }
        }
        #endregion


        #region ADMIN
       
        public async Task<Result<int>> Create(PhongCreateRequest request)
        {
            try


            {
                _action = $"Thêm phòng thành công!";

                if (await _context.Phongs.AnyAsync(x => x.SoPhong == request.SoPhong))
                    return Result<int>.Error($"Phòng {request.SoPhong} đã tồn tại!");

                var obj = new Phong()
                {
                    SoPhong = request.SoPhong,
                    LoaiPhongId = request.LoaiPhongId,
                    TrangThaiPhongId = request.TrangThaiPhongId,
                    Gia = request.Gia,
                    loaiGiuong = request.LoaiGiuong,
                    loaiView = request.LoaiView,
                    SoNguoiLonToiDa = request.SoNguoiLonToiDa,
                    SoTreEmToiDa = request.SoTreEmToiDa,
                    IsActive = true,
                    CreateBy = "admin",
                    CreateAt = DateTime.Now,
                    ModifiedBy = "system",
                    LateModifiedDate = DateTime.Now,
                    IsDeleted = false
                };

                _context.Phongs.Add(obj);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return Result<int>.Success(_action, obj.PhongId);
                }

                return Result<int>.Error("Cập nhật thất bại!");
            }
            catch (Exception ex)
            {
                return Result<int>.Error("Lỗi khi thêm phòng: " + ex.Message);
            }
        }
        public async Task<Result<int>> Update(PhongUpdateRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id))
                    return Result<int>.Error("Id phòng không hợp lệ!");
                int id = request.Id.DecodeId();

                var obj = await _context.Phongs.FindAsync(id);
                if (obj == null)
                    return Result<int>.Error("Không tìm thấy phòng cần sửa!");

                var existingRoom = await _context.Phongs
                    .Where(p => p.SoPhong == request.SoPhong && p.PhongId != id && !p.IsDeleted)
                    .FirstOrDefaultAsync();
                if (existingRoom != null)
                    return Result<int>.Error("Số phòng đã tồn tại!");


                obj.SoPhong = request.SoPhong;
                obj.Gia = request.Gia ?? 0;
                obj.loaiGiuong = request.loaiGiuong ?? enLoaiGiuong.DOI;
                obj.loaiView = request.loaiView ?? enLoaiView.THANHPHO;
                obj.LoaiPhongId = request.LoaiPhongId ?? 1;
                obj.TrangThaiPhongId = request.TrangThaiPhongId ?? 1;
                obj.SoNguoiLonToiDa = request.SoNguoiLonToiDa ?? 2;
                obj.SoTreEmToiDa = request.SoTreEmToiDa ?? 0;
                obj.ModifiedBy = request.UserId.ToString();
                obj.LateModifiedDate = DateTime.Now;

                _context.Phongs.Update(obj);
                var result = await SaveChange();

                return result > 0
                    ? Result<int>.Success("Cập nhật phòng thành công", id)
                    : Result<int>.Error("Cập nhật thất bại!");
            }
            catch
            {
                return Result<int>.Error("Lỗi hệ thống khi cập nhật phòng!");
            }
        }

        public async Task<Result<int>> Delete(DeleteRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                var obj = await _context.Phongs.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error("Không tìm thấy phòng cần xóa!");

                obj.IsDeleted = true;

                obj.ModifiedBy = request.UserId.ToString();
                obj.LateModifiedDate = DateTime.Now;

                _context.Phongs.Update(obj);
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
        public async Task<PhongUpdateRequest> GetById(int id)
        {
            var phong = await _context.Phongs.FindAsync(id);
            if (phong == null) return null;

            return new PhongUpdateRequest
            {
                Id = phong.PhongId.EncodeId1(),
                SoPhong = phong.SoPhong,
                Gia = phong.Gia,
                loaiGiuong = phong.loaiGiuong,
                loaiView = phong.loaiView,
                LoaiPhongId = phong.LoaiPhongId,
                TrangThaiPhongId = phong.TrangThaiPhongId,
                SoNguoiLonToiDa = phong.SoNguoiLonToiDa,
                SoTreEmToiDa = phong.SoTreEmToiDa
            };
        }


        #endregion
    }
}
