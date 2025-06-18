using DPKS.Common.Enum;
using DPKS.Model.ThanhToan.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.APP.Controllers
{

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
                Gia = tongTien,
                ThoiDiemThanhToan = DateTime.Now
            };
            ViewBag.PhuongThucThanhToanList = enHelper.GetSelectListPhuongThuc(); // enum -> dropdown
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ThanhToanCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PhuongThucThanhToanList = enHelper.GetSelectListPhuongThuc();
                return View(request);
            }

            var result = await _thanhToanService.Create(request);
            if (result)
                return RedirectToAction("Index", "DatPhong");

            ModelState.AddModelError("", "Thanh toán không thành công hoặc đã tồn tại.");
            ViewBag.PhuongThucThanhToanList = enHelper.GetSelectListPhuongThuc();
            return View(request);
        }

        
    }
}
