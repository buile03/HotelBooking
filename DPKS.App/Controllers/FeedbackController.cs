using DPKS.Model.Feedback;
using DPKS.Model.Feedback.Request;
using DPKS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DPKS.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: Feedback
        public async Task<IActionResult> Index(SearchFeedbackRequest request)
        {
            var result = await _feedbackService.GetAll(request);

            if (result.IsSuccessed)
            {
                ViewBag.Message = result.Message;
                return View(result.ResultObj);
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return View(new List<DanhSachFeedbackVm>());
            }
        }
        
        public async Task<ActionResult> Detail (int id)
        {
            var result = await _feedbackService.GetById(id);

            if (result.IsSuccessed)
            {
                ViewBag.Message = result.Message;
                return View(result.ResultObj);
            }

            ViewBag.ErrorMessage = result.Message;
            return RedirectToAction("Index");
        }
    }
}