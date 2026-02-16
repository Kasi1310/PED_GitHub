using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WiseX.ViewModels.Admin;
using WiseX.Data;
using WiseX.Services;
using WiseX.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WiseX.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WiseX.Helpers;

namespace WiseX.Controllers
{
    [Authorize]
    [SessionTimeout]
    public class BroadcastController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly CommonService _commonService;
        private readonly AdminService _adminService;
        private readonly BroadcastServices _broadcastServices;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HomeService _homeService;
        private readonly MenuUtils _menuUtils;
        private readonly IAuthorizationService _authorizationService;
        public BroadcastController(ApplicationDbContext applicationDbContext, RoleManager<IdentityRole> roleManager, IAuthorizationService authorizationService)
        {
            _applicationDbContext = applicationDbContext;
            _commonService = new CommonService(_applicationDbContext);
            _adminService = new AdminService(_applicationDbContext);
            _broadcastServices = new BroadcastServices(_applicationDbContext);
            _homeService = new HomeService(applicationDbContext);
            _menuUtils = new MenuUtils(applicationDbContext);
            _authorizationService = authorizationService;

            _roleManager = roleManager;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //Authorization
            if (!_authorizationService.AuthorizeAsync(User, this.ControllerContext, "MenuAccessPolicy").Result.Succeeded)
                return RedirectToAction("AccessDenied", "Home");

            //To set menu
            await _menuUtils.SetMenu(HttpContext.Session);

            return View("Index");
        }

     
    }
}