using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPKS.Common;
using DPKS.Data.EF;
using DPKS.Data.Entities;
using DPKS.Model.Trackings;
using DPKS.Common.Result;

namespace DPKS.Service
{

    public interface ITrackingService
    {
        public Task<PagedResult<TrackingVm>> GetPagings(GetPagingRequest request);
        public Task Create(Guid userId, Result result);
    }
    public class TrackingService : BaseService, ITrackingService
    {
        private readonly AppDbContext _context;
        public TrackingService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Create(Guid userId, Result result)
        {
            try
            {
                var obj = new Tracking()
                {
                    ErorMessage = result.Message,
                    IsError = !result.IsSuccessed,
                    Time = DateTime.Now,
                    UserId = userId,
                };
                await _context.AddAsync(obj);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<PagedResult<TrackingVm>> GetPagings(GetPagingRequest request)
        {
            var query = from t in _context.Trackings
                        orderby t.Time descending
                        select new
                        {
                            t.Id,
                            t.User.HoTen,
                            t.UserId,
                            t.Action,
                            t.Time
                        };

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new TrackingVm()
                {
                    Id = x.Id,
                    NoiDung = x.Action,
                    NguoiThucHien = x.HoTen,
                    ThoiGian = x.Time
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<TrackingVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }
    }
}
