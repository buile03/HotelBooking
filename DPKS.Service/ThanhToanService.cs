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
                .FirstOrDefaultAsync(x => x.Id == datPhongId);

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
    }
}
