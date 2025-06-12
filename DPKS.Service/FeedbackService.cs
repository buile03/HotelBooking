using DPKS.Common.Result;
using DPKS.Data.EF;
using DPKS.Model.Feedback;
using DPKS.Model.Feedback.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface IFeedbackService
    {
        Task<Result<List<DanhSachFeedbackVm>>> GetAll(SearchFeedbackRequest request); // Lấy tất cả feedback có phân trang và lọc theo điều kiện tìm kiếm.
        Task<Result<FeedbackDetailVm>> GetById(int id); // Lấy chi tiết một feedback theo ID.
        //Task<Result<FeedbackDetailVm>> Create(CreateFeedbackRequest request); // Tạo mới một feedback từ người dùng.
        //Task<Result<FeedbackDetailVm>> Update(int id, UpdateFeedbackRequest request); // Cập nhật thông tin của một feedback theo ID.
        //Task<Result<bool>> Delete(int id); // Xóa một feedback theo ID.
        //Task<Result<List<DanhSachFeedbackVm>>> GetByUserId(int userId);  // Lấy danh sách feedback theo ID của người dùng.
        //Task<Result<List<DanhSachFeedbackVm>>> GetByDatPhongId(int datPhongId);  // Lấy danh sách feedback theo ID của đặt phòng.
        //Task<Result<List<DanhSachFeedbackVm>>> GetByRating(int rating);  // Lấy danh sách feedback theo mức đánh giá (rating).
        //Task<Result<FeedbackStatisticsVm>> GetStatistics();  // Lấy thống kê tổng quan về feedback.
        //Task<Result<List<DanhSachFeedbackVm>>> GetRecentFeedbacks(int count = 10);  // Lấy danh sách feedback mới nhất theo số lượng.
        //Task<Result<bool>> CheckUserCanFeedback(int userId, int datPhongId);  // Kiểm tra người dùng có thể để lại feedback cho đặt phòng hay không.
    }
    public class FeedbackService : BaseService, IFeedbackService
    {
        private readonly AppDbContext _context;
        
        public FeedbackService(AppDbContext context
            , IStorageService storageService) : 
            base(context, storageService)
        {
            _context = context;
        }

        public async Task<Result<List<DanhSachFeedbackVm>>> GetAll(SearchFeedbackRequest request)
        {
            try
            {
                var query = from g in _context.FeedBacks
                            join u in _context.Users on g.UserId equals u.Id
                            join dp in _context.DatPhongs on g.DatPhongId equals dp.Id
                            join p in _context.Phongs on dp.PhongId equals p.PhongId
                            select new DanhSachFeedbackVm
                            {
                                Id = g.Id,
                                DanhGia = g.DanhGia,
                                BinhLuan = g.BinhLuan,
                                CreateAt = g.CreateAt,
                                UserName = u.UserName,
                                SoPhong = p.SoPhong,
                                NgayNhanPhong = dp.NgayNhanPhong,
                                NgayTraPhong = dp.NgayTraPhong
                                
                            };
                // Truy vấn lọc
                if(request != null)
                {
                    if (request.DanhGia.HasValue)
                        query = query.Where(x => x.DanhGia == request.DanhGia.Value);
                    if (!string.IsNullOrEmpty(request.Keyword))
                        query = query.Where(x => x.BinhLuan.Contains(request.Keyword) || x.UserName.Contains(request.Keyword));
                    if (request.TuNgay.HasValue)
                        query = query.Where(x => x.CreateAt >= request.TuNgay.Value);
                    if(request.DenNgay.HasValue)
                        query = query.Where(x => x.CreateAt <=  request.DenNgay.Value);
                }

                query = query.OrderByDescending(x => x.CreateAt);


                if (!await query.AnyAsync())
                    return Result<List<DanhSachFeedbackVm>>.Error("Không có dữ liệu để hiển thị");
                return Result<List<DanhSachFeedbackVm>>.Success($"Hiển thị {await query.CountAsync()} feedback", await query.ToListAsync());
            }
            catch (Exception ex)
            {
                return Result<List<DanhSachFeedbackVm>>.Error($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<Result<FeedbackDetailVm>> GetById(int id)
        {
            try
            {
                var query = await (from f in _context.FeedBacks
                            join u in _context.Users on f.UserId equals u.Id
                            join dp in _context.DatPhongs on f.DatPhongId equals dp.Id
                            join p in _context.Phongs on dp.PhongId equals p.PhongId
                            where f.Id == id
                            select new FeedbackDetailVm
                            {
                                Id = f.Id,
                                UserId = f.UserId,
                                DatPhongId = f.DatPhongId,
                                DanhGia = f.DanhGia,
                                BinhLuan = f.BinhLuan,
                                CreateAt = f.CreateAt,
                                UserName = u.UserName,
                                Email = u.Email,
                                Type = p.SoPhong,
                                NgayNhanPhong = dp.NgayNhanPhong,
                                NgayTraPhong = dp.NgayTraPhong,
                                TongTien = dp.TongTien
                            }).FirstOrDefaultAsync();
                if (query == null)
                    return Result<FeedbackDetailVm>.Error("Không tìm thấy feedback");
                return Result<FeedbackDetailVm>.Success($"Lấy thông tin thành công", query);
            }
            catch (Exception ex)
            {
                return Result<FeedbackDetailVm>.Error($"Có lỗi xảy ra : {ex.Message}");
            }
        }
    }
}
