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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DPKS.Service
{
    public interface IPhongService
    {
        Task<Result<List<ThongTinDanhSachPhongVm>>> GetAllPhongAsync(PhongSearchRequest request);
        Task<Result<ChiTietPhongVm>> GetPhongById(int Id);
        Task<Result<List<ThongTinDanhSachPhongVm>>> GetAvailablePhongsAsync(PhongSearchRequest request);
        Task<Result<decimal>> CalculateTotalPriceAsync(int phongId, DateTime ngayNhanPhong, DateTime ngayTraPhong);

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
        public async Task<Result<List<ThongTinDanhSachPhongVm>>> GetAllPhongAsync(PhongSearchRequest request)
        {
            try
            {

                var query = _context.Phongs
                    .Where(p => p.IsActive)
                    .Include(p => p.LoaiPhong)
                    .Include(p => p.TrangThaiPhong)
                    .Include(p => p.anhPhongs)
                    .Include(p => p.LoaiPhong).ThenInclude(lp => lp.tienNghiTheoLoaiPhongs).ThenInclude(tn => tn.TienNghi)
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
                    TienNghis = phongEntity.LoaiPhong.tienNghiTheoLoaiPhongs.Select(tn => tn.TienNghi.Name).ToList()
                };

                return Result<ChiTietPhongVm>.Success("Lấy thông tin phòng thành công", phong);
            }
            catch (Exception ex)
            {
                return Result<ChiTietPhongVm>.Error($"Lỗi khi lấy chi tiết phòng: {ex.Message}");
            }
        }

        // Lấy danh sách phòng khả dụng theo ngày nhận và trả phòng
        //public async Task<Result<List<ThongTinDanhSachPhongVm>>> GetAvailablePhongsAsync(PhongSearchRequest request)
        //{
        //    try
        //    {
        //        if (!request.NgayNhanPhong.HasValue || !request.NgayTraPhong.HasValue)
        //            return Result<List<ThongTinDanhSachPhongVm>>.Error("Vui lòng cung cấp ngày nhận và trả phòng");

        //        if (request.NgayNhanPhong.Value >= request.NgayTraPhong.Value)
        //            return Result<List<ThongTinDanhSachPhongVm>>.Error("Ngày nhận phòng phải nhỏ hơn ngày trả phòng");

        //        var bookedPhongIds = await _context.DatPhongs
        //            .Where(dp => dp.NgayNhanPhong <= request.NgayTraPhong.Value
        //                      && dp.NgayTraPhong >= request.NgayNhanPhong.Value
        //                      && dp.TrangThaiDatPhongId != (int)enTrangThaiDatPhong.DAHUY) // Sử dụng trạng thái DAHUY = 4
        //            .Select(dp => dp.PhongId)
        //            .Distinct()
        //            .ToListAsync();

        //        var query = _context.Phongs
        //            .Where(p => p.IsActive
        //                     && !bookedPhongIds.Contains(p.PhongId)
        //                     && p.TrangThaiPhong.trangThaiPhong == enTrangThaiPhong.TRONG)
        //            .Include(p => p.LoaiPhong)
        //            .Include(p => p.TrangThaiPhong)
        //            .Include(p => p.anhPhongs)
        //            .Include(p => p.LoaiPhong).ThenInclude(lp => lp.tienNghiTheoLoaiPhongs).ThenInclude(tn => tn.TienNghi)
        //            .AsQueryable();

        //        // Lọc thêm theo yêu cầu
        //        if (request.LoaiPhongId.HasValue)
        //            query = query.Where(p => p.LoaiPhongId == request.LoaiPhongId.Value);
        //        if (request.GiaTu.HasValue)
        //            query = query.Where(p => p.Gia >= request.GiaTu.Value);
        //        if (request.GiaDen.HasValue)
        //            query = query.Where(p => p.Gia <= request.GiaDen.Value);
        //        if (request.LoaiGiuong.HasValue)
        //            query = query.Where(p => p.loaiGiuong == request.LoaiGiuong.Value);
        //        if (request.LoaiView.HasValue)
        //            query = query.Where(p => p.loaiView == request.LoaiView.Value);
        //        if (request.SoLuongKhach.HasValue)
        //            query = query.Where(p => p.LoaiPhong.tienNghiTheoLoaiPhongs.Any(tn =>
        //                tn.TienNghi.Name.Contains("Sức chứa") && tn.TienNghi.Description.Contains(request.SoLuongKhach.Value.ToString())));

        //        var result = await query.Select(p => new ThongTinDanhSachPhongVm
        //        {
        //            PhongId = p.PhongId,
        //            SoPhong = p.SoPhong,
        //            Gia = p.Gia,
        //            Type = p.LoaiPhong.Type,
        //            LoaiGiuong = p.loaiGiuong,
        //            LoaiView = p.loaiView,
        //            TrangThaiPhong = p.TrangThaiPhong.trangThaiPhong,
        //            //SoLuongKhach = p.LoaiPhong.tienNghiTheoLoaiPhongs
        //            //    .Where(tn => tn.TienNghi.Name.Contains("Sức chứa"))
        //            //    .Select(tn => int.TryParse(tn.TienNghi.Description.Replace(" người", ""), out int sucChua) ? sucChua : 0)
        //            //    .FirstOrDefault(),
        //            AnhPhong = p.anhPhongs.Select(ap => ap.PhotoName).ToList(),
        //            TienNghis = p.LoaiPhong.tienNghiTheoLoaiPhongs.Select(tn => tn.TienNghi.Name).ToList()
        //        }).ToListAsync();

        //        if (!result.Any())
        //            return Result<List<ThongTinDanhSachPhongVm>>.Success("Không có phòng trống trong khoảng thời gian yêu cầu", new List<ThongTinDanhSachPhongVm>());

        //        return Result<List<ThongTinDanhSachPhongVm>>.Success($"Tìm thấy {result.Count} phòng trống", result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<List<ThongTinDanhSachPhongVm>>.Error($"Lỗi khi lấy danh sách phòng trống: {ex.Message}");
        //    }
        //}

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
    }
}
