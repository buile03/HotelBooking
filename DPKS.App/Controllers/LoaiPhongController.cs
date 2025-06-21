using DPKS.Model.LoaiPhong;
using DPKS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPKS.APP.Controllers
{
    public class LoaiPhongController : Controller
    {
        private readonly ILoaiPhongService _loaiPhongService;

        public LoaiPhongController(ILoaiPhongService loaiPhongService)
        {
            _loaiPhongService = loaiPhongService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _loaiPhongService.GetAllLoaiPhong();

            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(new List<ThongTinLoaiPhongVm>());
            }    

            return View(result.ResultObj); 
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _loaiPhongService.GetPhongById(id);

            if (!result.IsSuccessed || result.ResultObj == null)
            {
                ViewBag.ErrorMessage = result.Message;
                return RedirectToAction("Index");
            }

            return View(result.ResultObj); // ✅ CHỈ truyền LoaiPhongDetailVm
        }
    }
}
