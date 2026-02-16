using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using WiseX.Models;
using WiseX.Services;
using WiseX.Data;

namespace WiseX.Helpers
{

    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly HelperService _HelperService;
        public CustomExceptionFilter( ApplicationDbContext applicationDbContext)
        {
            _HelperService = new HelperService(applicationDbContext);
        }

        public interface IExceptionFilter : IFilterMetadata
        {
            void OnException(ExceptionContext context);
        }

        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            String message = String.Empty;

            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(MyAppException))
            {
                message = context.Exception.ToString();
                status = HttpStatusCode.InternalServerError;
            }
            else
            {
                message = context.Exception.Message;
                status = HttpStatusCode.NotFound;
            }
            string controller = string.Empty;
            string actionname = string.Empty;
            string host = string.Empty;
            string path = string.Empty;
            ErrorModel model = new ErrorModel();
            ErrorLog error = new ErrorLog();
            error.Message = context.Exception.Message;
            error.IPAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            error.StackTrace = context.Exception.StackTrace;
            context.ActionDescriptor.RouteValues.TryGetValue("controller", out controller);
            error.Controller = controller;
            context.ActionDescriptor.RouteValues.TryGetValue("action", out actionname);
            error.Method = actionname;
            path =  context.HttpContext.Request.Path;
            host = context.HttpContext.Request.Host.ToString();
            error.URL = host + " " + path;     
            model.ErrorLog = error;
            _HelperService.SaveErrorLog(error);
            //HttpResponse response = context.HttpContext.Response;
            //response.StatusCode = (int)status;
            //response.ContentType = "application/json";
            //var err = message + " " + context.Exception.StackTrace;
            //response.WriteAsync(err);

        }
    }
}