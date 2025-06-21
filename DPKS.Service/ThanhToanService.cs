using DPKS.Common.Enum;
using DPKS.Data.Config;
using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Model.ThanhToan;
using DPKS.Model.ThanhToan.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface IThanhToanService
    {
        Task<ThanhToanVm> GetByDatPhongId (int datPhongId);
        Task<bool> Create(ThanhToanCreateRequest request);
        // các phương thức thanh toán
        Task ThanhToanTienMat(int datPhongId);
        Task ThanhToanStripe(int datPhongId);
        Task ThanhToanPaypal(int datPhongId);
        Task ThanhToanMoMo(int datPhongId);
        Task ThanhToanVnPay(int datPhongId);
    }

    public class ThanhToanService : BaseService, IThanhToanService
    {
        private readonly AppDbContext _context;

        public ThanhToanService(AppDbContext context, IStorageService storageService
            ) : base(context, storageService)
        {
            _context = context;
        }

        public async Task<ThanhToanVm> GetByDatPhongId(int datPhongId)
        {
            var tt = await _context.ThanhToans
                .Include(x => x.PhuongThucThanhToan)
                .FirstOrDefaultAsync(x => x.DatPhongId == datPhongId);

            if (tt == null)
                return null;

            return new ThanhToanVm
            {
                Id = tt.Id,
                DatPhongId = tt.DatPhongId,
                PhuongThucThanhToanId = tt.PhuongThucThanhToanId,
                TenPhuongThucThanhToan = tt.PhuongThucThanhToan?.loaiThanhToan.ToString(),
                Gia = tt.Gia,
                ThoiDiemThanhToan = tt.ThoiDiemThanhToan
            };
        }

        public async Task<bool> Create(ThanhToanCreateRequest request)
        {
            var datPhong = await _context.DatPhongs.FindAsync(request.DatPhongId);
            if (datPhong == null) 
                return false;

            // Kiểm tra nếu đã có thanh toán thì không tạo mới
            var existing = await _context.ThanhToans
                .AnyAsync(x => x.DatPhongId == request.DatPhongId);

            if (existing)
                return false;

            var thanhToan = new ThanhToan
            {
                DatPhongId = request.DatPhongId,
                PhuongThucThanhToanId = request.PhuongThucThanhToanId,
                Gia = request.Gia,
                ThoiDiemThanhToan = request.ThoiDiemThanhToan != DateTime.MinValue ? request.ThoiDiemThanhToan : DateTime.Now
            };

            _context.ThanhToans.Add(thanhToan);

            //datPhong.TrangThaiDatPhong = enTrangThaiDatPhong.DANHANPHONG;

            await _context.SaveChangesAsync();
            return true;
        }

        //Thanh toán bằng tiền mặt
        public async Task ThanhToanTienMat(int datPhongId)
        {
            var datPhong = await _context.DatPhongs.FindAsync(datPhongId);
            if (datPhong == null)
                throw new Exception("Không tìm thấy đơn đặt phòng.");

            if (_context.ThanhToans.Any(t => t.DatPhongId == datPhongId))
                throw new Exception("Đơn đặt phòng đã được thanh toán.");

            var thanhToan = new ThanhToan
            {
                DatPhongId = datPhongId,
                PhuongThucThanhToanId = (int)enLoaiThanhToan.TienMat,
                Gia = datPhong.TongTien,
                ThoiDiemThanhToan = DateTime.Now
            };

            _context.ThanhToans.Add(thanhToan);
            datPhong.TrangThaiDatPhongId = (int)enTrangThaiDatPhong.DATHANHTOAN;
            await _context.SaveChangesAsync();
        }

        //Thanh toán bằng Stripe
        public async Task ThanhToanStripe(int datPhongId)
        {
            var datPhong = await _context.DatPhongs.FindAsync(datPhongId);
            if (datPhong == null) throw new Exception("Đặt phòng không tồn tại");

            if (_context.ThanhToans.Any(t => t.DatPhongId == datPhongId))
                return; // Tránh ghi đè nếu đã thanh toán

            var thanhToan = new ThanhToan
            {
                DatPhongId = datPhongId,
                PhuongThucThanhToanId = (int)enLoaiThanhToan.Stripe,
                Gia = datPhong.TongTien,
                ThoiDiemThanhToan = DateTime.Now,
                TrangThai = enTrangThaiThanhToan.ThanhCong
            };

            _context.ThanhToans.Add(thanhToan);
            datPhong.TrangThaiDatPhongId = (int)enTrangThaiDatPhong.DATHANHTOAN;
            await _context.SaveChangesAsync();
        }

        //Thanh toán bằng Paypal
        public async Task ThanhToanPaypal(int datPhongId)
        {
            var datPhong = await _context.DatPhongs.FindAsync(datPhongId);
            if (datPhong == null)
                throw new Exception("Không tìm thấy đơn đặt phòng.");

            if (_context.ThanhToans.Any(t => t.DatPhongId == datPhongId))
                throw new Exception("Đơn đặt phòng đã được thanh toán.");

            var thanhToan = new ThanhToan
            {
                DatPhongId = datPhongId,
                PhuongThucThanhToanId = (int)enLoaiThanhToan.PayPal,
                Gia = datPhong.TongTien,
                ThoiDiemThanhToan = DateTime.Now,
                TrangThai = enTrangThaiThanhToan.ThanhCong
            };

            _context.ThanhToans.Add(thanhToan);
            datPhong.TrangThaiDatPhongId = (int)enTrangThaiDatPhong.DATHANHTOAN;
            await _context.SaveChangesAsync();
        }

        //Thanh toán bằng Momo
        public async Task ThanhToanMoMo(int datPhongId)
        {
            var datPhong = await _context.DatPhongs.FindAsync(datPhongId);
            if (datPhong == null || _context.ThanhToans.Any(t => t.DatPhongId == datPhongId)) return;

            var thanhToan = new ThanhToan
            {
                DatPhongId = datPhongId,
                PhuongThucThanhToanId = (int)enLoaiThanhToan.Momo,
                Gia = datPhong.TongTien,
                ThoiDiemThanhToan = DateTime.Now,
                TrangThai = enTrangThaiThanhToan.ThanhCong
            };

            _context.ThanhToans.Add(thanhToan);
            datPhong.TrangThaiDatPhongId = (int)enTrangThaiDatPhong.DATHANHTOAN;
            await _context.SaveChangesAsync();
        }

        //Thanh toán bằng VNPay
        public async Task ThanhToanVnPay(int datPhongId)
        {
            var datPhong = await _context.DatPhongs.FindAsync(datPhongId);
            if (datPhong == null || _context.ThanhToans.Any(x => x.DatPhongId == datPhongId)) return;

            var thanhToan = new ThanhToan
            {
                DatPhongId = datPhongId,
                PhuongThucThanhToanId = (int)enLoaiThanhToan.VNPay,
                Gia = datPhong.TongTien,
                ThoiDiemThanhToan = DateTime.Now,
                TrangThai = enTrangThaiThanhToan.ThanhCong
            };

            _context.ThanhToans.Add(thanhToan);
            datPhong.TrangThaiDatPhongId = (int)enTrangThaiDatPhong.DATHANHTOAN;
            await _context.SaveChangesAsync();
        }

    }
}
