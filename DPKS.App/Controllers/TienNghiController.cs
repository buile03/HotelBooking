
using DPKS.Common.Result;
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
        public async Task<IActionResult> Index([FromQuery] GetPagingRequest request)
        {
            var result = await _tienNghiService.GetAll(request);

            if (!result.IsSuccessed || result.ResultObj == null)
            {
                ViewBag.Error = result.Message;
                return View(new PagedResult<DPKS.Model.TienNghi.DanhSachTienNghiVm>());
            }

            return View(result.ResultObj);
        }


    }
}
