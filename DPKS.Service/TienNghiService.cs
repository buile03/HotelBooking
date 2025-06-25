using DPKS.Common;
using DPKS.Common.Helper;
using DPKS.Common.Result;
using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Model.TienNghi;
using DPKS.Model.TienNghi.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface ITienNghiService
    {
        Task<PagedResult<DanhSachTienNghiVm>> GetPaging(TienNghiSearchRequest request);
        Task<Result<PagedResult<DanhSachTienNghiVm>>> GetAll(GetPagingRequest request);
        Task<Result<int>> Create(TienNghiCreateRequest request);
        Task<Result<int>> Update(TienNghiUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<TienNghiUpdateRequest> GetById(int id);
        Task<Result<List<DanhSachTienNghiVm>>> GetAll();

    }
    public class TienNghiService : BaseService, ITienNghiService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TienNghiService(AppDbContext context
            , IStorageService storageService
            , IHttpContextAccessor httpContextAccessor) : base(context, storageService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        //public string GetDomain()
        //{
        //    var request = _httpContextAccessor.HttpContext.Request;
        //    return $"{request.Scheme}://{request.Host.Value}";
        //}

        public async Task<PagedResult<DanhSachTienNghiVm>> GetPaging(TienNghiSearchRequest request)
        {
            try
            {
                var query = _context.TienNghis
                    .Where(tn => tn.IsActive && !tn.IsDeleted)
                    .Include(tn => tn.loaiPhongs)
                    .Include(tn => tn.tienNghiTheoLoaiPhongs)
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    var keyword = request.Keyword.ToLower();
                    query = query.Where(p =>
                        p.Name.ToLower().Contains(keyword));
                }

                int totalRecords = await query.CountAsync();

                var data = await query
                    .OrderBy(tn => tn.Id)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(tn => new DanhSachTienNghiVm
                    {
                        Id = tn.Id,
                        Name = tn.Name,
                        Description = tn.Description,
                        Icon = tn.Icon
                    }).ToListAsync();

                return Result<PagedResult<DanhSachTienNghiVm>>.Success("Lấy danh sách tiện nghi thanh công", new PagedResult<DanhSachTienNghiVm>
                {
                    TotalRecords = totalRecords,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data,
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách tiện nghi: " + ex.Message,ex);
            }
        }


        public async Task<Result<PagedResult<DanhSachTienNghiVm>>> GetAll(GetPagingRequest request)
        {
            try
            {
                var query = from g in _context.TienNghis
                            where string.IsNullOrEmpty(request.Keyword) || g.Name.Contains(request.Keyword)
                            select new DanhSachTienNghiVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Description = g.Description,
                                Type = g.loaiPhongs.FirstOrDefault().Type
                            };

                int totalRecords = await query.CountAsync();

                var items = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var pagedResult = new PagedResult<DanhSachTienNghiVm>
                {
                    TotalRecords = totalRecords,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Keyword = request.Keyword,
                    Items = items
                };

                return Result<PagedResult<DanhSachTienNghiVm>>.Success("Lấy danh sách tiện nghi thành công", pagedResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<DanhSachTienNghiVm>>.Error("Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<Result<int>> Create(TienNghiCreateRequest request)
        {
            try
            {
                _action = $"Thêm tiện nghi thành công";
                if (await _context.TienNghis.AnyAsync(x => x.Name == request.Name))
                    return Result<int>.Error($"Loại tiện nghi {request.Name} đã tồn tại!");

                var obj = new TienNghi()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Icon = request.Icon,
                    IsActive = true,
                    CreateAt = DateTime.Now,
                    CreateBy = "System",
                    ModifiedBy = "System",
                    LateModifiedDate = DateTime.Now,
                    IsDeleted = false
                };

                _context.TienNghis.Add(obj);
                var result = await _context.SaveChangesAsync();

                if(result > 0) 
                    return Result<int>.Success(result);
                return Result<int>.Error("Cập nhật thất bại");
            }
            catch(Exception ex)
            {
                return Result<int>.Error("Lỗi khi thêm tiện nghi" + ex.Message);
            }
        }

        public async Task<Result<int>> Update(TienNghiUpdateRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Id))
                    return Result<int>.Error("ID không hợp lệ!");

                int id = request.Id.DecodeId();

                var entity = await _context.TienNghis.FindAsync(id);
                if (entity == null)
                    return Result<int>.Error("Không tìm thấy loại phòng!");

                // Check trùng Name
                bool isDuplicate = await _context.TienNghis
                    .AnyAsync(x => x.Name == request.Name && x.Id != id && !x.IsDeleted);
                if (isDuplicate)
                    return Result<int>.Error("Tên tiện nghi đã tồn tại!");

                entity.Name = request.Name;
                entity.Description = request.Description;
                entity.Icon = request.Icon;
                entity.ModifiedBy = request.UserId.ToString();
                entity.LateModifiedDate = DateTime.Now;


                _context.TienNghis.Update(entity);
                var result = await SaveChange();

                return result > 0
                    ? Result<int>.Success("Cập nhật tiện nghi thành công", id)
                    : Result<int>.Error("Cập nhật thất bại!");
            }
            catch (Exception ex)
            {
                return Result<int>.Error("Lỗi hệ thống khi cập nhật tiện nghi");
            }
        }

        public async Task<Result<int>> Delete(DeleteRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                var obj = await _context.TienNghis.FindAsync(id);

                if (obj == null)
                    return Result<int>.Error("Không tìm thấy tiện nghi cần xóa!");

                obj.IsDeleted = true;

                obj.ModifiedBy = request.UserId.ToString();
                obj.LateModifiedDate = DateTime.Now;

                _context.TienNghis.Update(obj);
                var result = await SaveChange();

                return result > 0
                    ? Result<int>.Success("Xóa tiện nghi thành công", id)
                    : Result<int>.Error("Xóa thất bại!");
            }
            catch
            {

                return Result<int>.Error("Đã xảy ra lỗi khi xóa tiện nghi.");
            }
        }

        public async Task<TienNghiUpdateRequest> GetById(int id)
        {
            var tiennghi = await _context.TienNghis.FindAsync(id);
            if (tiennghi == null) return null;

            return new TienNghiUpdateRequest
            {
                Id = tiennghi.Id.EncodeId1(),
                Name = tiennghi.Name,
                Description = tiennghi.Description,
                Icon = tiennghi.Icon
            };
        }
        public async Task<Result<List<DanhSachTienNghiVm>>> GetAll()
        {
            try
            {
                var tienNghiList = await _context.TienNghis
                    .Where(t => !t.IsDeleted) // nếu có soft-delete
                    .Select(t => new DanhSachTienNghiVm
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        Type = t.loaiPhongs.FirstOrDefault().Type
                    })
                    .ToListAsync();

                return Result<List<DanhSachTienNghiVm>>.Success("Lấy danh sách tiện nghi thành công", tienNghiList);
            }
            catch (Exception ex)
            {
                return Result<List<DanhSachTienNghiVm>>.Error("Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}
