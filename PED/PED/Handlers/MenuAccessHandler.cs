using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WiseX.Services;
using WiseX.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WiseX.Handlers
{
    public class MenuAccessRequirement : IAuthorizationRequirement
    {
        public MenuAccessRequirement(/*string ActionName, string ControllerName, */string RoleID)
        {
            //this.ActionName = ActionName;
            //this.ControllerName = ControllerName;
            this.RoleID = RoleID;
        }

        //public string ActionName { get; set; }
        //public string ControllerName { get; set; }
        public string RoleID { get; set; }
    }

    public class MenuAccessHandler : AuthorizationHandler<MenuAccessRequirement>
    {
        ApplicationDbContext _applicationDbContext;
        public MenuAccessHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MenuAccessRequirement requirement)
        {      
            byte[] byteArrRole;
            string RoleID = string.Empty;
            CommonService _commonService = new CommonService(_applicationDbContext);

            var redirectContext = context.Resource as ControllerContext;
            string actionName = redirectContext.ActionDescriptor.ActionName;
            string controllerName = redirectContext.ActionDescriptor.ControllerName;

            if (redirectContext.HttpContext.Session.TryGetValue("RoleID", out byteArrRole))
                RoleID = Encoding.UTF8.GetString(byteArrRole);
            
            if (_commonService.CheckMenuAccess(actionName, controllerName, RoleID))
            {
                context.Succeed(requirement);
            }
            else
                context.Fail();

            return Task.CompletedTask;
        }
    }
}
