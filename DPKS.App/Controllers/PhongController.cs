using Azure.Core;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DPKS.Model.Phong;
using DPKS.Model.Phong.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPKS.APP.Controllers
{
    //public class PhongController : Controller
    //{
    //    private readonly IPhongService _phongService;
    //    private readonly ILogger<PhongController> _logger;

    //    public PhongController(IPhongService phongService, ILogger<PhongController> logger)
    //    {
    //        _phongService = phongService;
    //        _logger = logger;
    //    }

    //    public async Task<IActionResult> Index(PhongSearchRequest request)
    //    {
    //        // Nếu có dropdown lọc loại phòng thì truyền thêm ViewBag tại đây
    //        return View(request);
    //    }

    //    public async Task<IActionResult> List(PhongSearchRequest request)
    //    {
    //        try
    //        {
    //            var result = await _phongService.GetAllPhongAsync(request);

    //            if (!result.IsSuccessed)
    //            {
    //                ViewBag.ErrorMessage = result.Message;
    //                return PartialView(new List<ThongTinDanhSachPhongVm>());
    //            }

    //            return PartialView(result.ResultObj);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Lỗi khi lấy danh sách phòng");
    //            return PartialView(new List<ThongTinDanhSachPhongVm>());
    //        }
    //    }
    //}
    public class PhongController : Controller
    {
        private readonly IPhongService _phongService;

        public PhongController(IPhongService phongService)
        {
            _phongService = phongService;
        }

        // GET: /Phong
        public async Task<IActionResult> Index(PhongSearchRequest request)
        {
            try
            {
                var result = await _phongService.GetAllPhongAsync(request);

                if (!result.IsSuccessed)
                {
                    ViewBag.ErrorMessage = result.Message;
                    return View(new List<ThongTinDanhSachPhongVm>());
                }

                ViewBag.SuccessMessage = result.Message;
                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra khi tải dữ liệu: " + ex.Message;
                return View(new List<ThongTinDanhSachPhongVm>());
            }
        }
    }
}
