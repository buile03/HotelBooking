using Azure.Core;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DPKS.Common.Result;
using DPKS.Data.EF;
using DPKS.Data.Entites;
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
    public class PhongTestController : Controller
    {
        private readonly IPhongService _phongService;
        private readonly AppDbContext _context;
        public PhongTestController(IPhongService phongService, AppDbContext context)
        {
            _phongService = phongService;
            _context = context;
        }
        // GET: Phong/Index - Hiển thị trang chính với phân trang
        public async Task<IActionResult> Index(PhongSearchRequest request)
        {
            try
            {
                // Thiết lập giá trị mặc định cho phân trang
                if (request.PageIndex <= 0) request.PageIndex = 1;
                if (request.PageSize <= 0) request.PageSize = 10;

                var result = await _phongService.GetPagings(request);

                // Truyền request vào ViewBag để giữ lại các filter
                ViewBag.SearchRequest = request;

                return View(result);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách phòng: " + ex.Message;
                return View(new PagedResult<ThongTinDanhSachPhongVm>
                {
                    Items = new List<ThongTinDanhSachPhongVm>(),
                    TotalRecords = 0,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                });
            }
        }
        
        // GET: Phong/List - Hiển thị danh sách đầy đủ không phân trang
        public async Task<IActionResult> List(PhongSearchRequest request)
        {
            try
            {
                var result = await _phongService.GetAllPhongAsync(request);

                if (result.IsSuccessed )
                {
                    ViewBag.Message = result.Message;
                    ViewBag.SearchRequest = request;
                    return View(result.ResultObj);
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                    return View(new List<ThongTinDanhSachPhongVm>());
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải danh sách phòng: " + ex.Message;
                return View(new List<ThongTinDanhSachPhongVm>());
            }
        }

        // POST: Filter - Xử lý AJAX filter
        [HttpPost]
        public async Task<IActionResult> Filter(PhongSearchRequest request, string viewType = "index")
        {
            try
            {
                if (viewType.ToLower() == "list")
                {
                    var result = await _phongService.GetAllPhongAsync(request);
                    if (result.IsSuccessed)
                    {
                        return PartialView("_PhongListPartial", result.ResultObj);
                    }
                    return PartialView("_PhongListPartial", new List<ThongTinDanhSachPhongVm>());
                }
                else
                {
                    if (request.PageIndex <= 0) request.PageIndex = 1;
                    if (request.PageSize <= 0) request.PageSize = 10;

                    var result = await _phongService.GetPagings(request);
                    return PartialView("_PhongIndexPartial", result);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
