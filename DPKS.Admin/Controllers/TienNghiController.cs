using DPKS.App.Extensions;
using DPKS.Common.Result;
using DPKS.Model.LoaiPhong;
using DPKS.Model.TienNghi;
using DPKS.Model.TienNghi.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DPKS.Admin.Controllers
{
    public class TienNghiController : BaseController
    {
        private readonly ITienNghiService _tienNghiService;
        private readonly ILogger<TienNghiController> _logger;
        private static readonly List<(string DisplayName, string CssClass)> HotelIconList = new()
        {
            ("Wi-Fi", "fa fa-wifi"),
            ("TV", "fa fa-tv"),
            ("Bồn tắm", "fa fa-bath"),
            ("Giường ngủ", "fa fa-bed"),
            ("Chuông lễ tân", "fa fa-concierge-bell"),
            ("Hồ bơi", "fa fa-swimmer"),
            ("Cà phê", "fa fa-coffee"),
            ("Nhà hàng", "fa fa-utensils"),
            ("Bãi đỗ xe", "fa fa-parking"),
            ("Vòi sen", "fa fa-shower"),
            ("Máy lạnh", "fa fa-snowflake"),
            ("Nhiệt độ", "fa fa-thermometer-half"),
            ("Thân thiện người khuyết tật", "fa fa-wheelchair"),
            ("Thang máy", "fa fa-elevator"),
            ("Khách sạn", "fa fa-hotel"),
            ("Khóa cửa", "fa fa-lock"),
            ("Chìa khóa", "fa fa-key"),
            ("Điện thoại", "fa fa-phone"),
            ("Laptop", "fa fa-laptop"),
            ("Cocktail", "fa fa-glass-martini"),
            ("Khu vực hút thuốc", "fa fa-smoking"),
            ("Không hút thuốc", "fa fa-smoking-ban"),
            ("Máy xay sinh tố", "fa fa-blender"),
            ("Quạt", "fa fa-fan"),
            ("Cốc nóng", "fa fa-mug-hot"),
            ("Bình chữa cháy", "fa fa-fire-extinguisher"),
            ("Camera", "fa fa-camera"),
            ("Camera cổ điển", "fa fa-camera-retro"),
            ("TV cổ", "fa fa-tv-retro"),
            ("Âm lượng", "fa fa-volume-up"),
            ("Tắt tiếng", "fa fa-volume-off"),
            ("Cửa mở", "fa fa-door-open"),
            ("Cửa đóng", "fa fa-door-closed"),
            ("Sofa", "fa fa-couch"),
            ("Ghế", "fa fa-chair"),
            ("Chổi", "fa fa-broom"),
            ("Thùng rác", "fa fa-trash"),
            ("Bóng đèn", "fa fa-lightbulb"),
            ("Mic", "fa fa-microphone"),
            ("Mic khác", "fa fa-microphone-alt"),
            ("Tai nghe", "fa fa-headphones"),
            ("Xe đẩy hành lý", "fa fa-luggage-cart"),
            ("Chuông", "fa fa-bell"),
            ("Chuông dịch vụ", "fa fa-bell-concierge"),
            ("Bãi biển", "fa fa-umbrella-beach"),
            ("Nước", "fa fa-water"),
            ("Gió", "fa fa-wind"),
            ("Điện", "fa fa-bolt"),
            ("Bồn rửa", "fa fa-sink"),
            ("Toilet", "fa fa-toilet"),
            ("Rửa tay", "fa fa-hands-wash"),
            ("Lịch", "fa fa-calendar-check"),
            ("Địa điểm", "fa fa-map-marker-alt"),
            ("Bản đồ", "fa fa-map"),
            ("Chỉ đường", "fa fa-location-arrow"),
            ("La bàn", "fa fa-compass"),
            ("Giường phụ", "fa fa-bed-alt"),
            ("Nệm & gối", "fa fa-mattress-pillow"),
            ("Bồn tắm nóng", "fa fa-hot-tub"),
            ("Áo tắm", "fa fa-tshirt"),
            ("Xà phòng", "fa fa-soap"),
            ("Xịt phòng", "fa fa-spray-can"),
            ("Đồng hồ", "fa fa-clock"),
            ("Đã kiểm tra", "fa fa-check-circle"),
            ("Cấm", "fa fa-ban"),
            ("Thông tin", "fa fa-info-circle"),
            ("Cảnh báo", "fa fa-exclamation-triangle"),
            ("Người dùng", "fa fa-users"),
            ("Bạn bè", "fa fa-user-friends"),
            ("Bảo vệ", "fa fa-user-shield"),
            ("Em bé", "fa fa-baby"),
            ("Trẻ em", "fa fa-child"),
            ("Phòng vệ sinh", "fa fa-restroom"),
            ("Lăn sơn", "fa fa-paint-roller"),
            ("Chống muỗi", "fa fa-mosquito"),
            ("Khiên bảo vệ", "fa fa-shield-alt"),
            ("Vân tay", "fa fa-fingerprint"),
            ("Rửa tay sát khuẩn", "fa fa-hand-sparkles"),
            ("Chứng nhận", "fa fa-certificate"),
            ("Giải thưởng", "fa fa-award"),
            ("Ngôi sao", "fa fa-star"),
            ("Nửa sao", "fa fa-star-half-alt"),
            ("Trái tim", "fa fa-heart"),
            ("Tim đập", "fa fa-heartbeat"),
            ("Quà tặng", "fa fa-gift"),
            ("Thẻ giá", "fa fa-tag"),
            ("Nhãn dán", "fa fa-tags"),
            ("Mã vạch", "fa fa-barcode"),
            ("Thẻ tín dụng", "fa fa-credit-card"),
            ("Tiền mặt", "fa fa-money-bill"),
            ("Máy tính tiền", "fa fa-cash-register"),
            ("Hóa đơn", "fa fa-receipt"),
            ("Ngôn ngữ", "fa fa-language")
        };

        public TienNghiController(ITienNghiService tienNghiService
            , IUserService userService
            , ILogger<TienNghiController> logger
            , ITrackingService trackingService
            ) : base(userService, trackingService,  logger)
        {
            _tienNghiService = tienNghiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(TienNghiSearchRequest request)
        {
            if (request == null)
            {
                request = new TienNghiSearchRequest();
            }
            return View(request);
        }
        public async Task<IActionResult> List(TienNghiSearchRequest request)
        {
            try
            {
                var result = await _tienNghiService.GetPaging(request);

                return PartialView(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi lấy danh sách tiện nghi");
                return PartialView(new PagedResult<DanhSachTienNghiVm>());
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.IconList = HotelIconList;
                return PartialView();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi load form tạo tiện nghi");
                return PartialView();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TienNghiCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    _logger.LogWarning("ModelState invalid: " + string.Join(", ", errors));
                    return BadRequest(ModelState);
                }



                var result = await _tienNghiService.Create(request);

                return await ActionResult(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi khi tạo tiện nghi");
                return ErrorResult();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit (int id)
        {
            try
            {
                var obj = await _tienNghiService.GetById(id);
                if (obj == null)
                {
                    return NotFound();
                }

                ViewBag.IconList = HotelIconList;
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi load form sửa tiện nghi");
                return StatusCode(500);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TienNghiUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}");
                    _logger.LogWarning("ModelState Invalid: " + string.Join(" | ", errors));
                    return BadRequest();
                }

                request.UserId = User.GetUserId();
                var result = await _tienNghiService.Update(request);
                return await ActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật tiện nghi");
                return ErrorResult();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { isSuccessed = false, message = "Dữ liệu không hợp lệ" });

                request.UserId = User.GetUserId();

                var result = await _tienNghiService.Delete(request);
                return Json(new
                {
                    isSuccessed = result.IsSuccessed,
                    message = result.Message,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi xóa tiện nghi");
                return ErrorResult();
            }
        }


    }
}
