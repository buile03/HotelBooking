using DPKS.App.Extensions;
using DPKS.Common.Result;
using DPKS.Model.Role.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DPKS.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleService roleService, IUserService userService, ILogger<RoleController> logger, ITrackingService trackingService) : base(userService, trackingService, logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(RoleSearchRequest request)
        {
            return View(request);
        }
        public async Task<IActionResult> List(RoleSearchRequest request)
        {
            var result = await _roleService.GetPaging(request);
            return PartialView(result.ResultObj);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _roleService.Create(request);
            return await ActionResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleService.GetById(id);
            return role == null ? NotFound() : PartialView(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            request.UserId = User.GetUserId();
            var result = await _roleService.Update(request);
            return await ActionResult(result);
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

                var result = await _roleService.Delete(request);
                return Json(new
                {
                    isSuccessed = result.IsSuccessed,
                    message = result.Message,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã có lỗi xảy ra khi xóa vai trò");
                return ErrorResult();
            }
        }
    }
}
