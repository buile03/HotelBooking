using DPKS.Data.EF;
using DPKS.Model.DatPhong.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DPKS.APP.Controllers
{
    [Authorize]
    public class DatPhongController : Controller
    {
        private readonly IDatPhongService _datPhongService;
        private readonly IPhongService _phongService;
        private readonly AppDbContext _context;


        public DatPhongController(AppDbContext context,IDatPhongService datPhongService, IPhongService phongService)
        {
            _datPhongService = datPhongService;
            _context = context;
            _phongService = phongService;
        }

        public IActionResult Create(int id)
        {
            var model = new DatPhongCreateRequest
            {
                PhongId = id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DatPhongCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine("Bắt đầu xử lý đặt phòng");

                        Console.WriteLine($"PhongId: {request.PhongId}");
                        Console.WriteLine($"NgayNhan: {request.NgayNhanPhong}, NgayTra: {request.NgayTraPhong}");
                        Console.WriteLine($"UserId: {request.UserId}");

                    }
                }
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
                await _datPhongService.DatPhongAsync(request);
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
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
