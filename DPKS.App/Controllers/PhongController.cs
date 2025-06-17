using Azure.Core;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DPKS.Common.Result;
using DPKS.Common.System;
using DPKS.Data.EF;
using DPKS.Model.Phong;
using DPKS.Model.Phong.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.APP.Controllers
{
    public class PhongController : Controller
    {
        private readonly IPhongService _phongService;
        private readonly AppDbContext _context;
        private readonly ILogger<PhongController> _logger;
        public PhongController(IPhongService phongService, AppDbContext context, ILogger<PhongController> logger)
        {
            _phongService = phongService;
            _context = context;
            _logger = logger;
        }

        
        public async Task<IActionResult> Index(PhongSearchRequest request)
        {
            // Chuẩn bị dữ liệu cho các dropdown và bộ lọc
            var loaiPhongs = await _context.LoaiPhongs.ToListAsync();
            ViewBag.LoaiPhong = loaiPhongs.Select(lp => new { lp.Id, lp.Type }).ToList();

            var tienNghiList = await _context.TienNghis.Select(tn => tn.Name).ToListAsync();
            ViewBag.TienNghiList = tienNghiList;

            
            return View(request);
        }


        public async Task<IActionResult> List(PhongSearchRequest request)
        {
            try
            {
                var data = await _phongService.GetPagings(request);
                return PartialView(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi lấy danh sách phòng");
                return PartialView(new PagedResult<ThongTinDanhSachPhongVm>());
            }
        }


        [HttpPost]
        public async Task<IActionResult> Search(PhongSearchRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

                ViewBag.ErrorMessage = "Dữ liệu tìm kiếm không hợp lệ." + string.Join("; ", errors);
                // Tải danh sách loại phòng
                ViewBag.LoaiPhong = await _context.LoaiPhongs.Select(lp => new { lp.Id, lp.Type }).ToListAsync();
                // Tải danh sách tiện nghi
                ViewBag.TienNghiList = await _context.TienNghis.Select(tn => tn.Name).ToListAsync();
                return View("List", new List<ThongTinDanhSachPhongVm>());
            }

            var result = await _phongService.GetAvailablePhongsAsync(request);
            ViewBag.SearchRequest = request;
            ViewBag.LoaiPhong = await _context.LoaiPhongs.Select(lp => new { lp.Id, lp.Type }).ToListAsync();
            ViewBag.TienNghiList = await _context.TienNghis.Select(tn => tn.Name).ToListAsync();

            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = result.Message;
                return View("List", new List<ThongTinDanhSachPhongVm>());
            }

            return View("List", result.ResultObj);
        }


        // Xem chi tiết phòng
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _phongService.GetPhongById(id);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }
            return View(result.ResultObj);
        }

        

        // Tính tổng giá (dành cho form trong chi tiết phòng)
        [HttpPost]
        public async Task<IActionResult> CalculatePrice([FromBody] CalculatePriceRequest request)
        {
            try
            {
                var result = await _phongService.CalculateTotalPriceAsync(
                    request.PhongId,
                    DateTime.Parse(request.NgayNhanPhong),
                    DateTime.Parse(request.NgayTraPhong)
                );

                if (!result.IsSuccessed)
                {
                    return Json(new { success = false, message = result.Message });
                }

                return Json(new { success = true, totalPrice = result.ResultObj });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
    }
}
