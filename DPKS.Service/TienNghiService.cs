using DPKS.Common.Result;
using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Model.TienNghi;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface ITienNghiService
    {
        Task<Result<PagedResult<DanhSachTienNghiVm>>> GetAll(GetPagingRequest request);
        
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
    }
}
