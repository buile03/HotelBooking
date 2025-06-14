using Azure.Core;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
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
        public PhongController(IPhongService phongService, AppDbContext context)
        {
            _phongService = phongService;
            _context = context;
        }

        //Hiển thị danh sách phòng
        public async Task<IActionResult> Index()
        {
            var request = new PhongSearchRequest();
            var result = await _phongService.GetAllPhongAsync(request);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(new List<ThongTinDanhSachPhongVm>());
            }
            return View(result.ResultObj);
        }

        public async Task<IActionResult> List()
        {
            var request = new PhongSearchRequest();
            var result = await _phongService.GetAllPhongAsync(request);
            ViewBag.SearchRequest = request;
            ViewBag.LoaiPhong = await _context.LoaiPhongs.Select(lp => new { lp.Id, lp.Type }).ToListAsync();
            ViewBag.TienNghiList = await _context.TienNghis.Select(tn => tn.Name).ToListAsync();

            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(new List<ThongTinDanhSachPhongVm>());
            }

            return View(result.ResultObj);
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


        // Tìm kiếm phòng trống
        //[HttpPost]
        //public async Task<IActionResult> Search(PhongSearchRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.ErrorMessage = "Dữ liệu tìm kiếm không hợp lệ.";
        //        return View("Index", new List<ThongTinDanhSachPhongVm>());
        //    }

        //    var result = await _phongService.GetAvailablePhongsAsync(request);
        //    if (!result.IsSuccessed)
        //    {
        //        ViewBag.ErrorMessage = result.Message;
        //        return View("Index", new List<ThongTinDanhSachPhongVm>());
        //    }

        //    ViewBag.SearchRequest = request; // Lưu request để hiển thị lại trong form
        //    return View("Index", result.ResultObj);
        //}
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

        //// Hiển thị danh sách loại phòng
        //public async Task<IActionResult> LoaiPhong()
        //{
        //    var result = await _phongService.GetAllLoaiPhongAsync();
        //    if (!result.Success)
        //    {
        //        ViewBag.ErrorMessage = result.Message;
        //        return View(new List<LoaiPhongVm>());
        //    }
        //    return View(result.Data);
        //}

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
