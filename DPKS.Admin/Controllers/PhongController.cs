using DPKS.App.Extensions;
using DPKS.Common.Enum;
using DPKS.Common.Result;
using DPKS.Data.EF;
using DPKS.Model.Phong;
using DPKS.Model.Phong.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;
using DPKS.Data.Entites;
using DPKS.Common;

namespace DPKS.Admin.Controllers
{
    public class PhongController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IPhongService _phongService;
        private readonly ILogger<PhongController> _logger;
        private readonly ILoaiPhongService _loaiPhongServie;
        private readonly UserManager<ApplicationUser> _userManager;
        public PhongController(IUserService userService
            , IOrganizationService organizationService
            , IPhongService phongService
            , ILogger<PhongController> logger
            , ILoaiPhongService loaiPhongService
            , ITrackingService trackingService,
            UserManager<ApplicationUser> userManager) : base(userService, trackingService, logger)
        {
            _phongService = phongService;
            _logger = logger;
            _loaiPhongServie = loaiPhongService;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index(PhongSearchRequest request)
        {
            if (request == null)
            {
                request = new PhongSearchRequest(); // đảm bảo không null
            }

            return View(request);
        }

        public async Task<IActionResult> List (PhongSearchRequest request)
        {
            try
            {
                var result = await _phongService.GetPagings(request); 
                return PartialView(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi lấy danh sách phòng");
                return PartialView(new PagedResult<ThongTinDanhSachPhongVm>());
                
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            var phong = await _phongService.GetPhongById(id);
            if (phong == null)
            {
                return NotFound();
            }
            return View(phong);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.TrangThaiPhongItems = enHelper.GetSelectListTrangThaiPhong();
                ViewBag.LoaiGiuongItems = enHelper.GetSelectListLoaiGiuong();
                ViewBag.LoaiViewItems = enHelper.GetSelectListLoaiView();

                // Sử dụng service cho LoaiPhong
                ViewBag.LoaiPhongItems = await _loaiPhongServie.GetAllForDropdown();
                return PartialView();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi load form tạo phòng");
                return PartialView();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhongCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    _logger.LogWarning("ModelState invalid: " + string.Join(", ", errors));
                    return BadRequest(ModelState);
                }

               
                return await ActionResult(await _phongService.Create(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi tạo phòng");
                return ErrorResult();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                int decodedId = id.DecodeId();

                ViewBag.TrangThaiPhongItems = enHelper.GetSelectListTrangThaiPhong();
                ViewBag.LoaiGiuongItems = enHelper.GetSelectListLoaiGiuong();
                ViewBag.LoaiViewItems = enHelper.GetSelectListLoaiView();

                // Sử dụng service cho LoaiPhong
                ViewBag.LoaiPhongItems = await _loaiPhongServie.GetAllForDropdown();

                var result = await _phongService.GetPhongById(decodedId);
                if (!result.IsSuccessed || result.ResultObj == null)
                {
                    _logger.LogWarning("Không tìm thấy thông tin phòng với ID: {Id}", id);
                    return PartialView(); // hoặc return NotFound();
                }

                var data = result.ResultObj;

                var model = new PhongUpdateRequest
                {
                    Id = id,
                    SoPhong = data.SoPhong,
                    Gia = data.Gia,
                    loaiGiuong = data.LoaiGiuong ??enLoaiGiuong.KHONGRO,
                    loaiView = data.LoaiView ?? enLoaiView.KHONGCO,
                    LoaiPhongId = data.LoaiPhongId, // bạn cần thêm vào ChiTietPhongVm
                    TrangThaiPhongId = data.TrangThaiPhongId, // bạn cần thêm vào ChiTietPhongVm
                    SoNguoiLonToiDa = data.SoLuongKhach, // có thể cần sửa nếu lấy theo TienNghi
                    SoTreEmToiDa = 0 // bạn có thể cập nhật chính xác nếu có trong ChiTietPhongVm
                };

                return PartialView(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi gọi Edit phòng");
                return PartialView();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit(PhongUpdateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return IsValidResult();

                request.UserId = User.GetUserId();

                return await ActionResult(await _phongService.Update(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
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
                    return IsValidResult();

                request.UserId = User.GetUserId();
                return await ActionResult(await _phongService.Delete(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
                return ErrorResult();
            }
        }

    }
}
