using DPKS.Model.TienNghi.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPKS.App.Controllers
{
    public class TienNghiController : Controller
    {
        private readonly ITienNghiService _tienNghiService;
        private readonly IOrganizationService _organizationService;
        public TienNghiController(ITienNghiService tienNghiService
            , IOrganizationService organizationService)
        {
            _tienNghiService = tienNghiService;
            _organizationService = organizationService;
        }
        public async Task<IActionResult> Index()
        {
            var request = new TienNghiCreateRequest(); // nếu không cần lọc, truyền rỗng
            var result = await _tienNghiService.GetAll(request);

            if (!result.IsSuccessed)
            {
                ViewBag.Error = result.Message;
                return View(new List<DPKS.Model.TienNghi.TienNghiVm>());
            }

            return View(result.ResultObj);
        }
    }
}
