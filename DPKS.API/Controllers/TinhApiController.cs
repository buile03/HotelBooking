using DPKS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DPKS.API.Controllers
{
    [Route("api/tinh")]
    [ApiController]
    public class TinhApiController : ControllerBase
    {
        private readonly IDanhMucService _danhMucService;

        public TinhApiController(IDanhMucService danhMucService)
        {
            _danhMucService = danhMucService;
        }

        [HttpGet("by-quocgia/{quocGiaId}")]
        public async Task<IActionResult> GetByQuocGia(int quocGiaId)
        {
            var list = await _danhMucService.GetDanhSachTinhTheoQuocGiaAsync(quocGiaId);
            return Ok(list.Select(t => new
            {
                id = t.Value,
                tenTinh = t.Text
            }));
        }
    }

}
