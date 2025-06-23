using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace DPKS.Common.Helper
{
    public static class HtmlHelperExtensions
    {
        public static string GetAntiXsrfRequestToken(this IHtmlHelper htmlHelper)
        {
            using (var writer = new StringWriter())
            {
                htmlHelper.AntiForgeryToken().WriteTo(writer, HtmlEncoder.Default);
                var html = writer.ToString();

                // Lấy giá trị token từ input
                var match = Regex.Match(html, "value=\"(.+?)\"");
                return match.Success ? match.Groups[1].Value : "";
            }
        }
    }
}
