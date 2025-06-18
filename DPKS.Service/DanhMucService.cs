using DPKS.Common.Enum;
using DPKS.Data.EF;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface IDanhMucService
    {
        Task<List<SelectListItem>> GetDanhSachQuocGiaAsync();
        Task<List<SelectListItem>> GetDanhSachTinhTheoQuocGiaAsync(int quocGiaId);
    }
    public class DanhMucService : IDanhMucService
    {
        private readonly AppDbContext _context;

        public DanhMucService(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<List<SelectListItem>> GetDanhSachQuocGiaAsync()
        {
            var quocGias = await _context.QuocGias.OrderBy(q => q.Name).ToListAsync();
            return quocGias.Select(q => new SelectListItem
            {
                Value = q.Id.ToString(),
                Text = q.Name
            }).ToList();
        }
        public async Task<List<SelectListItem>> GetDanhSachTinhTheoQuocGiaAsync(int quocGiaId)
        {
            var tinhs = await _context.Tinhs
                .Where(t => t.QuocGiaId == quocGiaId)
                .OrderBy(t => t.Name)
                .ToListAsync();

            return tinhs
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList();
        }

        
    }
}
