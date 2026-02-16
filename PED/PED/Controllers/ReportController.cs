using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WiseX.Data;
using WiseX.Services;
using WiseX.ViewModels.Admin;
using WiseX.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using WiseX.Helpers;
using System.Data;
using WiseX.ViewModels.Home;

namespace WiseX.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        private readonly ReportService _reportService;
        private readonly CommonService _commonService;
        private readonly HomeService _homeService;
        private readonly AdminService _adminService;
        private readonly IAuthorizationService _authorizationService;
        private readonly MenuUtils _menuUtils;

        public ReportController(ApplicationDbContext applicationDbContext, IAuthorizationService authorizationService)
        {
            _ApplicationDbContext = applicationDbContext;
            _reportService = new ReportService(applicationDbContext);
            _homeService = new HomeService(applicationDbContext);
            _commonService = new CommonService(applicationDbContext);
            _adminService = new AdminService(applicationDbContext);
            _authorizationService = authorizationService;
            _menuUtils = new MenuUtils(_ApplicationDbContext);
        }
        public IActionResult Index()
        {
            return View();
        }
 

    }
}