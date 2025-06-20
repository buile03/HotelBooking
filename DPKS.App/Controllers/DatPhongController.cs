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


        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var phongrs = await _phongService.GetPhongById(id);
            
            if (phongrs == null)
            {
                return NotFound();
            }

            var phong = phongrs.ResultObj;
            var model = new DatPhongCreateRequest
            {
                PhongId = phong.PhongId,
                Gia1Dem = phong.Gia 
            };

            ViewBag.PhuongThucThanhToanList =  enHelper.GetSelectListPhuongThuc();

            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DatPhongCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PhuongThucThanhToanList = enHelper.GetSelectListPhuongThuc();
                return View(request);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                // Gửi người dùng về trang login nếu chưa đăng nhập
                return RedirectToAction("Login", "Account");
            }
            request.UserId = int.Parse(userIdClaim.Value);

            try
            {
                var datPhongId = await _datPhongService.DatPhongAsync(request);

                //Nếu thanh toán bằng tiền mặt thì xác nhận thành công
                if (request.PhuongThucThanhToanId == (int)enLoaiThanhToan.TienMat)
                {
                    //await _thanhToanService.ThanhToanTienMat(datPhongId);
                    return RedirectToAction("ChoThanhToan", "ThanhToan", new { id = datPhongId });
                }
                else if (request.PhuongThucThanhToanId == (int)enLoaiThanhToan.Stripe)
                {
                    // Gọi tới StripeCheckout action để xử lý
                    return RedirectToAction("StripeCheckout", "ThanhToan", new { datPhongId = datPhongId });
                }

                else if (request.PhuongThucThanhToanId == (int)enLoaiThanhToan.PayPal)
                {
                    // Gọi tới StripeCheckout action để xử lý
                    return RedirectToAction("PaypalCheckout", "ThanhToan", new { datPhongId = datPhongId });
                }

                else if (request.PhuongThucThanhToanId == (int)enLoaiThanhToan.Momo)
                {
                    // Gọi tới StripeCheckout action để xử lý
                    return RedirectToAction("MoMoCheckout", "ThanhToan", new { datPhongId = datPhongId });
                }
                else if (request.PhuongThucThanhToanId == (int)enLoaiThanhToan.VNPay)
                {
                    // Gọi tới StripeCheckout action để xử lý
                    return RedirectToAction("VnPayCheckout", "ThanhToan", new { datPhongId = datPhongId });
                }

                // Nếu là online: chuyển sang controller thanh toán
                return RedirectToAction("ThanhToan", "ThanhToan", new { datPhongId = datPhongId });
                

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.PhuongThucThanhToanList = enHelper.GetSelectListPhuongThuc();
                return View(request);
            }
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
