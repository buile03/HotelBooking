using DPKS.Common.Enum;
using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Model.DatPhong;
using DPKS.Model.DatPhong.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface IDatPhongService
    {
        Task<int> DatPhongAsync(DatPhongCreateRequest request);
        Task<List<ThongTinDatPhongVm>> GetListByUserIdAsync(int userId);
        Task<ThongTinDatPhongVm> GetByIdAsync(int id);
        Task<bool> HuyDatPhongAsync(int id);
        Task<bool> CapNhatTrangThaiDatPhongAsync(int datPhongId, enTrangThaiDatPhong trangThaiMoi);
    }
    public class DatPhongService : BaseService, IDatPhongService
    {
        private readonly AppDbContext _context;

        public DatPhongService(AppDbContext context, IStorageService storageService) : base(context, storageService)
        {
            _context = context;
        }

        public async Task<int> DatPhongAsync(DatPhongCreateRequest request)
        {
            var phong = await _context.Phongs.FindAsync(request.PhongId);
            if (phong == null || !phong.IsActive)
                throw new Exception("Phòng không tồn tại hoặc không khả dụng.");

            int soDem = (request.NgayTraPhong - request.NgayNhanPhong).Days;
            if (soDem <= 0 || soDem > 31) throw new Exception("Số đêm không hợp lệ.");

            var ktraXungDot = await _context.DatPhongs
                    .AnyAsync(dp => dp.PhongId == request.PhongId &&
                    dp.TrangThaiDatPhongId != (int)enTrangThaiDatPhong.DAHUY &&
                    !(request.NgayTraPhong <= dp.NgayNhanPhong || request.NgayNhanPhong >= dp.NgayTraPhong));

            if (ktraXungDot)
                throw new Exception("Phòng này đã được đặt trong khoảng thời gian bạn chọn.");

            var tongTien = phong.Gia * soDem;

            var datPhong = new DatPhong
            {
                PhongId = request.PhongId,
                UserId = request.UserId,
                NgayNhanPhong = request.NgayNhanPhong,
                NgayTraPhong = request.NgayTraPhong,
                SoLuongKhach = request.SoLuongKhach,
                SoDem = soDem,
                TongTien = tongTien,
                TrangThaiDatPhongId = (int)enTrangThaiDatPhong.CHOTHANHTOAN,
                NgayDat = DateTime.Now,
                HoTen = request.HoTen,
                Email = request.Email,
                SDT = request.SDT,
                GhiChu = request.GhiChu
            };

            _context.DatPhongs.Add(datPhong);
            await _context.SaveChangesAsync();

            return datPhong.Id;
        }

        public async Task<List<ThongTinDatPhongVm>> GetListByUserIdAsync(int userId)
        {
            return await _context.DatPhongs
                .Where(x => x.UserId == userId)
                .Include(x => x.Phong)
                .Include(x => x.User)
                .Select(x => new ThongTinDatPhongVm
                {
                    Id = x.Id,
                    PhongId = x.PhongId,
                    SoPhong = x.Phong.SoPhong,
                    UserId = x.UserId,
                    TenKhachHang = x.User.HoTen,
                    NgayNhanPhong = x.NgayNhanPhong,
                    NgayTraPhong = x.NgayTraPhong,
                    SoDem = x.SoDem,
                    SoLuongKhach = x.SoLuongKhach,
                    TongTien = x.TongTien,
                    TrangThaiDatPhong = (enTrangThaiDatPhong)x.TrangThaiDatPhongId,
                    DaThanhToan = _context.ThanhToans.Any(t => t.DatPhongId == x.Id)
                }).ToListAsync();
        }


        public async Task<ThongTinDatPhongVm> GetByIdAsync(int id)
        {
            var dp = await _context.DatPhongs
                .Include(x => x.Phong)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dp == null) return null;

            return new ThongTinDatPhongVm
            {
                Id = dp.Id,
                PhongId = dp.PhongId,
                SoPhong = dp.Phong?.SoPhong,
                UserId = dp.UserId,
                TenKhachHang = dp.User?.HoTen,
                NgayNhanPhong = dp.NgayNhanPhong,
                NgayTraPhong = dp.NgayTraPhong,
                SoDem = dp.SoDem,
                SoLuongKhach = dp.SoLuongKhach,
                TongTien = dp.TongTien,
                TrangThaiDatPhong = (enTrangThaiDatPhong)dp.TrangThaiDatPhongId
            };
        }
        public async Task<bool> HuyDatPhongAsync(int id)
        {
            return await CapNhatTrangThaiDatPhongAsync(id, enTrangThaiDatPhong.DAHUY);
        }

        public async Task<bool> CapNhatTrangThaiDatPhongAsync(int datPhongId, enTrangThaiDatPhong trangThaiMoi)
        {
            var datPhong = await _context.DatPhongs
                .Include(x => x.Phong)
                .FirstOrDefaultAsync(x => x.Id == datPhongId);

            if (datPhong == null) return false;

            datPhong.TrangThaiDatPhongId = (int)trangThaiMoi;

            switch (trangThaiMoi)
            {
                case enTrangThaiDatPhong.DANHANPHONG:
                    datPhong.Phong.TrangThaiPhongId = (int)enTrangThaiPhong.DANGO;
                    break;
                case enTrangThaiDatPhong.DATRAPHONG:
                    datPhong.Phong.TrangThaiPhongId = (int)enTrangThaiPhong.DANGDONDEP;
                    break;
                case enTrangThaiDatPhong.DAHUY:
                case enTrangThaiDatPhong.KHONGDEN:
                    datPhong.Phong.TrangThaiPhongId = (int)enTrangThaiPhong.TRONG;
                    break;
                default:
                    break;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
