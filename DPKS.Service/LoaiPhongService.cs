using DPKS.Common.Enum;
using DPKS.Common.Result;
using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Model.Feedback;
using DPKS.Model.LoaiPhong;
using DPKS.Model.LoaiPhong.Request;
using DPKS.Model.TienNghi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DPKS.Service
{
    public interface ILoaiPhongService
    {
        Task<Result<List<ThongTinLoaiPhongVm>>> GetAllLoaiPhong();
        Task<Result<LoaiPhongDetailVm>> GetPhongById(int loaiPhongId);
    }
    public class LoaiPhongService : BaseService, ILoaiPhongService
    {
        private readonly AppDbContext _context;

        public LoaiPhongService(AppDbContext context, IStorageService storageService) : base(context, storageService)
        {
            _context = context;
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
    }
}
