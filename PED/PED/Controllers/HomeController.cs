using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WiseX.Data;
using WiseX.Helpers;
using WiseX.Services;
using WiseX.ViewModels.Account;
using WiseX.ViewModels.Home;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WiseX.Models;
using WiseX.ViewModels.Admin;
using Microsoft.Extensions.Configuration;
using System.IO;
using OfficeOpenXml;

namespace WiseX.Controllers
{
    [Authorize]
    [SessionTimeout]
    public class HomeController : Controller
    {
        private readonly AdminService _adminService;
        private readonly HomeService _homeService;
        private readonly MenuUtils _menuUtils;
        private readonly CommonService _commonService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ReportService _reportService;
        private readonly IConfiguration _configuration;

        public HomeController(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _adminService = new AdminService(applicationDbContext);
            _homeService = new HomeService(applicationDbContext);
            _commonService = new CommonService(applicationDbContext);
            _menuUtils = new MenuUtils(applicationDbContext);
            _hostingEnvironment = hostingEnvironment;
            _reportService = new ReportService(applicationDbContext);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            _configuration = configuration;
        }

        //
        // GET: /Home/
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //To set menu
            await _menuUtils.SetMenu(HttpContext.Session);
            var userId = "";
            var RoleId = "";
            var RoleName = "";
            int ProjectID = 0;
            var ProjectName = "";
            int ResourceID = 0;
            try
            {
                userId = HttpContext.Session.GetString("UserID");
                RoleId = HttpContext.Session.GetString("RoleID");
                RoleName = HttpContext.Session.GetString("RoleName");

                ProjectName = string.IsNullOrWhiteSpace(HttpContext.Session.GetString("ProjectName")) ? "" : HttpContext.Session.GetString("ProjectName");
                if (HttpContext.Session.GetString("ProjectID") != null)
                    ProjectID = Int32.Parse(HttpContext.Session.GetString("ProjectID"));
                if (HttpContext.Session.GetString("ResourceID") != null)
                    ResourceID = Convert.ToInt32(HttpContext.Session.GetString("ResourceID"));
            }
            catch (Exception Ex)
            {
            }
            if (userId == null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            IList<NotificationDetails> NotificationDetails = new List<NotificationDetails>();
            NotificationDetails = await _commonService.GetNotificationDetailsList(userId);

            //Save List in Session
            var str = JsonConvert.SerializeObject(NotificationDetails);
            HttpContext.Session.SetString("my-message", str);


            //Dashboard details
            DataSet data = new DataSet();
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.ChartBoxes = await _homeService.GetBoxes();
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.ChartBoxPropertiesList = await _homeService.GetBoxesList("0", UserID);
            if (homeViewModel.ChartBoxes == null)
                homeViewModel.ChartBoxes = new ChartProperties();
            return View(homeViewModel);
        }
        public ActionResult Charts()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View("Error");
        }

        public ActionResult AccessDenied()
        {
            return View("Unauthorized");
        }

        public async Task<ActionResult> GetDashboardList(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.ChartBoxPropertiesList = await _homeService.GetBoxesList(BoxName, UserID);

            return PartialView("_BoxesList", homeViewModel);
        }

        public async Task<ActionResult> GetDashboardList1(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.ChartBoxPropertiesLoad = await _homeService.GetBoxesList1(Start, Length, BoxName, UserID);
            int totalrows = (homeViewModel.ChartBoxPropertiesLoad.Count > 0 ? homeViewModel.ChartBoxPropertiesLoad.First().TotalCount : 0);
            int totalRowsAfterFiltering = (homeViewModel.ChartBoxPropertiesLoad.Count > 0 ? homeViewModel.ChartBoxPropertiesLoad.First().TotalCount : 0);

            return Json(new { data = homeViewModel.ChartBoxPropertiesLoad, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }


        public async Task<ActionResult> GetPECredentialingLicenseBoxesList(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.PECredentialingLicenseBoxesList = await _homeService.GetPECredentialingLicenseBoxesList(Start, Length, BoxName, UserID);
            int totalrows = (homeViewModel.PECredentialingLicenseBoxesList.Count > 0 ? homeViewModel.PECredentialingLicenseBoxesList.First().TotalCount : 0);
            int totalRowsAfterFiltering = (homeViewModel.PECredentialingLicenseBoxesList.Count > 0 ? homeViewModel.PECredentialingLicenseBoxesList.First().TotalCount : 0);

            return Json(new { data = homeViewModel.PECredentialingLicenseBoxesList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }


        public async Task<ActionResult> GetPEContractBoxesList(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.PEContractBoxesList = await _homeService.GetPEContractBoxesList(Start, Length, BoxName, UserID);
            int totalrows = (homeViewModel.PEContractBoxesList.Count > 0 ? homeViewModel.PEContractBoxesList.First().TotalCount : 0);
            int totalRowsAfterFiltering = (homeViewModel.PEContractBoxesList.Count > 0 ? homeViewModel.PEContractBoxesList.First().TotalCount : 0);

            return Json(new { data = homeViewModel.PEContractBoxesList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        public async Task<ActionResult> GetPECheckListBoxes(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.PECheckListBoxes = await _homeService.GetPECheckListBoxes(Start, Length, BoxName, UserID);
            int totalrows = (homeViewModel.PECheckListBoxes.Count > 0 ? homeViewModel.PECheckListBoxes.First().TotalCount : 0);
            int totalRowsAfterFiltering = (homeViewModel.PECheckListBoxes.Count > 0 ? homeViewModel.PECheckListBoxes.First().TotalCount : 0);

            return Json(new { data = homeViewModel.PECheckListBoxes, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        
        public async Task<ActionResult> GetPECheckListBoxesForAllocatedUser(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");
            homeViewModel.PECheckListBoxesFroAllocatedUser = await _homeService.GetPECheckListBoxesForAllocatedUser(Start, Length, BoxName, UserID);
            int totalrows = (homeViewModel.PECheckListBoxesFroAllocatedUser.Count > 0 ? homeViewModel.PECheckListBoxesFroAllocatedUser.First().TotalCount : 0);
            int totalRowsAfterFiltering = (homeViewModel.PECheckListBoxesFroAllocatedUser.Count > 0 ? homeViewModel.PECheckListBoxesFroAllocatedUser.First().TotalCount : 0);

            return Json(new { data = homeViewModel.PECheckListBoxesFroAllocatedUser, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }


        //added by aajit on 23/10/2020
        [HttpPost]
        public async Task<IActionResult> DownloadDashboardData(string BoxName)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            int Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
            int Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
            string UserID = HttpContext.Session.GetString("UserID");

            string sFileName = @"DashboardData.xlsx";
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string UrlBase = _configuration["AppSettings:AppURL"].ToString() + sFileName;
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                

                if (BoxName == "B1" || BoxName == "B2" || BoxName == "B3" || BoxName == "B4"
                    || BoxName == "B5" || BoxName == "B6" || BoxName == "B7" || BoxName == "B8")
                {
                    homeViewModel.ChartBoxPropertiesLoad = await _homeService.GetBoxesList1(Start, Length, BoxName, UserID);
                    var data = homeViewModel.ChartBoxPropertiesLoad.Select(x => new { x.CompanyID, x.CompanyName, x.Fees, x.ContractExpiryDate, x.UserName, x.Status, x.Notes }).ToList();

                    worksheet.Cells.LoadFromCollection(data, true);
                }
                else if (BoxName == "B9" || BoxName == "B10")
                {
                    homeViewModel.PECredentialingLicenseBoxesList = await _homeService.GetPECredentialingLicenseBoxesList(Start, Length, BoxName, UserID);
                    var data = homeViewModel.PECredentialingLicenseBoxesList.Select(x => new { x.CompanyId, x.CompanyName, x.LicenseType, x.CertificationLevel, x.LicenseNo,x.IssuedDate,x.EffectiveDate, x.ExpiryDate }).ToList();

                    worksheet.Cells.LoadFromCollection(data, true);
                }
                else if (BoxName == "B11" || BoxName == "B15")
                {
                    homeViewModel.PEContractBoxesList = await _homeService.GetPEContractBoxesList(Start, Length, BoxName, UserID);
                    var data = homeViewModel.PEContractBoxesList.Select(x => new { x.CompanyId, x.CompanyName, x.InsuranceContract, x.LastRevalidateDate, x.EffectiveDate, x.NextUpdateDue, x.TermedDate }).ToList();

                    worksheet.Cells.LoadFromCollection(data, true);
                }
                else if (BoxName == "B12" || BoxName == "B16")
                {
                    homeViewModel.PEContractBoxesList = await _homeService.GetPEContractBoxesList(Start, Length, BoxName, UserID);
                    var data = homeViewModel.PEContractBoxesList.Select(x => new { x.CompanyId, x.CompanyName, x.InsuranceContract, x.LastRevalidateDate, x.EffectiveDate, x.NextUpdateDue, x.TermedDate }).ToList();

                    worksheet.Cells.LoadFromCollection(data, true);
                }
                else if (BoxName == "B13" || BoxName == "B14")
                {
                    homeViewModel.PECheckListBoxes = await _homeService.GetPECheckListBoxes(Start, Length, BoxName, UserID);
                    var data = homeViewModel.PECheckListBoxes.Select(x => new { x.CompanyId, x.CompanyName,x.BillingType, x.Task, x.Status, x.AllocatedUser }).ToList();

                    worksheet.Cells.LoadFromCollection(data, true);
                }
                else if (BoxName == "B17")
                {
                    homeViewModel.PECheckListBoxes = await _homeService.GetPECheckListBoxes(Start, Length, BoxName, UserID);
                    var data = homeViewModel.PECheckListBoxes.Select(x => new { x.CompanyId, x.CompanyName, x.BillingType, x.Task, x.Status, x.AllocatedUser }).ToList();

                    worksheet.Cells.LoadFromCollection(data, true);
                }
                else if (BoxName == "B18")
                {
                    homeViewModel.PECheckListBoxes = await _homeService.GetPECheckListBoxes(Start, Length, BoxName, UserID);
                    var data = homeViewModel.PECheckListBoxes.Select(x => new { x.CompanyId, x.CompanyName, x.BillingType, x.Task, x.Status, x.AllocatedUser }).ToList();

                    worksheet.Cells.LoadFromCollection(data, true);
                }
                else if (BoxName == "B19")
                {
                    homeViewModel.PECheckListBoxes = await _homeService.GetPECheckListBoxes(Start, Length, BoxName, UserID);
                    var data = homeViewModel.PECheckListBoxes.Select(x => new { x.CompanyId, x.CompanyName, x.BillingType, x.Task, x.Status, x.AllocatedUser }).ToList();

                    worksheet.Cells.LoadFromCollection(data, true);
                }

                package.Save();
                package.Dispose();
                worksheet.Dispose();

            }
            return Json(UrlBase);
        }
    }

}
