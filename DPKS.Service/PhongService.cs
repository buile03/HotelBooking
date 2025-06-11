using DPKS.Data.EF;
using DPKS.Model.Phong;
using Microsoft.AspNetCore.Http;
using DPKS.Data.Entites;
using DPKS.Model.TienNghi;
using DPKS.Model.TienNghi.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPKS.Common.Result;
using DPKS.Model.Phong.Request;

namespace DPKS.Service
{
    public interface IPhongService
    {
        Task<Result<List<ThongTinDanhSachPhongVm>>> GetAllPhongAsync(PhongSearchRequest request);

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
                // Version đơn giản nhất để test trước
                var query = from p in _context.Phongs
                            where p.IsActive
                            select new ThongTinDanhSachPhongVm
                            {
                                PhongId = p.PhongId,
                                SoPhong = p.SoPhong,
                                Gia = p.Gia,
                                Type = "N/A", // Tạm thời hardcode để test
                                TrangThaiPhong = Common.Enum.enTrangThaiPhong.TRONG, // Tạm thời hardcode để test
                                SoLuongKhach = 1,
                                TienNghi = new List<string>() // Empty list để test
                            };

                var data = await query.ToListAsync();

                if (!data.Any())
                    return Result<List<ThongTinDanhSachPhongVm>>.Success("Không có dữ liệu để hiển thị", new List<ThongTinDanhSachPhongVm>());

                return Result<List<ThongTinDanhSachPhongVm>>.Success($"Hiển thị {data.Count} phòng", data);
            }
            catch (Exception ex)
            {
                return Result<List<ThongTinDanhSachPhongVm>>.Error($"Lỗi: {ex.Message}");
            }
        }
    }
}
