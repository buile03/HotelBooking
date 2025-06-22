using ClosedXML.Excel;
using DPKS.App.Extensions;
using DPKS.Common.Result;
using DPKS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DPKS.Admin.Controllers
{
    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [AllowAnonymous]
    public class BaseController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITrackingService _trackingService;
        private readonly ILogger<BaseController> _logger;

        public BaseController(IUserService userService,
            ITrackingService trackingService,
            ILogger<BaseController> logger)
        {
            _userService = userService;
            _trackingService = trackingService;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

   

        public async Task<IActionResult> ActionResult(Result result)
        {
            if (result.IsSuccessed)
            {
                await _trackingService.Create(User.GetUserId(), result);
            }
            return result.Status switch
            {
                ResultStatus.Ok => new OkObjectResult(result),
                ResultStatus.Error => new BadRequestObjectResult(result),
                ResultStatus.NotFound => new NotFoundObjectResult(result),
                _ => throw new NotImplementedException(),
            };
        }

        public async Task<IActionResult> ActionResult<T>(Result<T> result)
        {
            if (result.IsSuccessed)
            {
                var rs = new Result(result.IsSuccessed, result.Message);


                await _trackingService.Create(User.GetUserId(), rs);
            }
            return result.Status switch
            {
                ResultStatus.Ok => new OkObjectResult(result),
                ResultStatus.Error => new BadRequestObjectResult(result),
                ResultStatus.NotFound => new NotFoundObjectResult(result),
                _ => throw new NotImplementedException(),
            };
        }

        public IActionResult ErrorResult()
        {
            return new BadRequestObjectResult(new Result(false, "Cập nhật không thành công"));
        }
        public IActionResult IsValidResult()
        {
            return new BadRequestObjectResult(new Result(false, "Vui lòng nhập đầy đủ thông tin"));
        }
        protected void CellBorder(IXLWorksheet worksheet, int row, int col, bool isWrap = true, bool isCenter = true)
        {
            worksheet.Cell(row, col).Style.Alignment.WrapText = isWrap;
            worksheet.Cell(row, col).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(row, col).Style.Border.TopBorderColor = XLColor.Black;
            worksheet.Cell(row, col).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(row, col).Style.Border.BottomBorderColor = XLColor.Black;
            worksheet.Cell(row, col).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(row, col).Style.Border.LeftBorderColor = XLColor.Black;
            worksheet.Cell(row, col).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(row, col).Style.Border.RightBorderColor = XLColor.Black;
            if (isCenter)
            {
                worksheet.Cell(row, col).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell(row, col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            }
        }

        protected void RangeBorder(IXLWorksheet worksheet, int rowStart, int colStart, int rowEnd, int colEnd, bool isWrap = true, bool isCenter = true)
        {
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Alignment.WrapText = isWrap;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.TopBorderColor = XLColor.Black;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.BottomBorderColor = XLColor.Black;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.LeftBorderColor = XLColor.Black;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Border.RightBorderColor = XLColor.Black;
            worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            if (isCenter)
            {
                worksheet.Range(worksheet.Cell(rowStart, colStart), worksheet.Cell(rowEnd, colEnd)).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            }
        }
    }
}