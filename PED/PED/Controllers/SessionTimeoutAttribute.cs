using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;
using WiseX.Models;

namespace WiseX.Controllers
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //UserSessionDetails userSessionDetails = new UserSessionDetails();
            //userSessionDetails = await _commonService.GetUserSessionDetails(user.Id);

            //var thisController = ((ReviewController)filterContext.Controller);
            if (filterContext.HttpContext.Session.GetString("UserID") == null)
            {
                bool IsAjaxCall = HttpRequestExtensions.IsAjaxRequest(filterContext.HttpContext.Request);

                if (IsAjaxCall)
                {
                    filterContext.Result = new RedirectToActionResult("AjaxSessionExpire", "Account", null);
                }
                else
                {
                    filterContext.Result = new RedirectToActionResult("Login", "Account", null);
                }
            }
            else
            {
                if (filterContext.HttpContext.Session.GetString("SessionPwdValidationFlag") == "0")
                {
                    filterContext.Result = new RedirectToActionResult("ChangePassword", "Account", null);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
