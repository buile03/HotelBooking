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
using DPKS.Model.Phong.Request;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DPKS.Service
{
    public interface IPhongService
    {
        Task<Result<List<ThongTinDanhSachPhongVm>>> GetAllPhongAsync(PhongSearchRequest request);
        Task<Result<List<ThongTinDanhSachPhongVm>>> GetPhongById(PhongSearchRequest request);

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
                //// Version đơn giản nhất để test trước
                //var query = from p in _context.Phongs
                //            join dp in _context.DatPhongs on p.PhongId equals dp.PhongId
                //            where p.IsActive
                //            select new ThongTinDanhSachPhongVm
                //            {
                //                PhongId = p.PhongId,
                //                SoPhong = p.SoPhong,
                //                Gia = p.Gia,
                //                Type = p.LoaiPhong.Type,
                //                TrangThaiPhong = Common.Enum.enTrangThaiPhong.TRONG,
                //                SoLuongKhach = dp.SoLuongKhach,
                //                TienNghi = new List<string>() // Empty list để test
                //            };

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

        public async Task<Result<List<ThongTinDanhSachPhongVm>>> GetPhongById(PhongSearchRequest request)
        {
            try
            {
                var phong = await _context.Phongs
                    .Where(p => p.IsActive && p.PhongId == )
                    .Include(p => p.LoaiPhong)
                    .Include(p => p.TrangThaiPhong)
                    .Include(p => p.anhPhongs)
                    .Include(p => p.LoaiPhong).ThenInclude(lp => lp.tienNghiTheoLoaiPhongs).ThenInclude(tn => tn.TienNghi)
                    .Select(p => new ThongTinDanhSachPhongVm
                    {
                        PhongId = p.PhongId,
                        SoPhong = p.SoPhong,
                        Gia = p.Gia,
                        Type = p.LoaiPhong.Type,
                        BinhLuan = p.LoaiPhong.Description ?? "",
                        LoaiGiuong = p.loaiGiuong,
                        LoaiView = p.loaiView,
                        TrangThaiPhong = p.TrangThaiPhong.trangThaiPhong,
                        SoLuongKhach = p.LoaiPhong.tienNghiTheoLoaiPhongs
                            .Where(tn => tn.TienNghi.Name.Contains("Sức chứa"))
                            .Select(tn => int.TryParse(tn.TienNghi.Description.Replace(" người", ""), out int sucChua) ? sucChua : 0)
                            .FirstOrDefault(),
                        AnhPhong = p.anhPhongs.Select(ap => ap.PhotoName).ToList(),
                        TienNghis = p.LoaiPhong.tienNghiTheoLoaiPhongs.Select(tn => tn.TienNghi.Name).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (phong == null)
                    return Result<ThongTinDanhSachPhongVm>.Error("Phòng không tồn tại hoặc không hoạt động");

                return Result<ThongTinDanhSachPhongVm>.Success("Lấy thông tin phòng thành công", phong);
            }
            catch (Exception ex)
            {
                return Result<ThongTinDanhSachPhongVm>.Error($"Lỗi khi lấy chi tiết phòng: {ex.Message}");
            }
        }
    }
}
