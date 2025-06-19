using DPKS.Common.Enum;
using DPKS.Common.Helper;
using DPKS.Common.Helper.DPKS.Common.Helper;
using DPKS.Model.ThanhToan.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using PayPalCheckoutSdk.Orders;
using Stripe.Checkout;
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
        private readonly IDatPhongService _datPhongService;
        private readonly IDanhMucService _danhMucService;
        private readonly MoMoHelper _momoHelper;
        private readonly VnPayHelper _vnPayHelper;

        public ThanhToanController(IThanhToanService thanhToanService
            , IDanhMucService danhMucService
            , IDatPhongService datPhongService
            , MoMoHelper moMoHelper
            , VnPayHelper vnPayHelper)
        {
            _thanhToanService = thanhToanService;
            _danhMucService = danhMucService;
            _datPhongService = datPhongService;
            _momoHelper = moMoHelper;
            _vnPayHelper = vnPayHelper;
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

        public async Task<IActionResult> ThanhToan (int datPhongId)
        {
            var datPhong = await _datPhongService.GetByIdAsync(datPhongId);

            if (datPhong == null)
                return NotFound();

            if (datPhong.DaThanhToan)
                return RedirectToAction("Success", new { Id = datPhongId });

            // Nếu thanh toán bằng tiền mặt thì xử lý ngay
            await _thanhToanService.ThanhToanTienMat(datPhongId);
            return RedirectToAction("Success", new { Id = datPhongId });
        }

        // Thanh toán bằng Stripe

        [HttpGet]
        public async Task<IActionResult> StripeCheckout(int datPhongId)
        {
            var domain = "https://localhost:44369";

            // Lấy thông tin đặt phòng từ DB
            var datPhong = await _datPhongService.GetByIdAsync(datPhongId);
            if (datPhong == null)
                return NotFound();

            var amount = datPhong.TongTien; // Giá tiền thật sự từ DB

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(amount * 100 / 23000), // Đổi VND sang cent USD
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"Thanh toán đặt phòng #{datPhongId}"
                    }
                },
                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = $"{domain}/ThanhToan/StripeSuccess?datPhongId={datPhongId}",
                CancelUrl = $"{domain}/ThanhToan/StripeCancel?datPhongId={datPhongId}"
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }

        [HttpGet]
        public async Task<IActionResult> PaypalCheckout(int datPhongId)
        {
            var datPhong = await _datPhongService.GetByIdAsync(datPhongId);
            if (datPhong == null) return NotFound();

            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
        {
            new PurchaseUnitRequest
            {
                AmountWithBreakdown = new AmountWithBreakdown
                {
                    CurrencyCode = "USD",
                    Value = ((datPhong.TongTien / 23000M).ToString("F2")) // VND → USD
                },
                Description = $"Thanh toán đặt phòng #{datPhongId}"
            }
        },
                ApplicationContext = new ApplicationContext
                {
                    ReturnUrl = Url.Action("PaypalSuccess", "ThanhToan", new { datPhongId }, Request.Scheme),
                    CancelUrl = Url.Action("PaypalCancel", "ThanhToan", new { datPhongId }, Request.Scheme)
                }
            });

            var client = PayPalHelper.Client;
            var response = await client.Execute(request);

            var result = response.Result<Order>();

            var approveLink = result.Links.FirstOrDefault(x => x.Rel == "approve")?.Href;

            return Redirect(approveLink); 
        }


        //Thanh toán bằng momo
        [HttpGet]
        public async Task<IActionResult> MoMoCheckout(int datPhongId)
        {
            var datPhong = await _datPhongService.GetByIdAsync(datPhongId);
            if (datPhong == null) return NotFound();

            string paymentUrl = await _momoHelper.CreatePaymentUrl(datPhong.TongTien, datPhongId.ToString());

            return Redirect(paymentUrl); // chuyển hướng đến cổng thanh toán MoMo
        }

        public async Task<IActionResult> StripeSuccess(int datPhongId)
        {
            await _thanhToanService.ThanhToanStripe(datPhongId);
            ViewBag.PhuongThuc = "Stripe";
            ViewBag.DatPhongId = datPhongId;
            return View("Success");
        }

        // Thanh toán bằng VnPay
        [HttpGet]
        public async Task<IActionResult> VnPayCheckout(int datPhongId)
        {
            var datPhong = await _datPhongService.GetByIdAsync(datPhongId);
            if (datPhong == null)
                return NotFound();

            var paymentUrl = _vnPayHelper.CreatePaymentUrl(datPhongId.ToString(), datPhong.TongTien);

            return Redirect(paymentUrl);
        }

        [HttpGet]
        public async Task<IActionResult> PaypalSuccess(int datPhongId, string token)
        {
            var request = new OrdersCaptureRequest(token);
            request.RequestBody(new OrderActionRequest());

            var client = PayPalHelper.Client;
            var response = await client.Execute(request);

            var result = response.Result<Order>();

            if (result.Status == "COMPLETED")
            {
                await _thanhToanService.ThanhToanPaypal(datPhongId); // bạn cần thêm hàm này
                ViewBag.PhuongThuc = "PayPal";
                ViewBag.DatPhongId = datPhongId;
                return View("Success");
            }

            return RedirectToAction("Cancel", new { Id = datPhongId });
        }

        public IActionResult StripeCancel(int datPhongId)
        {
            ViewBag.PhuongThuc = "Stripe";
            return View("Cancel");
        }

        public IActionResult PaypalCancel(int datPhongId)
        {
            ViewBag.PhuongThuc = "PayPal";
            ViewBag.DatPhongId = datPhongId;
            return View("Cancel");
        }

        public IActionResult TienMatSuccess(int datPhongId)
        {
            ViewBag.PhuongThuc = "Tiền mặt";
            ViewBag.DatPhongId = datPhongId;
            return View("Success");
        }



        [HttpGet]
        public IActionResult Success(int Id)
        {
            ViewBag.DatPhongId = Id;
            return View();
        }

        [HttpGet] 
        public IActionResult ChoThanhToan(int Id)
        {
            ViewBag.DatPhongId = Id;
            ViewBag.Message = "Đặt phòng thành công. Vui lòng thanh toán tại quầy khi nhận phòng.";
            return View();
        }

        [HttpGet]
        public IActionResult Cancel (int Id)
        {
            ViewBag.DatPhongId = Id;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MoMoSuccess(string orderId, string resultCode)
        {
            if (resultCode == "0")
            {
                await _thanhToanService.ThanhToanMoMo(int.Parse(orderId));
                ViewBag.PhuongThuc = "MoMo";
                return View("Success");
            }

            return RedirectToAction("Cancel");
        }

        [HttpPost]
        public IActionResult MoMoNotify()
        {
            // Đây là webhook – có thể xử lý thêm logic ở đây nếu cần
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> VnPayReturn()
        {
            var query = Request.Query;

            var vnp_ResponseCode = query["vnp_ResponseCode"];
            var vnp_TxnRef = query["vnp_TxnRef"];

            if (vnp_ResponseCode == "00")
            {
                // thanh toán thành công
                await _thanhToanService.ThanhToanVnPay(int.Parse(vnp_TxnRef));
                ViewBag.PhuongThuc = "VNPAY";
                return View("Success");
            }
            else
            {
                // thanh toán thất bại
                return View("Cancel");
            }
        }
    }
}
