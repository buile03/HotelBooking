using DPKS.Common.Enum;
using DPKS.Model.ThanhToan.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.APP.Controllers
{
    [Authorize]
    public class ThanhToanController : Controller
    {
        private readonly IThanhToanService _thanhToanService;
        private readonly IDanhMucService _danhMucService;

        public ThanhToanController(IThanhToanService thanhToanService, IDanhMucService danhMucService)
        {
            _thanhToanService = thanhToanService;
            _danhMucService = danhMucService;
        }

        // Hiển thị form thanh toán cho một đặt phòng cụ thể
        [HttpGet]
        public IActionResult Create(int datPhongId, decimal tongTien)
        {
            var model = new ThanhToanCreateRequest
            {
                DatPhongId = datPhongId,
                PhuongThucThanhToanId = 1, // Tiền mặt
                Gia = tongTien,
                ThoiDiemThanhToan = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThanhToanCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                //ViewBag.PhuongThucThanhToanList = enHelper.GetSelectListPhuongThuc();
                return View(request);
            }

            var result = await _thanhToanService.Create(request);
            if (!result)
            {
                ModelState.AddModelError("", "Không thể thanh toán. Vui lòng thử lại.");
                return View(request);
            }

            //ViewBag.PhuongThucThanhToanList = enHelper.GetSelectListPhuongThuc();
            return RedirectToAction("Success");
        }
        [HttpGet]
        public async Task<IActionResult> Details (int Id)
        {
            var thanhtoan = await _thanhToanService.GetByDatPhongId(Id);
            if (thanhtoan == null)
                return NotFound();

            return View(thanhtoan);
               
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}
