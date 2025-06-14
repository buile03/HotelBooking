using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using DPKS.App.Models;
using DPKS.Common;
using DPKS.Common.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace DPKS.App.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Render a partial view to string.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="controller"></param>
        /// <param name="viewNamePath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<string> RenderViewToStringAsync<TModel>(this Controller controller, string viewNamePath, TModel model)
        {
            if (string.IsNullOrEmpty(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            controller.ViewData.Model = model;

            using (StringWriter writer = new StringWriter())
            {
                try
                {
                    IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                    ViewEngineResult viewResult = null;

                    if (viewNamePath.EndsWith(".cshtml"))
                        viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                    else
                        viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

                    if (!viewResult.Success)
                        return $"A view with the name '{viewNamePath}' could not be found";

                    ViewContext viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    return writer.GetStringBuilder().ToString();
                }
                catch (Exception exc)
                {
                    return $"Failed - {exc.Message}";
                }
            }
        }

        /// <summary>
        /// Render a partial view to string, without a model present.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="controller"></param>
        /// <param name="viewNamePath"></param>
        /// <returns></returns>
        public static async Task<string> RenderViewToStringAsync(this Controller controller, string viewNamePath)
        {
            if (string.IsNullOrEmpty(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            using (StringWriter writer = new StringWriter())
            {
                try
                {
                    IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                    ViewEngineResult viewResult = null;

                    if (viewNamePath.EndsWith(".cshtml"))
                        viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                    else
                        viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

                    if (!viewResult.Success)
                        return $"A view with the name '{viewNamePath}' could not be found";

                    ViewContext viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    return writer.GetStringBuilder().ToString();
                }
                catch (Exception exc)
                {
                    return $"Failed - {exc.Message}";
                }
            }
        }

        public static async Task<string> RenderViewToStringAsync<TModel>(this ViewComponent component, string viewNamePath, TModel model)
        {


            component.ViewData.Model = model;

            using (StringWriter writer = new StringWriter())
            {
                try
                {
                    IViewEngine viewEngine = component.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                    ViewEngineResult viewResult = null;

                    if (viewNamePath.EndsWith(".cshtml"))
                        viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                    else
                        viewResult = viewEngine.FindView(component.ViewContext, viewNamePath, false);

                    if (!viewResult.Success)
                        return $"A view with the name '{viewNamePath}' could not be found";

                    ViewContext viewContext = new ViewContext(
                        component.ViewContext,
                        viewResult.View,
                        component.ViewData,
                        component.TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    return writer.GetStringBuilder().ToString();
                }
                catch (Exception exc)
                {
                    return $"Failed - {exc.Message}";
                }
            }
        }
        public static void AddCookie(this HttpResponse response, string cookieName, string value)
        {
            response.Cookies.Append(cookieName, value,
                new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Path = "/",
                    Expires = DateTime.Now.AddMinutes(5)
                }
            );
        }
        public static string GetCookie(this HttpRequest request, string cookieName)
        {
            var cookies = request.Cookies.Select((header) => $"{header.Key}");
            if (cookies.Contains(cookieName))
            {
                return request.Cookies[cookieName];
            }

            return string.Empty;
        }
        public static string[] GetError(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray();
        }
        public static string GetStringError(this ModelStateDictionary modelState)
        {
            return string.Join(",", modelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).ToArray());
        }
        //public static UserLoginRequest GetUser(this ClaimsPrincipal user)
        //{
        //    if (user.Identity.IsAuthenticated)
        //    {
        //        var userId = user.FindFirst("Id").Value;
        //        if (string.IsNullOrEmpty(userId)) return null;

        //        return new UserLoginRequest()
        //        {
        //            Id = Guid.Parse(userId),
        //            UserName = user.FindFirst("UserName").Value,
        //            HoVaTen = user.FindFirst("FullName").Value,
        //            OrganName = user.FindFirst("OrganName").Value,
        //            OrganId = Convert.ToInt32(user.FindFirst("OrganId").Value),
        //            IsSupperUser = Convert.ToBoolean((user.FindFirst("IsSupperUser")?.Value ?? "false"))
        //        };
        //    }
        //    return new UserLoginRequest()
        //    {
        //        Id = Guid.Empty
        //    };

        //}
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                var userId = user.FindFirst("Id").Value;
                if (string.IsNullOrEmpty(userId))
                    return Guid.Empty;

                return Guid.Parse(userId);
            }
            return Guid.Empty;
        }
        public static string GetRawUrl(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
        }
        public static string GetRawUrl(this HttpRequest request, string url, bool IsQuery = true)
        {
            var httpContext = request.HttpContext;
            if (!IsQuery)
            {
                return $"{url}";
            }
            else
                return $"{url}{HttpUtility.UrlDecode(SystemFunctions.RemoveIllegalCharacters(httpContext.Request.QueryString.ToString()))}";
        }
        public static string GetBackUrl(this HttpRequest request)
        {
            var httpContext = request.HttpContext;
            string url = httpContext.Request.Headers["Referer"].ToString();
            if (!String.IsNullOrEmpty(url)) return url;
            else return "/";
        }
    }
}
