using Microsoft.AspNetCore.Mvc;

namespace DPKS.APP.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
