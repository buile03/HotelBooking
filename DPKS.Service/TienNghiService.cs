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
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface ITienNghiService
    {
        Task<Result<List<TienNghiVm>>> GetAll(TienNghiCreateRequest request);
        
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
        public string GetDomain()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host.Value}";
        }

        public async Task<Result<List<TienNghiVm>>> GetAll(TienNghiCreateRequest request)
        {
            try
            {
                var query = from g in _context.TienNghis
                            select new TienNghiVm
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Description = g.Description
                            };
                if (!await query.AnyAsync())
                    return Result<List<TienNghiVm>>.Error("Không có dữ liệu để hiển thị");
                return Result<List<TienNghiVm>>.Success($"Hiển thị {await query.CountAsync()} tiện nghi", await query.ToListAsync());
            }
            catch
            {
                throw;
            }
        }
    }
}
