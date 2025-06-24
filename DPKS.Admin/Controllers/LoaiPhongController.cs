using DPKS.App.Extensions;
using DPKS.Common.Enum;
using DPKS.Common.Result;
using DPKS.Data.EF;
using DPKS.Model.LoaiPhong;
using DPKS.Model.LoaiPhong.Request;
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
using DPKS.Model.Phong.Request;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.VariantTypes;

namespace DPKS.Admin.Controllers
{
    public class LoaiPhongController : BaseController
    {
        
        private readonly ILoaiPhongService _loaiPhongService;
        private readonly ILogger<LoaiPhongController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;
        public LoaiPhongController(
            IUserService userService,
            ITrackingService trackingService,
            ILoaiPhongService loaiPhongService,
            ILogger<LoaiPhongController> logger,
            IWebHostEnvironment webHostEnvironment,
            AppDbContext context
            )
            : base(userService, trackingService, logger) // truyền cùng logger lên base
        {
            _loaiPhongService = loaiPhongService;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public async Task<IActionResult> Index(LoaiPhongSearchRequest request)
        {
            return View(request);
        }
        public async Task<IActionResult> List(LoaiPhongSearchRequest request)
        {
            try
            {
                var result = await _loaiPhongService.GetPaging(request);
                
                return PartialView(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi lấy danh sách loại phòng");
                return PartialView(new PagedResult<ThongTinLoaiPhongVm>());
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create(int id )
        {
            ViewBag.AnhPhu = await _loaiPhongService.GetAnhLoaiPhong(id);

            var model = new LoaiPhongCreateRequest
            {
                Id = id
            };

            return PartialView(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoaiPhongCreateRequest request)
        {
            try
            {
                string fileName = "default.jpg";

                if (request.HinhAnhUpload != null && request.HinhAnhUpload.Length > 0)
                {
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "loaiphong");
                    Directory.CreateDirectory(uploadFolder); // đảm bảo thư mục tồn tại

                    string extension = Path.GetExtension(request.HinhAnhUpload.FileName);
                    fileName = $"{Path.GetFileNameWithoutExtension(request.HinhAnhUpload.FileName)}_{Guid.NewGuid():N}{extension}";
                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.HinhAnhUpload.CopyToAsync(stream);
                    }
                }

                
                request.HinhAnhChinh = fileName;

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage);
                    _logger.LogWarning("ModelState invalid: " + string.Join(", ", errors));
                    return BadRequest(ModelState);
                }

                var result = await _loaiPhongService.Create(request);
                if (result.IsSuccessed)
                {
                    return Json(new
                    {
                        isSuccessed = true,
                        message = result.Message,
                        loaiPhongId = result.ResultObj
                    });
                }
                return await ActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi tạo loại phòng");
                if (ex.InnerException != null)
                {
                    _logger.LogInformation($"Type: {request.Type}, Length: {request.Type?.Length}");
                }
                return ErrorResult();
            }
        }

        public async Task<IActionResult> AddAnhPhu(int id)
        {
            var anhPhu = await _loaiPhongService.GetAnhLoaiPhong(id);
            ViewBag.LoaiPhongId = id;
            return PartialView("AddAnhPhu", anhPhu);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _loaiPhongService.GetById(id);
            if (entity == null)
                return NotFound();

            ViewBag.LoaiPhongId = id;
            ViewBag.AnhPhu = await _loaiPhongService.GetAnhLoaiPhong(id);
            return PartialView(entity);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(LoaiPhongUpdateRequest request)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                  .Select(e => e.ErrorMessage)
                                  .ToList();
                    return BadRequest(new { success = false, errors });
                } 
                
                request.UserId = User.GetUserId();
                return await ActionResult(await _loaiPhongService.Update(request));
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra trong quá trình xử lý!" });
            }
        }
        
        
        [HttpPost]
        public async Task<IActionResult> AddAnhLoaiPhong(AddAnhLoaiPhongRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                request.UserId = User.GetUserId();

                foreach (var file in request.ImageFile)
                {
                    if (file == null || file.Length == 0)
                        continue;

                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "loaiphong");
                    var filePath = Path.Combine(uploadPath, fileName);

                    Directory.CreateDirectory(uploadPath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Gọi service lưu ảnh
                    await _loaiPhongService.AddAnhLoaiPhong(request.LoaiPhongId, fileName, request.UserId);
                }

                // Trả lại danh sách ảnh mới
                return Json(new
                {
                    success = true,
                    message = "Thêm ảnh thành công!",
                    loaiPhongId = request.LoaiPhongId
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi thêm ảnh!" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAnhLoaiPhong(int photoId)
        {
            try
            {
                var userId = User.GetUserId();
                var result = await _loaiPhongService.DeleteAnhLoaiPhong(photoId, userId);

                return Json(new { success = result.IsSuccessed, message = result.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi xóa ảnh!" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetHinhAnhChinh(SetMainPhotoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                request.UserId = User.GetUserId();
                var result = await _loaiPhongService.SetHinhAnhChinh(request.LoaiPhongId, request.PhotoId, request.UserId);

                if (result.IsSuccessed)
                {
                    return Json(new
                    {
                        success = true,
                        message = result.Message,
                        data = await _loaiPhongService.GetAnhLoaiPhong(request.LoaiPhongId)
                    });
                }

                return Json(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi đặt ảnh chính!" });
            }
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var lpId = id.DecodeId();
            var lp = await _context.LoaiPhongs
                .Where(x => x.Id == lpId && !x.IsDeleted)
                .Select(x => new DeleteRequest
                {
                    Id = id,

                }).FirstOrDefaultAsync();

            if (lp == null)
                return Content("Không tìm thấy phòng.");

            return PartialView("_Delete", lp);
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

                var result = await _loaiPhongService.Delete(request);
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
