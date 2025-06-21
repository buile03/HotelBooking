using DPKS.Common.Enum;
using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Model.DatPhong.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DPKS.APP.Controllers
{
    [Authorize]
    public class DatPhongController : Controller
    {
        private readonly IDatPhongService _datPhongService;
        private readonly IPhongService _phongService;
        private readonly IThanhToanService _thanhToanService;
        private readonly AppDbContext _context;


        public DatPhongController(AppDbContext context
            , IDatPhongService datPhongService
            , IPhongService phongService
            , IThanhToanService thanhToanService)
        {
            _datPhongService = datPhongService;
            _context = context;
            _phongService = phongService;
            _thanhToanService = thanhToanService;
        }

        // GET: /DatPhong/Create/{id}
        public async Task<IActionResult> Create(int id)
        {
            var phongrs = await _phongService.GetPhongById(id);
            if (phongrs == null || phongrs.ResultObj == null)
                return NotFound();

            var phong = phongrs.ResultObj;
            var model = new DatPhongCreateRequest
            {
                PhongId = phong.PhongId,
                Gia1Dem = phong.Gia
            };

            ViewBag.PhuongThucThanhToanList = await _thanhToanService.GetAll();
            return View(model);
        }

        // POST: /DatPhong/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DatPhongCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PhuongThucThanhToanList = await _thanhToanService.GetAll();
                return View(request);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            request.UserId = int.Parse(userIdClaim.Value);

            try
            {
                var datPhongId = await _datPhongService.DatPhongAsync(request);

                return request.PhuongThucThanhToanId switch
                {
                    (int)enLoaiThanhToan.TienMat => RedirectToAction("ChoThanhToan", "ThanhToan", new { id = datPhongId }),
                    (int)enLoaiThanhToan.Stripe => RedirectToAction("StripeCheckout", "ThanhToan", new { datPhongId }),
                    (int)enLoaiThanhToan.PayPal => RedirectToAction("PaypalCheckout", "ThanhToan", new { datPhongId }),
                    (int)enLoaiThanhToan.Momo => RedirectToAction("MoMoCheckout", "ThanhToan", new { datPhongId }),
                    (int)enLoaiThanhToan.VNPay => RedirectToAction("VnPayCheckout", "ThanhToan", new { datPhongId }),
                    _ => RedirectToAction("ThanhToan", "ThanhToan", new { datPhongId })
                };
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.PhuongThucThanhToanList = await _thanhToanService.GetAll();
                return View(request);
            }
        }

        // POST: /DatPhong/CapNhatTrangThai
        [HttpPost]
        public async Task<IActionResult> CapNhatTrangThai(int id, enTrangThaiDatPhong trangThaiMoi)
        {
            var result = await _datPhongService.CapNhatTrangThaiDatPhongAsync(id, trangThaiMoi);
            if (!result)
                return BadRequest("Không thể cập nhật trạng thái.");

            return RedirectToAction("ChiTiet", new { id });
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var bookings = await _datPhongService.GetListByUserIdAsync(userId);
            return View(bookings);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            await _datPhongService.HuyDatPhongAsync(id);
            return RedirectToAction("Index");
        }
        

        //Hiển thị thông tin chi tiết đơn đặt phòng.
        public async Task<IActionResult> Details (int Id)
        {
            var datphong = await _datPhongService.GetByIdAsync(Id);
            if(datphong == null)
            {
                return NotFound();
            }
            return View(datphong);
        }

        
    }
}
