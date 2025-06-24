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
using DPKS.Admin.Models;
using DPKS.Common.Helper;

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
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var obj = await _phongService.GetById(id);
                if (obj == null)
                {
                    return NotFound();
                }

                // Tạo request object với Id đã được encode
                var request = new PhongUpdateRequest
                {
                    Id = id.EncodeId1(), // Encode id thành string
                    SoPhong = obj.SoPhong,
                    Gia = obj.Gia,
                    loaiGiuong = obj.loaiGiuong,
                    loaiView = obj.loaiView,
                    LoaiPhongId = obj.LoaiPhongId,
                    TrangThaiPhongId = obj.TrangThaiPhongId,
                    SoNguoiLonToiDa = obj.SoNguoiLonToiDa,
                    SoTreEmToiDa = obj.SoTreEmToiDa
                };

                ViewBag.TrangThaiPhongItems = enHelper.GetSelectListTrangThaiPhong();
                ViewBag.LoaiGiuongItems = enHelper.GetSelectListLoaiGiuong();
                ViewBag.LoaiViewItems = enHelper.GetSelectListLoaiView();
                ViewBag.LoaiPhongItems = await _loaiPhongServie.GetAllForDropdown();

                return PartialView(request); // Truyền request thay vì obj
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi load form sửa phòng");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PhongUpdateRequest request)
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
                return await ActionResult(await _phongService.Update(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật phòng");
                return ErrorResult();
            }
        }



        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var roomId = id.DecodeId();
            var room = await _context.Phongs
                .Where(x => x.PhongId == roomId && !x.IsDeleted)
                .Select(x => new DeleteRequest
                {
                    Id = id,
                    
                }).FirstOrDefaultAsync();

            if (room == null)
                return Content("Không tìm thấy phòng.");

            return PartialView("_Delete", room);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { isSuccessed = false, message = "Dữ liệu không hợp lệ" });

                request.UserId = User.GetUserId(); // hàm extension để lấy id người dùng

                var result = await _phongService.Delete(request);
                return Json(new
                {
                    isSuccessed = result.IsSuccessed,
                    message = result.Message,
                    
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra");
                return ErrorResult();
            }
        }

    }
}
