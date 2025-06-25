using DPKS.App.Extensions;
using DPKS.Common.Result;
using DPKS.Model.User.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DPKS.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IDanhMucService _danhMucService;
        public UserController(IUserService userService
            , ITrackingService trackingService
            , ILogger<UserController> logger
            , IDanhMucService danhMucService
            ) : base(userService, trackingService, logger)
        {
            _logger = logger;
            _danhMucService = danhMucService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(UserSearchRequest request)
        {
            return View(request);
        }

        public async Task<IActionResult> List(UserSearchRequest request)
        {
            var result = await _userService.GetPaging(request);
            return PartialView(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.QuocGiaList = _danhMucService.GetDanhSachQuocGiaAsync();
                ViewBag.TinhList = new List<SelectListItem>();
                return PartialView();
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (UserCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userService.Create(request);
                return await ActionResult(result);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit (int id)
        {
            var user = await _userService.GetByIdAdmin(id);
            if (user == null) return NotFound();

            ViewBag.QuocGiaList = await _danhMucService.GetDanhSachQuocGiaAsync();
            ViewBag.TinhList = await _danhMucService.GetDanhSachTinhTheoQuocGiaAsync(user.QuocGiaId);
            return PartialView(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            request.UserId = User.GetUserId();

            var result = await _userService.Update(request);
            return await ActionResult(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteRequest request)
        {
            request.UserId = User.GetUserId();

            var result = await _userService.Delete(request);
            return Json(new
            {
                isSuccessed = result.IsSuccessed,
                message = result.Message
            });
        }

        // AJAX để load tỉnh theo quốc gia
        [HttpGet]
        public async Task<IActionResult> GetTinhByQuocGia(int quocGiaId)
        {
            var result = await _danhMucService.GetDanhSachTinhTheoQuocGiaAsync(quocGiaId);
            return Json(result);
        }

    }
}
