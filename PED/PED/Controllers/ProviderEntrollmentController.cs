//using Aspose.Pdf;
//using Aspose.Pdf.Text;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using PED.Helpers;
using PED.ViewModels.Admin;
using PED.ViewModels.Contract;
using PED.ViewModels.ProviderEntrollment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WiseX.Data;
using WiseX.Helpers;
using WiseX.Models;
using WiseX.Services;
using WiseX.ViewModels.Admin;
using static WiseX.Helpers.FilesHelper;


namespace PED.Controllers
{
    public class ProviderEntrollmentController : Controller
    {
        String StorageRoot = null;
        String ClientsStorageRoot = null;
        String TemplateRoot = null;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _ApplicationDbContext;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AdminService _adminService;
        private readonly CommonService _commonService;
        private readonly IConfiguration _configuration;
        private readonly IAuthorizationService _authorizationService;
        private readonly MenuUtils _menuUtils;
        private readonly IHostingEnvironment _hostingEnvironment;
        int totalrows, totalRowsAfterFiltering;
        List<SMTPServerDetails> SMTP = new List<SMTPServerDetails>();
        public ProviderEntrollmentController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext, RoleManager<ApplicationRole> roleManager, IHostingEnvironment hostingEnvironment, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _ApplicationDbContext = applicationDbContext;
            _adminService = new AdminService(applicationDbContext);
            _commonService = new CommonService(applicationDbContext);
            _userManager = userManager;
            _ApplicationDbContext = applicationDbContext;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _authorizationService = authorizationService;
            _menuUtils = new MenuUtils(_ApplicationDbContext);
            this.StorageRoot = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["AppSettings:ContractUploadPath"].ToString());
            this.ClientsStorageRoot = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["AppSettings:ClientsUploadPath"].ToString());
            this.TemplateRoot = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["AppSettings:MailTemplate"].ToString());

        }

        [HttpGet]
        public async Task<IActionResult> ProviderEntrollment()
        {
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PEDetails = new PEDetails();
                model.PEInsuranceContracts = new PEInsuranceContracts();
                model.PEOtherAddress = new PEOtherAddress();
                model.PEContacts = new PEContacts();                

                model.PEDocuments = new PEDocuments();
                model.PECredentialingLicense = new PECredentialingLicense();
                model.PECheckList = new PECheckList();

                model.PENotes = new PENotes();

                model.ResidencyCodeList = new List<ResidencyCodeList>();// await _adminService.GetResidencyCodeList();
                model.PEStatusList = new List<PEStatusList>();// await _adminService.GetPEStatusList();
                model.AccountExecutiveList = new List<AccountExecutiveList>();// await _adminService.GetAccountExecutiveList();
                model.PEHoursofOperationList = new List<PEHoursofOperationList>();// await _adminService.GetHoursofOperationList();
                model.PETaxClassificationList = new List<PETaxClassificationList>();// await _adminService.GetTaxClassificationList();
                model.PEPaymentIndicatorList = new List<PEPaymentIndicatorList>();// await _adminService.GetPaymentIndicatorList();
                model.PETaxonomyList = new List<PETaxonomyList>();// await _adminService.GetPETaxonomyList();

                model.InsuranceContractList = new List<InsuranceContractList>();// await _adminService.GetInsuranceContract(0);
                model.PEAddressTypeList = new List<PEAddressTypeList>();// await _adminService.GetPEAddressTypeList();
                model.PEContactTypeList = new List<PEContactTypeList>();// await _adminService.GetPEContactTypeList();
                model.PELicenseTypeList = new List<PELicenseTypeList>();// await _adminService.GetPELicenseTypeList();
                model.PECertificationLevelList = new List<PECertificationLevelList>();// await _adminService.GetPECertificationLevelList();
                model.PEDocumentTitleList = new List<PEDocumentTitleList>();// await _adminService.GetPEDocumentTitleList();

                model.CitiesList = new List<CitiesList>();
                //model.StatesList = new List<StatesList>();// await _adminService.GetStatesList();
                model.StatesList = await _adminService.GetStatesList();

                model.PEPaymentCategoryChangeCheckList = new PEPaymentCategoryChangeCheckList();
                model.PESinglePayerUpdateCheckList = new PESinglePayerUpdateCheckList();
                model.PEPracticeLocationChangeCheckList = new PEPracticeLocationChangeCheckList();
                model.PEClosedClientCheckList = new PEClosedClientCheckList();
                model.PEClientBankChangeCheckList = new PEClientBankChangeCheckList();
                model.PECommericalEFTsCheckList = new PECommericalEFTsCheckList();
                model.PEMedicareChangeOfInfoCheckList = new PEMedicareChangeOfInfoCheckList();
                model.PEOutOfStateMCDCheckList = new PEOutOfStateMCDCheckList();

                model.ListOfPESinglePayerUpdateCheckList = new List<PESinglePayerUpdateCheckList>();

                model.PEInsuranceContractFileDetailsList = new List<PEInsuranceContractFileDetails>();
                model.PEOtherAddressList = new List<PEOtherAddressList>();
                model.PEContactsList = new List<PEContactsList>();

                model.PEDocumentsList = new List<PEDocumentsList>();
                model.PECredentialingLicenseList = new List<PECredentialingLicenseList>();
                model.PENotesList = new List<PENotesList>();

                model.PEEPCRList = new List<PEEPCRList>();
                model.PEEnrollmentRepresentativeStatusList = new List<PEEnrollmentRepresentativeStatusList>();

                model.PERefusedEnrollment = new PERefusedEnrollment();
                model.PERefusedEnrollmentList = new List<PERefusedEnrollmentList>();

                model.PEEFT = new PEEFT();
                model.PEEFTList = new List<PEEFTList>();

                model.PEClientsBankInfo = new PEClientsBankInfo();
                model.PEClientsBankInfoList = new List<PEClientsBankInfoList>();// new List<PEClientsBankInfo>();
                model.PEBankLetterDocumentList = new List<PEClientsBankInfoBankLetterDocumentsList>();


            }
            catch (Exception ex)
            {

            }
            return View("ProviderEntrollment", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveProviderEntrollment([FromBody] ProviderEntrollmentDetails providerEntrollmentDetails)
        {
            ProviderEntrollment model = new ProviderEntrollment();

            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                providerEntrollmentDetails.CreatedBy = HttpContext.Session.GetString("UserID");
                model.ProviderEntrollmentDetails = providerEntrollmentDetails;
                model.ProviderEntrollmentDetailsList = await _adminService.UpdateProviderEntrollmentDetails(model, userId);
            }
            catch (Exception ex)
            {

            }
            //ProviderEntrollmentDetailsList ProviderEntrollmentDetailsList
            return PartialView("_ProviderEntrollmentList", model.ProviderEntrollmentDetailsList);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditProviderEntrollment([FromBody] int PEId)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                model.ProviderEntrollmentDetailsList = await _adminService.GetProviderEntrollment(PEId);

                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();

                ProviderEntrollmentDetails res = new ProviderEntrollmentDetails();
                if (model.ProviderEntrollmentDetailsList.Count > 0)
                {
                    foreach (var item in model.ProviderEntrollmentDetailsList)
                    {
                        if (item.ID == PEId)
                        {
                            res.ID = item.ID;
                            res.CompanyId = item.CompanyId;
                            res.Description = item.Description.ToString();
                            res.ContactName = item.ContactName;
                            res.ContactPhone = item.ContactPhone.ToString();
                            res.NPINumber = item.NPINumber.ToString();
                            res.TaxID = item.TaxID;
                            res.CollectionsCompanyName = item.CollectionsCompanyName;
                            res.PayToName = item.PayToName;
                            res.PayToAddressLine1 = item.PayToAddressLine1;
                            res.PayToAddressLine2 = item.PayToAddressLine2;
                            res.PayToCityId = item.PayToCityId;
                            res.PayToStateId = item.PayToStateId;
                            res.PayToZipCode = item.PayToZipCode;
                            res.BillingProviderName = item.BillingProviderName;
                            res.BillingProviderAddressLine1 = item.BillingProviderAddressLine1;
                            res.BillingProviderAddressLine2 = item.BillingProviderAddressLine2;
                            res.BillingProviderCityId = item.BillingProviderCityId;
                            res.BillingProviderStateId = item.BillingProviderStateId;
                            res.BillingProviderZipCode = item.BillingProviderZipCode;
                            res.LastDOS = item.LastDOS;
                            res.ContractEndDate = item.ContractEndDate;
                        }
                    }
                    model.ProviderEntrollmentDetails = res;
                }

            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_ProviderEntrollmentForm", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteProviderEntrollment(int PEID)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");

                await _adminService.DeleteProviderEntrollment(PEID, userId);
                model.ProviderEntrollmentDetailsList = await _adminService.GetProviderEntrollment(0);
            }
            catch (Exception Ex)
            {

            }
            return PartialView("_ProviderEntrollment", model.ProviderEntrollmentDetailsList);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadProviderEntrollment()
        {
            List<DownloadProviderEntrollmentDetailsList> model = new List<DownloadProviderEntrollmentDetailsList>();
            string UserId = HttpContext.Session.GetString("UserID");
            model = await _adminService.ExportProviderEntrollmentDetails();
            var data = model;
            string sFileName = @"Provider_Entrollment.xlsx";
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
                worksheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            return Json(UrlBase);
        }

        [HttpPost]
        public async Task<IActionResult> GetPESearchClient(string Prefix)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                if (Prefix == null)
                    Prefix = "";
                model.SearchPEClientList = await _adminService.GetPESearchClient(Prefix);
            }
            catch (Exception Ex) { }
            return Json(model.SearchPEClientList);
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        [HttpPost]
        public async Task<IActionResult> SearchPEDetails(int Id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");

                model.ResidencyCodeList = await _adminService.GetResidencyCodeList();
                model.PEStatusList = await _adminService.GetPEStatusList();
                model.AccountExecutiveList = await _adminService.GetAccountExecutiveList();
                model.PEHoursofOperationList = await _adminService.GetHoursofOperationList();
                model.PETaxClassificationList = await _adminService.GetTaxClassificationList();
                model.PEPaymentIndicatorList = await _adminService.GetPaymentIndicatorList();
                model.PETaxonomyList = await _adminService.GetPETaxonomyList();

                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();


                model.PEDetails = await _adminService.GetPEDetails(Id, userId);
                model.PECheckList = await _adminService.GetPECheckList(Id, userId);

                //model.InsuranceContractList = await _adminService.GetInsuranceContract(0);
                //model.PEAddressTypeList = await _adminService.GetPEAddressTypeList();
                //model.PEContactTypeList = await _adminService.GetPEContactTypeList();
                //model.PELicenseTypeList = await _adminService.GetPELicenseTypeList();
                //model.PECertificationLevelList = await _adminService.GetPECertificationLevelList();
                //model.PEDocumentTitleList = await _adminService.GetPEDocumentTitleList();

                model.InsuranceContractList = new List<InsuranceContractList>();// await _adminService.GetInsuranceContract(0);
                model.PEAddressTypeList = new List<PEAddressTypeList>();// await _adminService.GetPEAddressTypeList();
                model.PEContactTypeList = new List<PEContactTypeList>();// await _adminService.GetPEContactTypeList();
                model.PELicenseTypeList = new List<PELicenseTypeList>();// await _adminService.GetPELicenseTypeList();
                model.PECertificationLevelList = new List<PECertificationLevelList>();// await _adminService.GetPECertificationLevelList();
                model.PEDocumentTitleList = new List<PEDocumentTitleList>();// await _adminService.GetPEDocumentTitleList();
                model.PENotesList = new List<PENotesList>();

                model.PEPaymentCategoryChangeCheckList = await _adminService.GetPEPaymentCategoryChangeCheckList(Id, userId);
                model.PESinglePayerUpdateCheckList = new PESinglePayerUpdateCheckList();
                model.ListOfPESinglePayerUpdateCheckList = await _adminService.GetListOfPESinglePayerUpdateCheckList(Id);
                model.PEPracticeLocationChangeCheckList = await _adminService.GetPEPracticeLocationChangeCheckList(Id, userId);
                model.PEClosedClientCheckList = await _adminService.GetPEClosedClientCheckList(Id, userId);
                model.PEClientBankChangeCheckList = await _adminService.GetPEClientBankChangeCheckList(Id, userId);
                model.PECommericalEFTsCheckList = await _adminService.GetPECommericalEFTsCheckList(Id, userId);
                model.PEMedicareChangeOfInfoCheckList = await _adminService.GetPEMedicareChangeOfInfoCheckList(Id, userId);
                model.PEOutOfStateMCDCheckList = await _adminService.GetPEOutOfStateMCDCheckList(Id, userId);

                //model.PEPaymentCategoryChangeCheckList = new PEPaymentCategoryChangeCheckList();
                //model.PESinglePayerUpdateCheckList = new PESinglePayerUpdateCheckList();
                //model.PEPracticeLocationChangeCheckList = new PEPracticeLocationChangeCheckList();
                //model.PEClosedClientCheckList = new PEClosedClientCheckList();
                //model.PEClientBankChangeCheckList = new PEClientBankChangeCheckList();
                //model.PECommericalEFTsCheckList = new PECommericalEFTsCheckList();
                //model.PEMedicareChangeOfInfoCheckList = new PEMedicareChangeOfInfoCheckList();
                //model.PEOutOfStateMCDCheckList = new PEOutOfStateMCDCheckList();

                model.PEEPCRList = await _adminService.GetPEEPCRStatusList();
                model.PEEnrollmentRepresentativeStatusList = await _adminService.GetPEEnrollmentRepresentativeStatusList();

                if (model.PEDetails == null)
                {
                    model.PEDetails = new PEDetails();
                    HttpContext.Session.Set("SessionPEDetailsDeleted", true);
                }
                else
                {
                    HttpContext.Session.Set("SessionPEDetailsDeleted", model.PEDetails.IsDeleted);
                }
            }
            catch (Exception ex)
            { }
            return PartialView("_PEDetails", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePEDetails([FromBody] PEDetails pEDetails)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();

            string message = "";
            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PEDetails = pEDetails;

                await _adminService.InsertPEDetails(model, userId);

                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePEDetails(int Id)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeletePEDetails(Id, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePEFiles(IFormFile fileGovBankLetter, IFormFile filePrevBank, IFormFile fileNonGovBankLetter, int Id)
        {
            var userId = HttpContext.Session.GetString("UserID");
            ProviderEntrollment providerEntrollment = new ProviderEntrollment();
            try
            {
                string GovBankLetterName = "";
                string OrgGovBankLetterName = "";
                string Direc_GovBankLetterName = "";
                string SaveGovBankLetterpath = "";
                string fullGovBankLetterPaths = "";

                string PrevBankName = "";
                string OrgPrevBankName = "";
                string Direc_PrevBankName = "";
                string SavePrevBankpath = "";
                string fullPrevBankPaths = "";

                string NonGovBankLetterName = "";
                string OrgNonGovBankLetterName = "";
                string Direc_NonGovBankLetterName = "";
                string SaveNonGovBankLetterpath = "";
                string fullNonGovBankLetterPaths = "";

                if (fileGovBankLetter != null)
                {
                    OrgGovBankLetterName = fileGovBankLetter.FileName;
                    //var clientdata = clients.ClientsDetailsList.FirstOrDefault();
                    GovBankLetterName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + fileGovBankLetter.FileName;
                    Direc_GovBankLetterName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + fileGovBankLetter.FileName;

                    SaveGovBankLetterpath = Path.Combine(StorageRoot, Direc_GovBankLetterName);
                    fullGovBankLetterPaths = Path.Combine(StorageRoot);

                    if (!Directory.Exists(fullGovBankLetterPaths))
                        Directory.CreateDirectory(fullGovBankLetterPaths);

                    if (Directory.Exists(fullGovBankLetterPaths))
                    {
                        using (var stream = new FileStream(SaveGovBankLetterpath, FileMode.Create))
                        {
                            await fileGovBankLetter.CopyToAsync(stream);
                            stream.Flush();
                            stream.Close();
                        }
                    }
                }

                if (filePrevBank != null)
                {
                    OrgPrevBankName = filePrevBank.FileName;
                    //var clientdata = clients.ClientsDetailsList.FirstOrDefault();
                    PrevBankName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + filePrevBank.FileName;
                    Direc_PrevBankName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + filePrevBank.FileName;

                    SavePrevBankpath = Path.Combine(StorageRoot, Direc_PrevBankName);
                    fullPrevBankPaths = Path.Combine(StorageRoot);

                    if (!Directory.Exists(fullPrevBankPaths))
                        Directory.CreateDirectory(fullPrevBankPaths);

                    if (Directory.Exists(fullPrevBankPaths))
                    {
                        using (var stream = new FileStream(SavePrevBankpath, FileMode.Create))
                        {
                            await filePrevBank.CopyToAsync(stream);
                            stream.Flush();
                            stream.Close();
                        }
                    }
                }

                if (fileNonGovBankLetter != null)
                {
                    OrgNonGovBankLetterName = fileNonGovBankLetter.FileName;
                    //var clientdata = clients.ClientsDetailsList.FirstOrDefault();
                    NonGovBankLetterName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + fileNonGovBankLetter.FileName;
                    Direc_NonGovBankLetterName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + fileNonGovBankLetter.FileName;

                    SaveNonGovBankLetterpath = Path.Combine(StorageRoot, Direc_NonGovBankLetterName);
                    fullNonGovBankLetterPaths = Path.Combine(StorageRoot);

                    if (!Directory.Exists(fullNonGovBankLetterPaths))
                        Directory.CreateDirectory(fullNonGovBankLetterPaths);

                    if (Directory.Exists(fullNonGovBankLetterPaths))
                    {
                        using (var stream = new FileStream(SaveNonGovBankLetterpath, FileMode.Create))
                        {
                            await fileNonGovBankLetter.CopyToAsync(stream);
                            stream.Flush();
                            stream.Close();
                        }
                    }
                }

                await _adminService.UpdatePEFileDetails(Id, GovBankLetterName, OrgGovBankLetterName, PrevBankName, OrgPrevBankName, NonGovBankLetterName, OrgNonGovBankLetterName, userId);
            }
            catch (Exception ex)
            {

            }
            //return PartialView("_ClientsInsuranceContractList", clients);
            return Json(new { Message = "Success" });
        }

        [HttpPost]
        public async Task<IActionResult> InsuranceContractList([FromBody] int Id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.InsuranceContractList = await _adminService.GetInsuranceContract(0);
                model.PEInsuranceContractFileDetailsList = await _adminService.GetPEInsuranceContractFileDetailsList(Id);
                //totalrows = (model.PEInsuranceContractFileDetailsList.Count > 0 ? model.PEInsuranceContractFileDetailsList.Count : 0);
                //totalRowsAfterFiltering = (model.PEInsuranceContractFileDetailsList.Count > 0 ? model.PEInsuranceContractFileDetailsList.Count : 0);
            }
            catch (Exception Ex) { }
            //return Json(new { data = model.PEInsuranceContractFileDetailsList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
            return PartialView("_PEContracts", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPEContract(int Id)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                model.PEInsuranceContracts = await _adminService.GetPEInsuranceContractsByID(Id);
            }
            catch (Exception ex)
            { }
            return Json(new { data = model.PEInsuranceContracts, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });

        }

        [HttpPost]
        public async Task<IActionResult> DeletePEContracts(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            Clients model = new Clients();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeleteClientInsuranceContract(ID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePEInsuranceContracts([FromBody] PEInsuranceContracts pEInsuranceContracts)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();

            string message = "";
            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PEInsuranceContracts = pEInsuranceContracts;


                await _adminService.InsertPEInsuranceContracts(model, userId);

                message = "Success";


            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePEOtherAddress([FromBody] PEOtherAddress pEOtherAddress)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();

            string message = "";
            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PEOtherAddress = pEOtherAddress;


                await _adminService.InsertPEOtherAddress(model, userId);

                message = "Success";


            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<IActionResult> EditPEOtherAddress(int Id)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                model.PEOtherAddress = await _adminService.GetPEOtherAddressesByID(Id);

                HttpContext.Session.Set("CopyPEOtherAddressID", "0");
            }
            catch (Exception ex)
            { }
            return Json(new { data = model.PEOtherAddress, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });

        }

        [HttpPost]
        public async Task<IActionResult> DeletePEOtherAddress(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeletePEOtherAddresses(ID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<IActionResult> CopyPEOtherAddressID(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }

            HttpContext.Session.Set("CopyPEOtherAddressID", ID.ToString());
            string message = "";
            message = "Success";

            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<IActionResult> PEOtherAddressList([FromBody] int id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.PEAddressTypeList = await _adminService.GetPEAddressTypeList();
                model.PEOtherAddressList = await _adminService.GetPEOtherAddresses(id);
                //totalrows = (model.PEOtherAddressList.Count > 0 ? model.PEOtherAddressList.Count : 0);
                //totalRowsAfterFiltering = (model.PEOtherAddressList.Count > 0 ? model.PEOtherAddressList.Count : 0);
            }
            catch (Exception Ex) { }
            //return Json(new { data = model.PEOtherAddressList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
            return PartialView("_PEOtherAddresses", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePEContacts([FromBody] PEContacts pEContacts)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();

            string message = "";
            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PEContacts = pEContacts;

                await _adminService.InsertPEContacts(model, userId);

                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePEEFT([FromBody] PEEFT pEEFT)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();

            string message = "";
            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PEEFT = pEEFT;

                await _adminService.InsertPEEFT(model, userId);

                message = "Success";


            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }
        [HttpPost]
        public async Task<IActionResult> PEEFTDetailList([FromBody] int id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.CitiesList = new List<CitiesList>();
                model.PEEFTList = await _adminService.GetPEEFTList(id);
            }
            catch (Exception Ex) { }
            return PartialView("_PEEFT", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditPEEFT(int Id)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                model.PEEFT = await _adminService.GetPEEFTByID(Id);
            }
            catch (Exception ex)
            { }
            return Json(new { data = model.PEEFT, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });

        }
        [HttpPost]
        public async Task<IActionResult> DeletePEEFT(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeletePEEFT(ID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePERefusedEnrollment([FromBody] PERefusedEnrollment pERefusedEnrollment)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();

            string message = "";
            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PERefusedEnrollment = pERefusedEnrollment;


                await _adminService.InsertPERefusedEnrollment(model, userId);

                message = "Success";


            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<IActionResult> PERefusedEnrollmentDetailList([FromBody] int id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.CitiesList = new List<CitiesList>();
                model.PERefusedEnrollmentList = await _adminService.GetPERefusedEnrollmentList(id);
            }
            catch (Exception Ex) { }
            return PartialView("_PERefusedEnrollment", model);
        }
        [HttpPost]
        public async Task<IActionResult> EditPERefusedEnrollment(int Id)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                model.PERefusedEnrollment = await _adminService.GetPERefusedEnrollmentByID(Id);
            }
            catch (Exception ex)
            { }
            return Json(new { data = model.PERefusedEnrollment, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });

        }

        [HttpPost]
        public async Task<IActionResult> DeletePERefusedEnrollment(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeletePERefusedEnrollment(ID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<IActionResult> EditPEContacts(int Id)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                model.PEContacts = await _adminService.GetPEContactsByID(Id);
            }
            catch (Exception ex)
            { }
            return Json(new { data = model.PEContacts, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });

        }

        [HttpPost]
        public async Task<IActionResult> DeletePEContacts(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeletePEContacts(ID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<IActionResult> PEContacstList([FromBody] int id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.PEContactTypeList = await _adminService.GetPEContactTypeList();
                model.PEContactsList = await _adminService.GetPEContactsList(id);
                //totalrows = (model.PEContactsList.Count > 0 ? model.PEContactsList.Count : 0);
                //totalRowsAfterFiltering = (model.PEContactsList.Count > 0 ? model.PEContactsList.Count : 0);
            }
            catch (Exception Ex) { }
            //return Json(new { data = model.PEContactsList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
            return PartialView("_PEContacts", model);
        }

        [HttpPost]
        public async Task<IActionResult> PEAuditLogList(int id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.PEAuditLogList = await _adminService.GetAuditLog(id);
                totalrows = (model.PEAuditLogList.Count > 0 ? model.PEAuditLogList.Count : 0);
                totalRowsAfterFiltering = (model.PEAuditLogList.Count > 0 ? model.PEAuditLogList.Count : 0);
            }
            catch (Exception Ex) { }
            return Json(new { data = model.PEAuditLogList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePEDocuments([FromBody] PEDocuments pEDocuments)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();

            string message = "";
            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PEDocuments = pEDocuments;


                model.PEDocuments = await _adminService.InsertPEDocuments(model, userId);

                message = "Success";


            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { DocumentsId = model.PEDocuments.Id });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveDocumentFiles(IFormFile file, int PEDocumentsId)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            var userId = HttpContext.Session.GetString("UserID");
            ProviderEntrollment providerEntrollment = new ProviderEntrollment();
            try
            {
                if (file != null)
                {
                    //var clientdata = clients.ClientsDetailsList.FirstOrDefault();
                    string FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Direc_FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;

                    string Savepath = Path.Combine(StorageRoot, Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot);

                    if (!Directory.Exists(fullPaths))
                        Directory.CreateDirectory(fullPaths);

                    if (Directory.Exists(fullPaths))
                    {
                        using (var stream = new FileStream(Savepath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                            stream.Flush();
                            stream.Close();
                        }
                    }

                    await _adminService.UpdatePEDocumentsFileDetails(PEDocumentsId, 0, "", FileName, Savepath, file.FileName, userId);

                }
            }
            catch (Exception ex)
            {

            }
            //return PartialView("_ClientsInsuranceContractList", clients);
            return Json(new { Message = "Success" });
        }

        //[HttpPost]
        //public async Task<IActionResult> EditPEDocuments(int Id)
        //{
        //    ProviderEntrollment model = new ProviderEntrollment();
        //    try
        //    {
        //        model.PEDocuments = await _adminService.GetPEDocumentsByID(Id);
        //    }
        //    catch (Exception ex)
        //    { }
        //    return Json(new { data = model.PEDocuments, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });

        //}

        [HttpPost]
        public async Task<IActionResult> DeletePEDocuments(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeletePEDocuments(ID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<IActionResult> PEDocumentsList([FromBody] int id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.PEDocumentTitleList = await _adminService.GetPEDocumentTitleList();
                model.PEDocumentsList = await _adminService.GetPEDocumentsList(id);
                //totalrows = (model.PEDocumentsList.Count > 0 ? model.PEDocumentsList.Count : 0);
                //totalRowsAfterFiltering = (model.PEDocumentsList.Count > 0 ? model.PEDocumentsList.Count : 0);
            }
            catch (Exception Ex) { }
            //return Json(new { data = model.PEDocumentsList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
            return PartialView("_PEDocuments", model);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailToClient(int ClientDetailsID, string Subject, string Body, string MailTo, string MailCC, string Attachment)
        {
            try
            {
                //string cc = "";
                //string[] mailIds = MailTo.Split(",");
                Body = Body.Replace("\n", "<br/>");
                Body = Body + "<br/><br/>";
                await _menuUtils.SetMenu(HttpContext.Session);
                var userId = HttpContext.Session.GetString("UserID");
                MailCC = MailCC == null ? "" : MailCC;

                SMTP = await _commonService.GetSMTPServerDetails(userId);

                string FileAttachement = "";
                string[] strArrAttachement = Attachment.Split(",");

                foreach (var item in strArrAttachement)
                {
                    if (item != "")
                    {
                        FileAttachement = FileAttachement + Path.Combine(StorageRoot + item) + ",";
                    }
                }

                if (FileAttachement.Length > 1)
                {
                    FileAttachement = FileAttachement.Substring(0, FileAttachement.Length - 1);
                }

                if (SMTP.Count > 0)
                {
                    CommonMail.SendMail(SMTP[0].SMTPServer, SMTP[0].SMTPPort, SMTP[0].SMTPSSL
                        , SMTP[0].SMTPUserName, SMTP[0].SMTPPassword
                        , MailTo, Subject, Body, MailCC, FileAttachement);
                }

                await _adminService.InsertMailHistory(ClientDetailsID, userId, MailTo, MailCC, Subject, Body, FileAttachement);

            }
            catch (Exception ex) { }
            return Json(new { msge = "Success" });
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePECredentialingLicense([FromBody] PECredentialingLicense pECredentialingLicense)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();

            string message = "";
            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PECredentialingLicense = pECredentialingLicense;


                await _adminService.InsertPECredentialingLicense(model, userId);

                message = "Success";


            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<IActionResult> EditPECredentialingLicense(int Id)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                model.PECredentialingLicense = await _adminService.GetPECredentialingLicenseByID(Id);
            }
            catch (Exception ex)
            { }
            return Json(new { data = model.PECredentialingLicense, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });

        }

        [HttpPost]
        public async Task<IActionResult> DeletePECredentialingLicense(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeletePECredentialingLicense(ID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        [HttpPost]
        public async Task<IActionResult> PECredentialingLicenseList([FromBody] int id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {

                model.PECertificationLevelList = await _adminService.GetPECertificationLevelList();
                model.PELicenseTypeList = await _adminService.GetPELicenseTypeList();
                model.PECredentialingLicenseList = await _adminService.GetPECredentialingLicenseList(id);
                //totalrows = (model.PECredentialingLicenseList.Count > 0 ? model.PECredentialingLicenseList.Count : 0);
                //totalRowsAfterFiltering = (model.PECredentialingLicenseList.Count > 0 ? model.PECredentialingLicenseList.Count : 0);
            }
            catch (Exception Ex) { }
            //return Json(new { data = model.PECredentialingLicenseList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
            return PartialView("_PECredentialingLicense", model);
        }


        public FileResult ViewPEDetailsFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(StorageRoot) + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }



        [HttpPost]
        public async Task<IActionResult> DownloadW9Form([FromBody] int ClientId)
        {
            List<PEDetailsList> peDetailsList = new List<PEDetailsList>();
            peDetailsList = await _adminService.GetPEDetailsList(ClientId);

            string strURL = string.Empty;
            strURL = await FW9(peDetailsList, ClientId);

            return Json(strURL);
        }


        public async Task<string> FW9(List<PEDetailsList> peDetailsList, int ClientId)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string pdfTemplate = Path.Combine(sWebRootFolder, "W9_Unlocked.pdf");
            string FilePath = "";
            string newFilePath = "";
            string newFileName = "";
            string UrlBase = "";

            if (ClientId == 0)
            {
                FilePath = Path.Combine(sWebRootFolder, "W9");
                try
                {
                    if (Directory.Exists(FilePath))
                    {
                        string[] files = Directory.GetFiles(FilePath);

                        foreach (string file in files)
                        {
                            System.IO.File.SetAttributes(file, FileAttributes.Normal);
                            System.IO.File.Delete(file);
                        }
                        Directory.Delete(FilePath);
                    }
                }
                catch (Exception ex)
                {

                }
                Directory.CreateDirectory(FilePath);
            }
            else
            {
                FilePath = sWebRootFolder;
            }


            for (int i = 0; i < peDetailsList.Count; i++)
            {
                try
                {
                    newFileName = peDetailsList[i].ClientID + "_" + peDetailsList[i].ClientName + "_W9.pdf";

                    UrlBase = _configuration["AppSettings:AppURL"].ToString() + newFileName;

                    newFilePath = FilePath + "\\" + newFileName;


                    PdfReader pdfReader = new PdfReader(pdfTemplate);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFilePath, FileMode.Create));
                    AcroFields pdfFormFields = pdfStamper.AcroFields;

                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_1[0]", peDetailsList[i].ClientName);
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].FederalClassification[0].f1_4[0]", peDetailsList[i].TaxIDClassification.ToString());
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].FederalClassification[0].c1_1[6]", "7");

                    //pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_7[0]", peDetailsList[i].PracticeLocation);
                    //pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_8[0]", peDetailsList[i].City + "," + peDetailsList[i].State + " " + peDetailsList[i].Zip);

                    List<PEOtherAddressList> peOtherAddressList = new List<PEOtherAddressList>();
                    peOtherAddressList = await _adminService.GetPEOtherAddresses(ClientId);
                    for (int j = 0; j < peOtherAddressList.Count; j++)
                    {
                        if (peOtherAddressList[j].AddressTypeID == 3)
                        {
                            pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_7[0]", peOtherAddressList[j].AddressLine1.ToString().ToUpper());
                            pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_8[0]", peOtherAddressList[j].City.ToString().ToUpper() + "," + peOtherAddressList[j].State.ToString().ToUpper() + " " + peOtherAddressList[j].ZipCode.ToString().ToUpper());
                        }
                    }

                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_10[0]", "NPI - " + peDetailsList[i].GroupNPI);
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].EmployerID[0].f1_14[0]", peDetailsList[i].TaxID == "" ? "" : peDetailsList[i].TaxID.Substring(0, 2));
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].EmployerID[0].f1_15[0]", peDetailsList[i].TaxID == "" ? "" : peDetailsList[i].TaxID.Substring(3, peDetailsList[i].TaxID.Length - 3));
                    pdfFormFields.SetField("DateSigned", DateTime.Now.ToString("MM/dd/yyyy"));

                    pdfStamper.FormFlattening = false;

                    pdfStamper.Close();
                }
                catch (Exception ex)
                {

                }
            }

            if (ClientId == 0)
            {
                try
                {
                    if (System.IO.File.Exists(FilePath + ".zip"))
                    {
                        System.IO.File.Delete(FilePath + ".zip");
                    }

                    ZipFile.CreateFromDirectory(FilePath, FilePath + ".zip");
                    UrlBase = _configuration["AppSettings:AppURL"].ToString() + "W9.zip";
                }
                catch (Exception ex)
                {

                }
            }

            return UrlBase;

        }

        [HttpPost]
        public async Task<IActionResult> DownloadW91099Form([FromBody] int ClientId)
        {
            List<PEDetailsList> peDetailsList = new List<PEDetailsList>();
            peDetailsList = await _adminService.GetPEDetailsList(ClientId);

            string strURL = string.Empty;
            strURL = await FW91099(peDetailsList, ClientId);

            return Json(strURL);
        }

        public async Task<string> FW91099(List<PEDetailsList> peDetailsList, int ClientId)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string pdfTemplate = Path.Combine(sWebRootFolder, "W9_Unlocked.pdf");
            string FilePath = "";
            string newFilePath = "";
            string newFileName = "";
            string UrlBase = "";

            if (ClientId == 0)
            {
                FilePath = Path.Combine(sWebRootFolder, "W9(1099)");
                try
                {
                    if (Directory.Exists(FilePath))
                    {
                        string[] files = Directory.GetFiles(FilePath);

                        foreach (string file in files)
                        {
                            System.IO.File.SetAttributes(file, FileAttributes.Normal);
                            System.IO.File.Delete(file);
                        }
                        Directory.Delete(FilePath);
                    }
                }
                catch (Exception ex)
                {

                }
                Directory.CreateDirectory(FilePath);
            }
            else
            {
                FilePath = sWebRootFolder;
            }


            for (int i = 0; i < peDetailsList.Count; i++)
            {
                try
                {
                    newFileName = peDetailsList[i].ClientID + "_" + peDetailsList[i].ClientName + "_W9(1099).pdf";

                    UrlBase = _configuration["AppSettings:AppURL"].ToString() + newFileName;

                    newFilePath = FilePath + "\\" + newFileName;


                    PdfReader pdfReader = new PdfReader(pdfTemplate);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFilePath, FileMode.Create));
                    AcroFields pdfFormFields = pdfStamper.AcroFields;

                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_1[0]", peDetailsList[i].ClientName);
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].FederalClassification[0].f1_4[0]", peDetailsList[i].TaxIDClassification.ToString());
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].FederalClassification[0].c1_1[6]", "7");

                    //pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_7[0]", peDetailsList[i].PracticeLocation);
                    //pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_8[0]", peDetailsList[i].City + "," + peDetailsList[i].State + " " + peDetailsList[i].Zip);

                    List<PEOtherAddressList> peOtherAddressList = new List<PEOtherAddressList>();
                    peOtherAddressList = await _adminService.GetPEOtherAddresses(ClientId);
                    for (int j = 0; j < peOtherAddressList.Count; j++)
                    {
                        if (peOtherAddressList[j].AddressTypeID == 1)
                        {
                            pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_7[0]", peOtherAddressList[j].AddressLine1.ToString().ToUpper());
                            pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_8[0]", peOtherAddressList[j].City.ToString().ToUpper() + "," + peOtherAddressList[j].State.ToString().ToUpper() + " " + peOtherAddressList[j].ZipCode.ToString().ToUpper());
                        }
                    }

                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_10[0]", "NPI - " + peDetailsList[i].GroupNPI);
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].EmployerID[0].f1_14[0]", peDetailsList[i].TaxID == "" ? "" : peDetailsList[i].TaxID.Substring(0, 2));
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].EmployerID[0].f1_15[0]", peDetailsList[i].TaxID == "" ? "" : peDetailsList[i].TaxID.Substring(3, peDetailsList[i].TaxID.Length - 3));
                    pdfFormFields.SetField("DateSigned", DateTime.Now.ToString("MM/dd/yyyy"));

                    pdfStamper.FormFlattening = false;

                    pdfStamper.Close();
                }
                catch (Exception ex)
                {

                }
            }

            if (ClientId == 0)
            {
                try
                {
                    if (System.IO.File.Exists(FilePath + ".zip"))
                    {
                        System.IO.File.Delete(FilePath + ".zip");
                    }

                    ZipFile.CreateFromDirectory(FilePath, FilePath + ".zip");
                    UrlBase = _configuration["AppSettings:AppURL"].ToString() + "W9.zip";
                }
                catch (Exception ex)
                {

                }
            }

            return UrlBase;

        }

        [HttpPost]
        public async Task<IActionResult> DownloadTricareForm([FromBody] int ClientId)
        {
            List<PEDetailsList> peDetailsList = new List<PEDetailsList>();
            peDetailsList = await _adminService.GetPEDetailsList(ClientId);

            string strURL = string.Empty;
            strURL = await TricareForm(peDetailsList, ClientId);

            return Json(strURL);
        }

        public async Task<string> TricareForm(List<PEDetailsList> peDetailsList, int ClientId)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string pdfTemplate = Path.Combine(sWebRootFolder, "BLANK_TRICARE_APPLICATION.pdf");
            string FilePath = "";
            string newFilePath = "";
            string newFileName = "";
            string UrlBase = "";

            if (ClientId == 0)
            {
                FilePath = Path.Combine(sWebRootFolder, "Tricare");
                try
                {
                    if (Directory.Exists(FilePath))
                    {
                        string[] files = Directory.GetFiles(FilePath);

                        foreach (string file in files)
                        {
                            System.IO.File.SetAttributes(file, FileAttributes.Normal);
                            System.IO.File.Delete(file);
                        }
                        Directory.Delete(FilePath);
                    }
                }
                catch (Exception ex)
                {

                }
                Directory.CreateDirectory(FilePath);
            }
            else
            {
                FilePath = sWebRootFolder;
            }


            for (int i = 0; i < peDetailsList.Count; i++)
            {
                try
                {
                    newFileName = peDetailsList[i].ClientID + "_" + peDetailsList[i].ClientName + "_Tricare.pdf";

                    UrlBase = _configuration["AppSettings:AppURL"].ToString() + newFileName;

                    newFilePath = FilePath + "\\" + newFileName;


                    PdfReader pdfReader = new PdfReader(pdfTemplate);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFilePath, FileMode.Create));
                    AcroFields pdfFormFields = pdfStamper.AcroFields;

                    pdfFormFields.SetField("Facility tax ID", peDetailsList[i].TaxID);
                    pdfFormFields.SetField("Facility NPI", peDetailsList[i].GroupNPI.ToString());
                    pdfFormFields.SetField("Request date", DateTime.Now.ToString("MM/dd/yyyy"));
                    pdfFormFields.SetField("Name of facility", peDetailsList[i].ClientName);
                    pdfFormFields.SetField("Federal tax ID", peDetailsList[i].TaxID);
                    pdfFormFields.SetField("National Provider Indentifier NPI", peDetailsList[i].GroupNPI);
                    pdfFormFields.SetField("Office phone", peDetailsList[i].Phone1);
                    pdfFormFields.SetField("Billing phone", "800-962-1484");
                    pdfFormFields.SetField("Fax", "513-527-0659");
                    pdfFormFields.SetField("GarageLocation street address", peDetailsList[i].PracticeLocation);
                    pdfFormFields.SetField("City", peDetailsList[i].City);
                    pdfFormFields.SetField("State", peDetailsList[i].State);
                    pdfFormFields.SetField("ZIP", peDetailsList[i].Zip);

                    List<PEOtherAddressList> peOtherAddressList = new List<PEOtherAddressList>();
                    peOtherAddressList = await _adminService.GetPEOtherAddresses(ClientId);
                    for (int j = 0; j < peOtherAddressList.Count; j++)
                    {
                        if (peOtherAddressList[j].AddressTypeID == 3)
                        {
                            pdfFormFields.SetField("Billing address if different", peOtherAddressList[j].AddressLine1);
                            pdfFormFields.SetField("City_2", peOtherAddressList[j].City);
                            pdfFormFields.SetField("State_2", peOtherAddressList[j].State);
                            pdfFormFields.SetField("ZIP_2", peOtherAddressList[j].ZipCode);
                        }
                    }

                    List<PECredentialingLicenseList> peCredentialingLicenseList = new List<PECredentialingLicenseList>();
                    peCredentialingLicenseList = await _adminService.GetPECredentialingLicenseList(ClientId);
                    for (int k = 0; k < peCredentialingLicenseList.Count; k++)
                    {
                        if (peDetailsList[i].State != "OHIO" && peDetailsList[i].State != "OH")
                        {
                            pdfFormFields.SetField("License", peCredentialingLicenseList[k].LicenseNo);
                            pdfFormFields.SetField("Issuing state", peDetailsList[i].PracticeLocation);
                            pdfFormFields.SetField("License issue date", peCredentialingLicenseList[k].IssuedDate);
                            pdfFormFields.SetField("Expiration date", peCredentialingLicenseList[k].ExpiryDate);
                        }
                    }

                    pdfFormFields.SetField("Medicare", peDetailsList[i].MedicareNumber);

                    pdfStamper.FormFlattening = false;

                    pdfStamper.Close();
                }
                catch (Exception ex)
                {

                }
            }

            if (ClientId == 0)
            {
                try
                {
                    if (System.IO.File.Exists(FilePath + ".zip"))
                    {
                        System.IO.File.Delete(FilePath + ".zip");
                    }

                    ZipFile.CreateFromDirectory(FilePath, FilePath + ".zip");
                    UrlBase = _configuration["AppSettings:AppURL"].ToString() + "Tricare.zip";
                }
                catch (Exception ex)
                {

                }
            }

            return UrlBase;

        }

        [HttpPost]
        public async Task<IActionResult> DownloadMMPIForm([FromBody] int ClientId)
        {
            List<PEDetailsList> peDetailsList = new List<PEDetailsList>();
            peDetailsList = await _adminService.GetPEDetailsList(ClientId);

            string strURL = string.Empty;
            strURL = await MMPIForm(peDetailsList, ClientId);

            return Json(strURL);
        }

        public async Task<string> MMPIForm(List<PEDetailsList> peDetailsList, int ClientId)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string pdfTemplate = Path.Combine(sWebRootFolder, "MedicalMutualProviderInformationForm.pdf");
            string FilePath = "";
            string newFilePath = "";
            string newFileName = "";
            string UrlBase = "";

            if (ClientId == 0)
            {
                FilePath = Path.Combine(sWebRootFolder, "MMPI");
                try
                {
                    if (Directory.Exists(FilePath))
                    {
                        string[] files = Directory.GetFiles(FilePath);

                        foreach (string file in files)
                        {
                            System.IO.File.SetAttributes(file, FileAttributes.Normal);
                            System.IO.File.Delete(file);
                        }
                        Directory.Delete(FilePath);
                    }
                }
                catch (Exception ex)
                {

                }
                Directory.CreateDirectory(FilePath);
            }
            else
            {
                FilePath = sWebRootFolder;
            }


            for (int i = 0; i < peDetailsList.Count; i++)
            {
                try
                {
                    newFileName = peDetailsList[i].ClientID + "_" + peDetailsList[i].ClientName + "_MMPI.pdf";

                    UrlBase = _configuration["AppSettings:AppURL"].ToString() + newFileName;

                    newFilePath = FilePath + "\\" + newFileName;
                    //newFilePath = FilePath + "\\MedicalMutual_W9\\" + newFileName;


                    PdfReader pdfReader = new PdfReader(pdfTemplate);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFilePath, FileMode.Create));
                    AcroFields pdfFormFields = pdfStamper.AcroFields;

                    pdfFormFields.SetField("SLI_InfoEffectiveDate", DateTime.Now.ToString("MM/dd/yyyy"));
                    pdfFormFields.SetField("SLI_TIN", peDetailsList[i].TaxID);
                    pdfFormFields.SetField("SLI_Facility or Group Name", peDetailsList[i].ClientName.ToString());
                    pdfFormFields.SetField("SLI_Street Address", peDetailsList[i].PracticeLocation);
                    pdfFormFields.SetField("SLI_City", peDetailsList[i].City);
                    pdfFormFields.SetField("SLI_State", peDetailsList[i].State);
                    pdfFormFields.SetField("SLI_Zip", peDetailsList[i].Zip);
                    pdfFormFields.SetField("SLI_County", peDetailsList[i].County);
                    pdfFormFields.SetField("SLI_Office Phone", peDetailsList[i].Phone1);
                    pdfFormFields.SetField("SLI_Fax", peDetailsList[i].Fax);
                    pdfFormFields.SetField("SLI_specialty", "Ambulance - Land Transport");
                    pdfFormFields.SetField("SLI_NPI No", peDetailsList[i].GroupNPI);
                    pdfFormFields.SetField("RAI_Reimbursement Name", peDetailsList[i].ClientName);
                    pdfFormFields.SetField("RAI_Reimbursement Entity's Tin", peDetailsList[i].TaxID);
                    pdfFormFields.SetField("RAI_Date", DateTime.Now.ToString("MM/dd/yyyy"));

                    pdfFormFields.SetField("RAI_Phone", "800-962-1484");
                    pdfFormFields.SetField("RAI_Fax", "513-527-0659");

                    pdfFormFields.SetField("RAI_Office Manager", "AMY KING");
                    pdfFormFields.SetField("RAI_Office Manager Phone", " 513-612-3158");
                    pdfFormFields.SetField("RAI_OM_Email", "PE@medicount.com");
                    pdfFormFields.SetField("RAI_OM_Date", DateTime.Now.ToString("MM/dd/yyyy"));

                    List<PEOtherAddressList> peOtherAddressList = new List<PEOtherAddressList>();
                    peOtherAddressList = await _adminService.GetPEOtherAddresses(ClientId);
                    for (int j = 0; j < peOtherAddressList.Count; j++)
                    {
                        if (peOtherAddressList[j].AddressTypeID == 3)
                        {
                            pdfFormFields.SetField("SLI_Correspondence Address", peOtherAddressList[j].AddressLine1 + ", " + peOtherAddressList[j].City + ", " + peOtherAddressList[j].State + " " + peOtherAddressList[j].ZipCode);
                            pdfFormFields.SetField("RAI_Address", peOtherAddressList[j].AddressLine1);
                            pdfFormFields.SetField("RAI_City", peOtherAddressList[j].City);
                            pdfFormFields.SetField("RAI_State", peOtherAddressList[j].State);
                            pdfFormFields.SetField("RAI_Zip", peOtherAddressList[j].ZipCode);
                        }
                    }

                    pdfStamper.FormFlattening = false;

                    pdfStamper.Close();
                }
                catch (Exception ex)
                {

                }
            }

            if (ClientId == 0)
            {
                try
                {
                    if (System.IO.File.Exists(FilePath + ".zip"))
                    {
                        System.IO.File.Delete(FilePath + ".zip");
                    }

                    ZipFile.CreateFromDirectory(FilePath, FilePath + ".zip");
                    UrlBase = _configuration["AppSettings:AppURL"].ToString() + "MMPI.zip";
                }
                catch (Exception ex)
                {

                }
            }

            return UrlBase;

        }

        [HttpPost]
        public async Task<IActionResult> DownloadMedcoForm([FromBody] int ClientId)
        {
            List<PEDetailsList> peDetailsList = new List<PEDetailsList>();
            peDetailsList = await _adminService.GetPEDetailsList(ClientId);

            string strURL = string.Empty;
            strURL = await MedcoForm(peDetailsList, ClientId);

            return Json(strURL);
        }

        public async Task<string> MedcoForm(List<PEDetailsList> peDetailsList, int ClientId)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string pdfTemplate = Path.Combine(sWebRootFolder, "Medco.pdf");
            string FilePath = "";
            string newFilePath = "";
            string newFileName = "";
            string UrlBase = "";

            if (ClientId == 0)
            {
                FilePath = Path.Combine(sWebRootFolder, "Medco");
                try
                {
                    if (Directory.Exists(FilePath))
                    {
                        string[] files = Directory.GetFiles(FilePath);

                        foreach (string file in files)
                        {
                            System.IO.File.SetAttributes(file, FileAttributes.Normal);
                            System.IO.File.Delete(file);
                        }
                        Directory.Delete(FilePath);
                    }
                }
                catch (Exception ex)
                {

                }
                Directory.CreateDirectory(FilePath);
            }
            else
            {
                FilePath = sWebRootFolder;
            }


            for (int i = 0; i < peDetailsList.Count; i++)
            {
                try
                {
                    newFileName = peDetailsList[i].ClientID + "_" + peDetailsList[i].ClientName + "_Medco.pdf";

                    UrlBase = _configuration["AppSettings:AppURL"].ToString() + newFileName;

                    newFilePath = FilePath + "\\" + newFileName;


                    PdfReader pdfReader = new PdfReader(pdfTemplate);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFilePath, FileMode.Create));
                    AcroFields pdfFormFields = pdfStamper.AcroFields;

                    pdfFormFields.SetField("02 AmbulanceCB", "Yes");


                    List<ClientInsuranceContractFileDetails> ContractList = await _commonService.GetClientInsuranceContractFileDetailsList(ClientId);

                    for (int j = 0; j < ContractList.Count; j++)
                    {
                        if (ContractList[j].InsuranceContractName == "BWC")//BWC provider number
                        {
                            pdfFormFields.SetField("GI_ProviderNumber", ContractList[j].ProviderNo.ToUpper());
                        }
                    }

                    pdfFormFields.SetField("GI_TIN", peDetailsList[i].TaxID.ToUpper());
                    pdfFormFields.SetField("GI_SSN#", "NA");
                    pdfFormFields.SetField("GI_Individual Name", peDetailsList[i].ClientName.ToUpper());
                    pdfFormFields.SetField("GI_CB1", "Yes");
                    pdfFormFields.SetField("GI_DBA Name", peDetailsList[i].ClientName.ToUpper());
                    pdfFormFields.SetField("GI_Business Type", peDetailsList[i].TaxIDClassification.ToUpper());
                    pdfFormFields.SetField("GI_NPI", peDetailsList[i].GroupNPI.ToUpper());
                    pdfFormFields.SetField("GI_Taxonomy Code", peDetailsList[i].Taxonomy.ToUpper());
                    pdfFormFields.SetField("GI_Business Owner Names", peDetailsList[i].ClientName + " - 100% OWNERSHIP");
                    pdfFormFields.SetField("GI_WCPolicy", peDetailsList[i].WPPolicyNumber.ToUpper());
                    pdfFormFields.SetField("GI_Practice Location", peDetailsList[i].PracticeLocation.ToUpper());
                    pdfFormFields.SetField("GI_City", peDetailsList[i].City.ToUpper());
                    pdfFormFields.SetField("GI_state", peDetailsList[i].State.ToUpper());
                    pdfFormFields.SetField("GI_Zip Code", peDetailsList[i].Zip.ToUpper());
                    pdfFormFields.SetField("GI_Telephone", "800-962-1484");
                    pdfFormFields.SetField("GI_Fax", "513-527-0659");
                    pdfFormFields.SetField("GI_Email Address", "PE@MEDICOUNT.COM"); //peDetailsList[i].Email);

                    List<PEOtherAddressList> peOtherAddressList = new List<PEOtherAddressList>();
                    peOtherAddressList = await _adminService.GetPEOtherAddresses(ClientId);
                    for (int j = 0; j < peOtherAddressList.Count; j++)
                    {
                        if (peOtherAddressList[j].AddressTypeID == 3)
                        {
                            pdfFormFields.SetField("GI_Reimb Address", peOtherAddressList[j].AddressLine1.ToUpper());
                            pdfFormFields.SetField("GI_Reimb City", peOtherAddressList[j].City.ToUpper());
                            pdfFormFields.SetField("GI_Reimb state", peOtherAddressList[j].State.ToUpper());
                            pdfFormFields.SetField("GI_Reimb Zip", peOtherAddressList[j].ZipCode);
                        }
                    }


                    pdfFormFields.SetField("GI_Corres Address", "PO BOX 392907");
                    pdfFormFields.SetField("GI_Corres City", "PITTSBURGH");
                    pdfFormFields.SetField("GI_Corres State", "PA");
                    pdfFormFields.SetField("GI_Corres Zip Code", "15251-9907");

                    pdfFormFields.SetField("Req_List Medicare", peDetailsList[i].MedicareNumber.ToUpper());
                    pdfFormFields.SetField("GI_MedicareState", peDetailsList[i].State.ToUpper());
                    pdfFormFields.SetField("Req_Medicaid Number", peDetailsList[i].MedicaidNumber.ToUpper());
                    pdfFormFields.SetField("GI_MedicaidState", peDetailsList[i].State.ToUpper());


                    //pdfFormFields.SetField("PQI_26", "26");
                    pdfFormFields.SetField("PIQ_1", "No");

                    //pdfFormFields.SetField("PQI_25", "25");
                    pdfFormFields.SetField("PIQ_2", "No");

                    //pdfFormFields.SetField("PQI_24", "24");
                    pdfFormFields.SetField("PQI_3", "No");

                    //pdfFormFields.SetField("PQI_23", "23");
                    pdfFormFields.SetField("PQI_4", "No");

                    //pdfFormFields.SetField("PQI_22", "22");
                    pdfFormFields.SetField("PQI_5", "No");

                    //pdfFormFields.SetField("PQI_21", "21");
                    pdfFormFields.SetField("PQI_6", "No");

                    //pdfFormFields.SetField("PQI_20", "20");
                    pdfFormFields.SetField("PQI_7", "No");

                    //pdfFormFields.SetField("PQI_19", "19");
                    pdfFormFields.SetField("PQI_8", "No");

                    //pdfFormFields.SetField("PQI_18", "18");
                    pdfFormFields.SetField("PQI_9", "No");

                    //pdfFormFields.SetField("PQI_17", "17");
                    pdfFormFields.SetField("PQI_10", "No");

                    //pdfFormFields.SetField("PQI_16", "16");
                    pdfFormFields.SetField("PQI_11", "No");

                    //pdfFormFields.SetField("PQI_15", "15");
                    pdfFormFields.SetField("PQI_12", "No");

                    //pdfFormFields.SetField("PQI_14", "14");
                    pdfFormFields.SetField("PQI_13", "No");

                    pdfFormFields.SetField("Exp_App Contact", "AMY KING");
                    pdfFormFields.SetField("Exp_Title", "PROVIDER ENROLLMENT MANAGER");
                    pdfFormFields.SetField("EXP_Telep", " 513-612-3158");
                    pdfFormFields.SetField("Exp_Fax", "513-527-0659");
                    pdfFormFields.SetField("Exp_Email", "PE@MEDICOUNT.COM");

                    pdfFormFields.SetField("OwnerName", peDetailsList[i].ClientName);
                    pdfFormFields.SetField("Ownership%", "100%");

                    pdfFormFields.SetField("PrintName", "AMY KING");
                    pdfFormFields.SetField("End Date", DateTime.Now.ToString("MM/dd/yyyy"));

                    //pdfFormFields.SetField("RAI_Phone", "800-962-1484");
                    //pdfFormFields.SetField("RAI_Fax", "513-527-0659");


                    //pdfFormFields.SetField("RAI_OM_Date", DateTime.Now.ToString("MM/dd/yyyy"));



                    pdfStamper.FormFlattening = false;

                    pdfStamper.Close();
                }
                catch (Exception ex)
                {

                }
            }

            if (ClientId == 0)
            {
                try
                {
                    if (System.IO.File.Exists(FilePath + ".zip"))
                    {
                        System.IO.File.Delete(FilePath + ".zip");
                    }

                    ZipFile.CreateFromDirectory(FilePath, FilePath + ".zip");
                    UrlBase = _configuration["AppSettings:AppURL"].ToString() + "Medco.zip";
                }
                catch (Exception ex)
                {

                }
            }

            return UrlBase;

        }

        [HttpPost]
        public async Task<IActionResult> DownloadPEDetails()
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            List<PEDetailsList> modelPEDetails = new List<PEDetailsList>();
            modelPEDetails = await _adminService.GetPEDetailsList(0);
            var dataPEDetails = modelPEDetails.Select(x => new
            {
                ClientID = x.ClientID,
                LegalName = x.ClientName,
                x.Status,
                x.ResidencyIndicator,
                x.PaymentIndicator,
                x.DoingBusinessAs,
                x.OtherName,
                x.HoursofOperation,
                x.AccountRepresentativeName,
                x.PracticeLocation,
                x.AddressLine2,
                x.County,
                x.City,
                x.State,
                x.Zip,
                x.Phone1,
                x.Ext1,
                x.Phone2,
                x.Ext2,
                x.Fax,
                x.Email,
                x.TaxID,
                x.TaxIDIssued,
                x.TaxIDClassification,
                x.Taxonomy,
                x.GroupNPI,
                x.GroupNPIIssued,
                x.MedicaidNumber,
                x.MedicareNumber,
                x.WPPolicyNumber,
                x.InitialBillingSetup,
                x.PreviousBiller,
                x.PreviousBillerContact,
                x.FirstRunDate,
                x.ClientAddedToESOCompany,
                x.MedicareSubmitApplicationThruPecos,
                x.WaystarSubmitTicketToCreateChildAccount,
                x.WaystarProviderSetup,
                x.WaystarEnrollments,
                x.MedicareEDISubmitApplication,
                x.BCBSRequestToJoinNetwork,
                x.MMORequestToJoinNetwork,
                x.AetnaNPISubmission,
                x.MedicaidSubmitApplication,
                x.MedicaidSubmitERAAndOrEDI,
                x.TricareEastSubmitApplication,
                x.RailroadMedicareApplyForPTAN,
                x.RailroadMedicareSubmitERAAndOrEDI,
                x.OHBWCSubmitMedco13Application,
                x.UHCRequestForNonParProvider,
                x.VAVendorForm,                
                x.EnrollmentRepresentativeStatus
            });


            List<ClientInsuranceContractFileDetails> modelPEContracts = new List<ClientInsuranceContractFileDetails>();
            modelPEContracts = await _commonService.GetClientInsuranceContractFileDetailsList(0);
            var dataPEContracts = modelPEContracts.Select(x => new
            {
                ClientID = x.CompanyID,
                LegalName = x.CompanyName,
                x.InsuranceContractName,
                x.Info,
                x.EffectiveDate,
                x.LastRevalidateDate,
                NextRevalidationDate = x.NextUpdateDue,
                x.TermedDate,
                x.ProviderNo
            });


            List<PEOtherAddressList> modelPEOtherAddress = new List<PEOtherAddressList>();
            modelPEOtherAddress = await _adminService.GetPEOtherAddresses(0);
            var dataPEOtherAddress = modelPEOtherAddress.Select(x => new
            {
                ClientID = x.CompanyID,
                LegalName = x.CompanyName,
                x.AddressType,
                x.AddressLine1,
                x.AddressLine2,
                x.City,
                x.State,
                x.ZipCode,
                x.PhoneNumber
            });

            List<PEContactsList> modelPEContacts = new List<PEContactsList>();
            modelPEContacts = await _adminService.GetPEContactsList(0);
            var dataPEContacts = modelPEContacts.Select(x => new
            {
                ClientID = x.CompanyID,
                LegalName = x.CompanyName,
                x.ContactType,
                x.Title,
                x.ContactName,
                x.Address,
                x.Phone,
                x.Fax,
                x.Email
            });

            List<PEDocumentsList> modelPEDocuments = new List<PEDocumentsList>();
            modelPEDocuments = await _adminService.GetPEDocumentsList(0);
            var dataPEDocuments = modelPEDocuments.Select(x => new
            {
                ClientID = x.CompanyID,
                LegalName = x.CompanyName,
                x.Title,
                x.DocumentName,
                x.AddedOn
            });

            List<PECredentialingLicenseList> modelPECredentialingLicense = new List<PECredentialingLicenseList>();
            modelPECredentialingLicense = await _adminService.GetPECredentialingLicenseList(0);
            var dataPECredentialingLicense = modelPECredentialingLicense.Select(x => new
            {
                ClientID = x.CompanyID,
                LegalName = x.CompanyName,
                x.LicenseType,
                x.CertificationLevel,
                x.LicenseNo,
                x.IssuedDate,
                x.EffectiveDate,
                x.ExpiryDate
            });

            List<PENotesList> modelPENotesList = new List<PENotesList>();
            modelPENotesList = await _adminService.GetPENotesListForExcel(0);
            var dataPENotesList = modelPENotesList.Select(x => new
            {
                ClientID = x.ClientId,
                CompanyName = x.CompanyName,
                x.Notes,
                x.AddedBy,
                x.AddedDate
            });

            List<PEEFTList> modelPEEFTList= new List<PEEFTList>();
            modelPEEFTList = await _adminService.GetPEEFTListForExcel(0);
            var dataPEEFTList = modelPEEFTList.Select(x => new
            {
                ClientDetailsID = x.ClientDetailsID,
                CompanyName = x.CompanyName,
                x.EFTPayerName,
                x.EFTBankType,
                x.EFTComments
            });

            List<PEClientsBankInfoList> modelPEClientsBankInfoList = new List<PEClientsBankInfoList>();
            modelPEClientsBankInfoList = await _adminService.GetPEClientsBankInfoListForExcel(0);
            var dataPEClientsBankInfoList = modelPEClientsBankInfoList.Select(x => new
            {
                x.ClientDetailsID,
                x.CompanyName,
                x.BankName,
                x.AccountType,
                x.BankCategory,
                x.EffectiveDate,
                x.EndDate,
                x.AddressLine1,
                x.AddressLine2, 
                x.City,
                x.State,
                x.ZipCode,
                x.RoutingNumber,
                x.AccountNumber

            });
            

            string sFileName = @"ProviderEnrollment.xlsx";// ProviderEnrollment
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
                ExcelWorksheet worksheetDetails = package.Workbook.Worksheets.Add("Details");
                ExcelWorksheet worksheetContract = package.Workbook.Worksheets.Add("Contracts");
                ExcelWorksheet worksheetOtherAddress = package.Workbook.Worksheets.Add("OtherAddress");
                ExcelWorksheet worksheetContacts = package.Workbook.Worksheets.Add("Contacts");
                ExcelWorksheet worksheetDocuments = package.Workbook.Worksheets.Add("Documents");
                ExcelWorksheet worksheetCredentialingLicense = package.Workbook.Worksheets.Add("Credentialing License");
                ExcelWorksheet worksheetNotes = package.Workbook.Worksheets.Add("Notes");
                ExcelWorksheet worksheetEFT = package.Workbook.Worksheets.Add("EFT");
                ExcelWorksheet worksheetClientsBankInfo = package.Workbook.Worksheets.Add("Client's Bank Info");

                worksheetDetails.Cells.LoadFromCollection(dataPEDetails, true);
                worksheetContract.Cells.LoadFromCollection(dataPEContracts, true);
                worksheetOtherAddress.Cells.LoadFromCollection(dataPEOtherAddress, true);
                worksheetContacts.Cells.LoadFromCollection(dataPEContacts, true);
                worksheetDocuments.Cells.LoadFromCollection(dataPEDocuments, true);
                worksheetCredentialingLicense.Cells.LoadFromCollection(dataPECredentialingLicense, true);
                worksheetNotes.Cells.LoadFromCollection(dataPENotesList, true);
                worksheetEFT.Cells.LoadFromCollection(dataPEEFTList, true);
                worksheetClientsBankInfo.Cells.LoadFromCollection(dataPEClientsBankInfoList, true);
                package.Save();
            }
            return Json(UrlBase);
        }

        [HttpPost]
        [AllowAnonymous]   //Changed by mohamed Iqbal
        public async Task<IActionResult> PEChkListSendMailUpdateStatus(int ClientID, string To, string CC, string Subject, string Body
        , string Attachment, string MailFor, string Status, string CheckListFor, string PESinglePUID, string TaskValueAllocatedTo)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            try
            {
                To = To == null ? "" : To;
                CC = CC == null ? "" : CC;
                Subject = Subject == null ? "" : Subject;
                Body = Body == null ? "" : Body;
                await _menuUtils.SetMenu(HttpContext.Session);
                var userId = HttpContext.Session.GetString("UserID");

                string Savepath = "";
                if (!string.IsNullOrEmpty(Attachment))   // changed by Mohamed Iqbal
                {
                    // Get the full path to the file in wwwroot
                    Savepath = Path.Combine(_hostingEnvironment.WebRootPath, Attachment.TrimStart('/'));                   
                }                

                if (To != "" && To != null)
                {
                    SMTP = await _commonService.GetSMTPServerDetails(userId);
                    if (SMTP.Count > 0)
                    {
                        Body = Body.Replace("\n", "<br/>");
                        Body = Body + "<br/><br/>";

                        CommonMail.SendMail(SMTP[0].SMTPServer, SMTP[0].SMTPPort, SMTP[0].SMTPSSL
                            , SMTP[0].SMTPUserName, SMTP[0].SMTPPassword
                            , To, Subject, Body, CC, Savepath);
                    }
                }
                if (CheckListFor == "Billing CheckList")
                {
                    await _adminService.UpdatePECheckListStatus(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
                }
                else if (CheckListFor == "Payment Category Change")
                {
                    await _adminService.UpdatePEPaymentCategoryChangeCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
                }
                else if (CheckListFor == "Single Payer Update")
                {
                    await _adminService.UpdatePESinglePayerUpdateCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath, TaskValueAllocatedTo, PESinglePUID);
                }
                else if (CheckListFor == "Practice Location Change")
                {
                    await _adminService.UpdatePEPracticeLocationChangeCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
                }
                else if (CheckListFor == "Closed Client")
                {
                    await _adminService.UpdatePEClosedClientCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
                }
                else if (CheckListFor == "Client Bank Change")
                {
                    await _adminService.UpdatePEClientBankChangeCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
                }
                else if (CheckListFor == "Commerical EFTs")
                {
                    await _adminService.UpdatePECommericalEFTsCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
                }
                else if (CheckListFor == "Medicare Change of Info")
                {
                    await _adminService.UpdatePEMedicareChangeOfInfoCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
                }
                else if (CheckListFor == "Out of State MCD")
                {
                    await _adminService.UpdatePEOutOfStateMCDCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
                }
            }
            catch (Exception ex) { }
            return Json(new { msge = "Success" });
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> PEChkListSendMailUpdateStatus(int ClientID, string To, string CC, string Subject, string Body
        //   , IFormFile Attachment, string MailFor, string Status, string CheckListFor, string PESinglePUID, string TaskValueAllocatedTo)
        //{
        //    if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
        //    {
        //        return Json("");
        //    }
        //    try
        //    {
        //        To = To == null ? "" : To;
        //        CC = CC == null ? "" : CC;
        //        Subject = Subject == null ? "" : Subject;
        //        Body = Body == null ? "" : Body;
        //        await _menuUtils.SetMenu(HttpContext.Session);
        //        var userId = HttpContext.Session.GetString("UserID");

        //        string Savepath = "";

        //        if (Attachment != null)
        //        {
        //            string FileName = Attachment.FileName;
        //            string Direc_FileName = FileName;

        //            Savepath = Path.Combine(StorageRoot, Direc_FileName);
        //            string fullPaths = Path.Combine(StorageRoot);

        //            if (!Directory.Exists(fullPaths))
        //                Directory.CreateDirectory(fullPaths);

        //            if (Directory.Exists(fullPaths))
        //            {
        //                using (var stream = new FileStream(Savepath, FileMode.Create))
        //                {
        //                    await Attachment.CopyToAsync(stream);
        //                    stream.Flush();
        //                    stream.Close();
        //                }
        //            }
        //        }

        //        if (To != "" && To != null)
        //        {
        //            //Savepath = Path.Combine(StorageRoot, Direc_FileName);
        //            //Savepath = _configuration["AppSettings:AppURL"].ToString() + "MedicalMutual_W9CombinedForms.pdf";
        //            SMTP = await _commonService.GetSMTPServerDetails(userId);
        //            if (SMTP.Count > 0)
        //            {
        //                Body = Body.Replace("\n", "<br/>");
        //                Body = Body + "<br/><br/>";

        //                CommonMail.SendMail(SMTP[0].SMTPServer, SMTP[0].SMTPPort, SMTP[0].SMTPSSL
        //                    , SMTP[0].SMTPUserName, SMTP[0].SMTPPassword
        //                    , To, Subject, Body, CC, Savepath);
        //            }
        //        }
        //        if (CheckListFor == "Billing CheckList")
        //        {
        //            await _adminService.UpdatePECheckListStatus(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
        //        }
        //        else if (CheckListFor == "Payment Category Change")
        //        {
        //            await _adminService.UpdatePEPaymentCategoryChangeCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
        //        }
        //        else if (CheckListFor == "Single Payer Update")
        //        {
        //            await _adminService.UpdatePESinglePayerUpdateCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath, TaskValueAllocatedTo, PESinglePUID);
        //        }
        //        else if (CheckListFor == "Practice Location Change")
        //        {
        //            await _adminService.UpdatePEPracticeLocationChangeCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
        //        }
        //        else if (CheckListFor == "Closed Client")
        //        {
        //            await _adminService.UpdatePEClosedClientCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
        //        }
        //        else if (CheckListFor == "Client Bank Change")
        //        {
        //            await _adminService.UpdatePEClientBankChangeCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
        //        }
        //        else if (CheckListFor == "Commerical EFTs")
        //        {
        //            await _adminService.UpdatePECommericalEFTsCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
        //        }
        //        else if (CheckListFor == "Medicare Change of Info")
        //        {
        //            await _adminService.UpdatePEMedicareChangeOfInfoCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
        //        }
        //        else if (CheckListFor == "Out of State MCD")
        //        {
        //            await _adminService.UpdatePEOutOfStateMCDCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
        //        }
        //    }
        //    catch (Exception ex) { }
        //    return Json(new { msge = "Success" });
        //}


        [HttpPost]
        public async Task<IActionResult> DeletePESinglePayerUpdateCheckList(int Id, int ClientID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeletePESinglePayerUpdateCheckList(Id, ClientID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }


        [HttpPost]
        public async Task<IActionResult> GenerateNewChildAccountRequest(int ClientDetailsID, string ClientNumber, string ClientName, string NPINumber)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            await _menuUtils.SetMenu(HttpContext.Session);
            var userId = HttpContext.Session.GetString("UserID");

            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string pdfTemplate = Path.Combine(sWebRootFolder, "NewChildAccountRequest_Template.pdf");
            string FilePath = "";
            string newFilePath = "";
            string newFileName = "";
            string UrlBase = "";

            FilePath = sWebRootFolder;

            string fullPaths = Path.Combine(StorageRoot);


            try
            {
                newFileName = ClientNumber + "_NewChildAccountRequest.pdf";

                newFilePath = fullPaths + newFileName;

                //newFilePath = FilePath +"\\"+ newFileName;


                PdfReader pdfReader = new PdfReader(pdfTemplate);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFilePath, FileMode.Create));
                AcroFields pdfFormFields = pdfStamper.AcroFields;

                pdfFormFields.SetField("Account Name", ClientName);
                pdfFormFields.SetField("Breakout Value NPI", NPINumber);


                pdfStamper.FormFlattening = false;

                pdfStamper.Close();

                await _adminService.UpdatePEDocumentsFileDetails(0, ClientDetailsID, "New Child Account Request", newFileName, newFilePath, newFileName, userId);

            }
            catch (Exception ex)
            {

            }


            return Json(new { msge = "Success" });
        }

        [HttpPost]
        public async Task<IActionResult> GetSearchPEClientNames(string Prefix, int clientId, string Mailto)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                if (Prefix == null)
                    Prefix = "";
                if (Mailto == null)
                    Mailto = "";
                model.SearchClientNames = await _adminService.GetSearchPEClientNames(Prefix, clientId);
            }
            catch (Exception Ex) { }

            return Json(model.SearchClientNames);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadMedicalMutualwithW_9Form([FromBody] int ClientId)
        {
            List<PEDetailsList> peDetailsList = new List<PEDetailsList>();
            peDetailsList = await _adminService.GetPEDetailsList(ClientId);

            string strURL = string.Empty;
            strURL = await MMW9PIForm(peDetailsList, ClientId);

            return Json(strURL);
        }

        public async Task<string> MMW9PIForm(List<PEDetailsList> peDetailsList, int ClientId) //Changed by Mohame Iqbal 
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string mmpiTemplate = Path.Combine(sWebRootFolder, "MedicalMutualProviderInformationForm.pdf");
            string w9Template = Path.Combine(sWebRootFolder, "W9_Unlocked.pdf");

            string mmpiPath = Path.Combine(sWebRootFolder, "MMPI");
            string w9Path = Path.Combine(sWebRootFolder, "W9");
            string mergedPath = Path.Combine(sWebRootFolder, "Merged");

            // Clean old folders
            if (Directory.Exists(mmpiPath)) Directory.Delete(mmpiPath, true);
            if (Directory.Exists(w9Path)) Directory.Delete(w9Path, true);
            if (Directory.Exists(mergedPath)) Directory.Delete(mergedPath, true);

            Directory.CreateDirectory(mmpiPath);
            Directory.CreateDirectory(w9Path);
            Directory.CreateDirectory(mergedPath);

            foreach (var pe in peDetailsList)
            {
                string mmpiFile = Path.Combine(mmpiPath, $"{pe.ClientID}_{pe.ClientName}_MMPI.pdf");
                string w9File = Path.Combine(w9Path, $"{pe.ClientID}_{pe.ClientName}_W9.pdf");
                string mergedFile = Path.Combine(mergedPath, $"{pe.ClientID}_{pe.ClientName}_Combined.pdf");

                // Get other address list once per client
                List<PEOtherAddressList> peOtherAddressList = await _adminService.GetPEOtherAddresses(ClientId);
                var address3 = peOtherAddressList.FirstOrDefault(x => x.AddressTypeID == 3);

                // Generate MMPI PDF
                using (var pdfReader = new PdfReader(mmpiTemplate))
                using (var pdfStamper = new PdfStamper(pdfReader, new FileStream(mmpiFile, FileMode.Create)))
                {
                    AcroFields pdfFormFields = pdfStamper.AcroFields;

                    pdfFormFields.SetField("SLI_InfoEffectiveDate", DateTime.Now.ToString("MM/dd/yyyy"));
                    pdfFormFields.SetField("SLI_TIN", pe.TaxID);
                    pdfFormFields.SetField("SLI_Facility or Group Name", pe.ClientName);
                    pdfFormFields.SetField("SLI_Street Address", pe.PracticeLocation);
                    pdfFormFields.SetField("SLI_City", pe.City);
                    pdfFormFields.SetField("SLI_State", pe.State);
                    pdfFormFields.SetField("SLI_Zip", pe.Zip);
                    pdfFormFields.SetField("SLI_County", pe.County);
                    pdfFormFields.SetField("SLI_Office Phone", pe.Phone1);
                    pdfFormFields.SetField("SLI_Fax", pe.Fax);
                    pdfFormFields.SetField("SLI_specialty", "Ambulance - Land Transport");
                    pdfFormFields.SetField("SLI_NPI No", pe.GroupNPI);
                    pdfFormFields.SetField("RAI_Reimbursement Name", pe.ClientName);
                    pdfFormFields.SetField("RAI_Reimbursement Entity's Tin", pe.TaxID);
                    pdfFormFields.SetField("RAI_Date", DateTime.Now.ToString("MM/dd/yyyy"));
                    pdfFormFields.SetField("RAI_Phone", "800-962-1484");
                    pdfFormFields.SetField("RAI_Fax", "513-527-0659");
                    pdfFormFields.SetField("RAI_Office Manager", "AMY KING");
                    pdfFormFields.SetField("RAI_Office Manager Phone", "513-612-3158");
                    pdfFormFields.SetField("RAI_OM_Email", "PE@medicount.com");
                    pdfFormFields.SetField("RAI_OM_Date", DateTime.Now.ToString("MM/dd/yyyy"));

                    if (address3 != null)
                    {
                        pdfFormFields.SetField("SLI_Correspondence Address", $"{address3.AddressLine1}, {address3.City}, {address3.State} {address3.ZipCode}");
                        pdfFormFields.SetField("RAI_Address", address3.AddressLine1);
                        pdfFormFields.SetField("RAI_City", address3.City);
                        pdfFormFields.SetField("RAI_State", address3.State);
                        pdfFormFields.SetField("RAI_Zip", address3.ZipCode);
                    }

                    pdfStamper.FormFlattening = true;  // ✅ flatten
                }

                // Generate W9 PDF
                using (var pdfReader = new PdfReader(w9Template))
                using (var pdfStamper = new PdfStamper(pdfReader, new FileStream(w9File, FileMode.Create)))
                {
                    AcroFields pdfFormFields = pdfStamper.AcroFields;

                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_1[0]", pe.ClientName);
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].FederalClassification[0].f1_4[0]", pe.TaxIDClassification.ToString());
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].FederalClassification[0].c1_1[6]", "7");

                    if (address3 != null)
                    {
                        pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_7[0]", address3.AddressLine1.ToUpper());
                        pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_8[0]",
                            $"{address3.City.ToUpper()},{address3.State.ToUpper()} {address3.ZipCode.ToUpper()}");
                    }

                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_10[0]", "NPI - " + pe.GroupNPI);
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].EmployerID[0].f1_14[0]", pe.TaxID == "" ? "" : pe.TaxID.Substring(0, 2));
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].EmployerID[0].f1_15[0]", pe.TaxID == "" ? "" : pe.TaxID.Substring(3));
                    pdfFormFields.SetField("DateSigned", DateTime.Now.ToString("MM/dd/yyyy"));

                    pdfStamper.FormFlattening = true;  // ✅ flatten
                }

                // Merge MMPI and W9 into one PDF
                using (var stream = new FileStream(mergedFile, FileMode.Create))
                using (var document = new iTextSharp.text.Document())
                using (var copy = new PdfCopy(document, stream))
                {
                    document.Open();
                    foreach (string file in new[] { mmpiFile, w9File })
                    {
                        using (var reader = new PdfReader(file))
                        {
                            for (int i = 1; i <= reader.NumberOfPages; i++)
                                copy.AddPage(copy.GetImportedPage(reader, i));
                        }
                    }
                }
            }

            // Merge all combined files into one big PDF
            string outputMergedPdf = Path.Combine(sWebRootFolder, "MedicalMutual_W9CombinedForms.pdf");
            if (System.IO.File.Exists(outputMergedPdf)) System.IO.File.Delete(outputMergedPdf);

            string[] pdfFiles = Directory.GetFiles(mergedPath, "*.pdf");
            using (FileStream stream = new FileStream(outputMergedPdf, FileMode.Create))
            {
                Document doc = new Document();
                PdfCopy pdf = new PdfCopy(doc, stream);
                doc.Open();

                List<PdfReader> readers = new List<PdfReader>();

                try
                {
                    foreach (string file in pdfFiles)
                    {
                        PdfReader reader = new PdfReader(file);
                        readers.Add(reader);

                        for (int page = 1; page <= reader.NumberOfPages; page++)
                        {
                            pdf.AddPage(pdf.GetImportedPage(reader, page));
                        }
                    }
                }
                finally
                {
                    foreach (var reader in readers)
                    {
                        doc.Close();
                    }
                }
            }


            string url = _configuration["AppSettings:AppURL"] + "MedicalMutual_W9CombinedForms.pdf";
            return url;
        }

        //public async Task<string> MMW9PIForm(List<PEDetailsList> peDetailsList, int ClientId)
        //{
        //    string sWebRootFolder = _hostingEnvironment.WebRootPath;
        //    string mmpiTemplate = Path.Combine(sWebRootFolder, "MedicalMutualProviderInformationForm.pdf");
        //    string w9Template = Path.Combine(sWebRootFolder, "W9_Unlocked.pdf");

        //    string mmpiPath = Path.Combine(sWebRootFolder, "MMPI");
        //    string w9Path = Path.Combine(sWebRootFolder, "W9");
        //    string mergedPath = Path.Combine(sWebRootFolder, "Merged");

        //    Directory.CreateDirectory(mmpiPath);
        //    Directory.CreateDirectory(w9Path);
        //    Directory.CreateDirectory(mergedPath);

        //    foreach (var pe in peDetailsList)
        //    {
        //        string mmpiFile = Path.Combine(mmpiPath, $"{pe.ClientID}_{pe.ClientName}_MMPI.pdf");
        //        string w9File = Path.Combine(w9Path, $"{pe.ClientID}_{pe.ClientName}_W9.pdf");
        //        string mergedFile = Path.Combine(mergedPath, $"{pe.ClientID}_{pe.ClientName}_Combined.pdf");

        //        // Generate MMPI PDF
        //        using (var pdfReader = new PdfReader(mmpiTemplate))
        //        using (var pdfStamper = new PdfStamper(pdfReader, new FileStream(mmpiFile, FileMode.Create)))
        //        {
        //            AcroFields pdfFormFields = pdfStamper.AcroFields;
        //            //pdfFormFields.SetField("SLI_TIN", pe.TaxID);
        //            //pdfFormFields.SetField("SLI_Facility or Group Name", pe.ClientName);
        //            pdfFormFields.SetField("SLI_InfoEffectiveDate", DateTime.Now.ToString("MM/dd/yyyy"));
        //            pdfFormFields.SetField("SLI_TIN", pe.TaxID);
        //            pdfFormFields.SetField("SLI_Facility or Group Name", pe.ClientName.ToString());
        //            pdfFormFields.SetField("SLI_Street Address", pe.PracticeLocation);
        //            pdfFormFields.SetField("SLI_City", pe.City);
        //            pdfFormFields.SetField("SLI_State", pe.State);
        //            pdfFormFields.SetField("SLI_Zip", pe.Zip);
        //            pdfFormFields.SetField("SLI_County", pe.County);
        //            pdfFormFields.SetField("SLI_Office Phone", pe.Phone1);
        //            pdfFormFields.SetField("SLI_Fax", pe.Fax);
        //            pdfFormFields.SetField("SLI_specialty", "Ambulance - Land Transport");
        //            pdfFormFields.SetField("SLI_NPI No", pe.GroupNPI);
        //            pdfFormFields.SetField("RAI_Reimbursement Name", pe.ClientName);
        //            pdfFormFields.SetField("RAI_Reimbursement Entity's Tin", pe.TaxID);
        //            pdfFormFields.SetField("RAI_Date", DateTime.Now.ToString("MM/dd/yyyy"));

        //            pdfFormFields.SetField("RAI_Phone", "800-962-1484");
        //            pdfFormFields.SetField("RAI_Fax", "513-527-0659");

        //            pdfFormFields.SetField("RAI_Office Manager", "AMY KING");
        //            pdfFormFields.SetField("RAI_Office Manager Phone", " 513-612-3158");
        //            pdfFormFields.SetField("RAI_OM_Email", "PE@medicount.com");
        //            pdfFormFields.SetField("RAI_OM_Date", DateTime.Now.ToString("MM/dd/yyyy"));

        //            List<PEOtherAddressList> peOtherAddressList = new List<PEOtherAddressList>();
        //            peOtherAddressList = await _adminService.GetPEOtherAddresses(ClientId);
        //            for (int j = 0; j < peOtherAddressList.Count; j++)
        //            {
        //                if (peOtherAddressList[j].AddressTypeID == 3)
        //                {
        //                    pdfFormFields.SetField("SLI_Correspondence Address", peOtherAddressList[j].AddressLine1 + ", " + peOtherAddressList[j].City + ", " + peOtherAddressList[j].State + " " + peOtherAddressList[j].ZipCode);
        //                    pdfFormFields.SetField("RAI_Address", peOtherAddressList[j].AddressLine1);
        //                    pdfFormFields.SetField("RAI_City", peOtherAddressList[j].City);
        //                    pdfFormFields.SetField("RAI_State", peOtherAddressList[j].State);
        //                    pdfFormFields.SetField("RAI_Zip", peOtherAddressList[j].ZipCode);
        //                }
        //            }
        //            pdfStamper.FormFlattening = true;
        //            // pdfStamper.Close();
        //        }

        //        // Generate W9 PDF
        //        using (var pdfReader = new PdfReader(w9Template))
        //        using (var pdfStamper = new PdfStamper(pdfReader, new FileStream(w9File, FileMode.Create)))
        //        {
        //            AcroFields pdfFormFields = pdfStamper.AcroFields;
        //            //pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_1[0]", pe.ClientName);
        //            pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_1[0]", pe.ClientName);
        //            pdfFormFields.SetField("topmostSubform[0].Page1[0].FederalClassification[0].f1_4[0]", pe.TaxIDClassification.ToString());
        //            pdfFormFields.SetField("topmostSubform[0].Page1[0].FederalClassification[0].c1_1[6]", "7");

        //            List<PEOtherAddressList> peOtherAddressList = new List<PEOtherAddressList>();
        //            peOtherAddressList = await _adminService.GetPEOtherAddresses(ClientId);
        //            for (int j = 0; j < peOtherAddressList.Count; j++)
        //            {
        //                if (peOtherAddressList[j].AddressTypeID == 3)
        //                {
        //                    pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_7[0]", peOtherAddressList[j].AddressLine1.ToString().ToUpper());
        //                    pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_8[0]", peOtherAddressList[j].City.ToString().ToUpper() + "," + peOtherAddressList[j].State.ToString().ToUpper() + " " + peOtherAddressList[j].ZipCode.ToString().ToUpper());
        //                }
        //            }

        //            pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_10[0]", "NPI - " + pe.GroupNPI);
        //            pdfFormFields.SetField("topmostSubform[0].Page1[0].EmployerID[0].f1_14[0]", pe.TaxID == "" ? "" : pe.TaxID.Substring(0, 2));
        //            pdfFormFields.SetField("topmostSubform[0].Page1[0].EmployerID[0].f1_15[0]", pe.TaxID == "" ? "" : pe.TaxID.Substring(3, pe.TaxID.Length - 3));
        //            pdfFormFields.SetField("DateSigned", DateTime.Now.ToString("MM/dd/yyyy"));

        //            pdfStamper.FormFlattening = true;
        //            //pdfStamper.Close();
        //        }

        //        // Merge MMPI and W9 into one PDF
        //        using (var stream = new FileStream(mergedFile, FileMode.Create))
        //        using (var document = new iTextSharp.text.Document())
        //        using (var copy = new PdfCopy(document, stream))
        //        {
        //            document.Open();
        //            foreach (string file in new[] { mmpiFile, w9File })
        //            {
        //                var reader = new PdfReader(file);
        //                for (int i = 1; i <= reader.NumberOfPages; i++)
        //                    copy.AddPage(copy.GetImportedPage(reader, i));
        //                reader.Close();
        //            }
        //            document.Close();
        //        }
        //    }

        //    string outputMergedPdf = Path.Combine(sWebRootFolder, "MedicalMutual_W9CombinedForms.pdf");
        //    if (System.IO.File.Exists(outputMergedPdf)) System.IO.File.Delete(outputMergedPdf);

        //    string[] pdfFiles = Directory.GetFiles(mergedPath, "*.pdf");

        //    using (FileStream stream = new FileStream(outputMergedPdf, FileMode.Create))
        //    {
        //        Document doc = new Document();
        //        PdfCopy pdf = new PdfCopy(doc, stream);
        //        doc.Open();

        //        foreach (string file in pdfFiles)
        //        {
        //            PdfReader reader = new PdfReader(file);
        //            for (int page = 1; page <= reader.NumberOfPages; page++)
        //            {
        //                pdf.AddPage(pdf.GetImportedPage(reader, page));
        //            }
        //            pdf.FreeReader(reader);
        //            reader.Close();
        //        }

        //        doc.Close();
        //    }

        //    string url = _configuration["AppSettings:AppURL"].ToString() + "MedicalMutual_W9CombinedForms.pdf";
        //    return url;


        //}

        [HttpPost]
        public async Task<IActionResult> DownloadMedcowithW_9Form([FromBody] int ClientId)
        {
            List<PEDetailsList> peDetailsList = new List<PEDetailsList>();
            peDetailsList = await _adminService.GetPEDetailsList(ClientId);

            string strURL = string.Empty;
            strURL = await MedcoW9Form(peDetailsList, ClientId);

            return Json(strURL);
        }


        public async Task<string> MedcoW9Form(List<PEDetailsList> peDetailsList, int ClientId)  // Changed by Mohamed Iqbal
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string mmpiTemplate = Path.Combine(sWebRootFolder, "Medco.pdf");
            string w9Template = Path.Combine(sWebRootFolder, "W9_Unlocked.pdf");

            string mmpiPath = Path.Combine(sWebRootFolder, "Medco");
            string w9Path = Path.Combine(sWebRootFolder, "W9");
            string mergedPath = Path.Combine(sWebRootFolder, "MedcoWithW9");

            // ✅ Clean up old files to avoid duplication or leftovers
            if (Directory.Exists(mergedPath)) Directory.Delete(mergedPath, true);
            if (Directory.Exists(mmpiPath)) Directory.Delete(mmpiPath, true);
            if (Directory.Exists(w9Path)) Directory.Delete(w9Path, true);

            Directory.CreateDirectory(mmpiPath);
            Directory.CreateDirectory(w9Path);
            Directory.CreateDirectory(mergedPath);

            foreach (var pe in peDetailsList)
            {
                string mmpiFile = Path.Combine(mmpiPath, $"{pe.ClientID}_{pe.ClientName}_Medco.pdf");
                string w9File = Path.Combine(w9Path, $"{pe.ClientID}_{pe.ClientName}_W9.pdf");
                string mergedFile = Path.Combine(mergedPath, $"{pe.ClientID}_{pe.ClientName}_Combined.pdf");

                // ✅ Generate MMPI PDF with FormFlattening = true
                using (var pdfReader = new PdfReader(mmpiTemplate))
                using (var pdfStamper = new PdfStamper(pdfReader, new FileStream(mmpiFile, FileMode.Create)))
                {
                    AcroFields pdfFormFields = pdfStamper.AcroFields;
                    pdfFormFields.GenerateAppearances = true;
                    pdfFormFields.SetField("02 AmbulanceCB", "Yes");
                    //AcroFields formFields = pdfStamper.AcroFields;
                    var fieldKeys = pdfFormFields.Fields.Keys;

                    foreach (var fieldKey in fieldKeys)
                    {
                        System.Diagnostics.Debug.WriteLine("Field: " + fieldKey);
                    }

                    List<ClientInsuranceContractFileDetails> ContractList = await _commonService.GetClientInsuranceContractFileDetailsList(ClientId);

                    for (int j = 0; j < ContractList.Count; j++)
                    {
                        if (ContractList[j].InsuranceContractName == "BWC")//BWC provider number
                        {
                            pdfFormFields.SetField("GI_ProviderNumber", ContractList[j].ProviderNo.ToUpper());
                        }
                    }

                    pdfFormFields.SetField("GI_TIN", pe.TaxID.ToUpper());
                    pdfFormFields.SetField("GI_SSN#", "NA");
                    pdfFormFields.SetField("GI_Individual Name", pe.ClientName.ToUpper());
                    pdfFormFields.SetField("GI_CB1", "Yes");
                    pdfFormFields.SetField("GI_DBA Name", pe.ClientName.ToUpper());
                    pdfFormFields.SetField("GI_Business Type", pe.TaxIDClassification.ToUpper());
                    pdfFormFields.SetField("GI_NPI", pe.GroupNPI.ToUpper());
                    pdfFormFields.SetField("GI_Taxonomy Code", pe.Taxonomy.ToUpper());
                    pdfFormFields.SetField("GI_Business Owner Names", pe.ClientName + " - 100% OWNERSHIP");
                    pdfFormFields.SetField("GI_WCPolicy", pe.WPPolicyNumber.ToUpper());
                    pdfFormFields.SetField("GI_Practice Location", pe.PracticeLocation.ToUpper());
                    pdfFormFields.SetField("GI_City", pe.City.ToUpper());
                    pdfFormFields.SetField("GI_state", pe.State.ToUpper());
                    pdfFormFields.SetField("GI_Zip Code", pe.Zip.ToUpper());
                    pdfFormFields.SetField("GI_Telephone", "800-962-1484");
                    pdfFormFields.SetField("GI_Fax", "513-527-0659");
                    pdfFormFields.SetField("GI_Email Address", "PE@MEDICOUNT.COM"); //peDetailsList[i].Email);

                    List<PEOtherAddressList> peOtherAddressList = new List<PEOtherAddressList>();
                    peOtherAddressList = await _adminService.GetPEOtherAddresses(ClientId);
                    for (int j = 0; j < peOtherAddressList.Count; j++)
                    {
                        if (peOtherAddressList[j].AddressTypeID == 3)
                        {
                            pdfFormFields.SetField("GI_Reimb Address", peOtherAddressList[j].AddressLine1.ToUpper());
                            pdfFormFields.SetField("GI_Reimb City", peOtherAddressList[j].City.ToUpper());
                            pdfFormFields.SetField("GI_Reimb state", peOtherAddressList[j].State.ToUpper());
                            pdfFormFields.SetField("GI_Reimb Zip", peOtherAddressList[j].ZipCode);
                        }
                    }


                    pdfFormFields.SetField("GI_Corres Address", "PO BOX 392907");
                    pdfFormFields.SetField("GI_Corres City", "PITTSBURGH");
                    pdfFormFields.SetField("GI_Corres State", "PA");
                    pdfFormFields.SetField("GI_Corres Zip Code", "15251-9907");

                    pdfFormFields.SetField("Req_List Medicare", pe.MedicareNumber.ToUpper());
                    pdfFormFields.SetField("GI_MedicareState", pe.State.ToUpper());
                    pdfFormFields.SetField("Req_Medicaid Number", pe.MedicaidNumber.ToUpper());
                    pdfFormFields.SetField("GI_MedicaidState", pe.State.ToUpper());


                    //pdfFormFields.SetField("PQI_26", "26");
                    pdfFormFields.SetField("PIQ_1", "No");

                    //pdfFormFields.SetField("PQI_25", "25");
                    pdfFormFields.SetField("PIQ_2", "No");

                    //pdfFormFields.SetField("PQI_24", "24");
                    pdfFormFields.SetField("PQI_3", "No");

                    //pdfFormFields.SetField("PQI_23", "23");
                    pdfFormFields.SetField("PQI_4", "No");

                    //pdfFormFields.SetField("PQI_22", "22");
                    pdfFormFields.SetField("PQI_5", "No");

                    //pdfFormFields.SetField("PQI_21", "21");
                    pdfFormFields.SetField("PQI_6", "No");

                    //pdfFormFields.SetField("PQI_20", "20");
                    pdfFormFields.SetField("PQI_7", "No");

                    //pdfFormFields.SetField("PQI_19", "19");
                    pdfFormFields.SetField("PQI_8", "No");

                    //pdfFormFields.SetField("PQI_18", "18");
                    pdfFormFields.SetField("PQI_9", "No");

                    //pdfFormFields.SetField("PQI_17", "17");
                    pdfFormFields.SetField("PQI_10", "No");

                    //pdfFormFields.SetField("PQI_16", "16");
                    pdfFormFields.SetField("PQI_11", "No");

                    //pdfFormFields.SetField("PQI_15", "15");
                    pdfFormFields.SetField("PQI_12", "No");

                    //pdfFormFields.SetField("PQI_14", "14");
                    pdfFormFields.SetField("PQI_13", "No");

                    pdfFormFields.SetField("Exp_App Contact", "AMY KING");
                    pdfFormFields.SetField("Exp_Title", "PROVIDER ENROLLMENT MANAGER");
                    pdfFormFields.SetField("EXP_Telep", " 513-612-3158");
                    pdfFormFields.SetField("Exp_Fax", "513-527-0659");
                    pdfFormFields.SetField("Exp_Email", "PE@MEDICOUNT.COM");

                    pdfFormFields.SetField("OwnerName", pe.ClientName);
                    pdfFormFields.SetField("Ownership%", "100%");

                    pdfFormFields.SetField("PrintName", "AMY KING");
                    pdfFormFields.SetField("End Date", DateTime.Now.ToString("MM/dd/yyyy"));

                    // ✅ Crucial fix: flatten the form so data is kept when merging
                    pdfStamper.FormFlattening = true;
                    pdfStamper.Close();
                }

                // ✅ Generate W9 PDF with FormFlattening = true
                using (var pdfReader = new PdfReader(w9Template))
                using (var pdfStamper = new PdfStamper(pdfReader, new FileStream(w9File, FileMode.Create)))
                {
                    AcroFields pdfFormFields = pdfStamper.AcroFields;
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_1[0]", pe.ClientName);
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].FederalClassification[0].f1_4[0]", pe.TaxIDClassification.ToString());

                    List<PEOtherAddressList> peOtherAddressList = await _adminService.GetPEOtherAddresses(ClientId);
                    foreach (var addr in peOtherAddressList)
                    {
                        if (addr.AddressTypeID == 3)
                        {
                            pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_7[0]", addr.AddressLine1.ToUpper());
                            pdfFormFields.SetField("topmostSubform[0].Page1[0].Address[0].f1_8[0]", addr.City.ToUpper() + "," + addr.State.ToUpper() + " " + addr.ZipCode.ToUpper());
                        }
                    }

                    pdfFormFields.SetField("topmostSubform[0].Page1[0].f1_10[0]", "NPI - " + pe.GroupNPI);
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].EmployerID[0].f1_14[0]", pe.TaxID == "" ? "" : pe.TaxID.Substring(0, 2));
                    pdfFormFields.SetField("topmostSubform[0].Page1[0].EmployerID[0].f1_15[0]", pe.TaxID == "" ? "" : pe.TaxID.Substring(3));
                    pdfFormFields.SetField("DateSigned", DateTime.Now.ToString("MM/dd/yyyy"));

                    // ✅ Crucial fix: flatten the form
                    pdfStamper.FormFlattening = true;
                }

                // Merge MMPI + W9 into one PDF
                using (var stream = new FileStream(mergedFile, FileMode.Create))
                using (var document = new Document())
                using (var copy = new PdfCopy(document, stream))
                {
                    document.Open();
                    foreach (string file in new[] { mmpiFile, w9File })
                    {
                        using (var reader = new PdfReader(file))
                        {
                            for (int i = 1; i <= reader.NumberOfPages; i++)
                            {
                                copy.AddPage(copy.GetImportedPage(reader, i));
                            }
                        }
                    }
                }
            }

            // ✅ Now merge all combined PDFs into one final file
            string outputMergedPdf = Path.Combine(sWebRootFolder, "MedcoW9CombinedForms.pdf");
            if (System.IO.File.Exists(outputMergedPdf))
                System.IO.File.Delete(outputMergedPdf);

            string[] pdfFiles = Directory.GetFiles(mergedPath, "*.pdf");

            using (FileStream stream = new FileStream(outputMergedPdf, FileMode.Create))
            {
                using (Document doc = new Document())
                using (PdfCopy pdf = new PdfCopy(doc, stream))
                {
                    doc.Open();
                    foreach (string file in pdfFiles)
                    {
                        using (PdfReader reader = new PdfReader(file))
                        {
                            for (int page = 1; page <= reader.NumberOfPages; page++)
                            {
                                pdf.AddPage(pdf.GetImportedPage(reader, page));
                            }
                        }
                    }
                }
            }

            string url = _configuration["AppSettings:AppURL"].ToString() + "MedcoW9CombinedForms.pdf";
            return url;
        }


        [HttpGet]
        public IActionResult GetMMW9PEFile(string fileName)
        {
            var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath);//, "uploads/PEChecklist");
            var filePath = Path.Combine(uploadPath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                // Construct the public URL
                string baseUrl = _configuration["AppSettings:AppURL"];
                string fileUrl = $"{baseUrl}{fileName}";

                return Json(new { success = true, fileUrl = fileUrl });
            }

            return Json(new { success = false, message = "File not found" });
        }

         [HttpGet]
       public IActionResult GetM_W9PEFile(string fileName)  //Created by Mohamed Iqbal
       {
           var uploadPath = Path.Combine(_hostingEnvironment.WebRootPath);//, "uploads/PEChecklist");
           var filePath = Path.Combine(uploadPath, fileName);

           if (System.IO.File.Exists(filePath))
           {
               // Construct the public URL
               string baseUrl = _configuration["AppSettings:AppURL"];
               string fileUrl = $"{baseUrl}{fileName}";

               return Json(new { success = true, fileUrl = fileUrl });
           }

           return Json(new { success = false, message = "File not found" });
       }


        #region Instructions Document


        [HttpGet]
        public async Task<IActionResult> InstructionsDocument()
        {
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.InstructionsDocument = new InstructionsDocument();
                model.InstructionsDocumentList = await _adminService.GetInstructionsDocumentList();
            }
            catch (Exception ex)
            {

            }
            return View("InstructionsDocument", model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertInstructionsDocument(int Id, string DocumentName
           , IFormFile FileAttachment)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                var userId = HttpContext.Session.GetString("UserID");

                ProviderEntrollment model = new ProviderEntrollment();
                model.InstructionsDocument = new InstructionsDocument();

                model.InstructionsDocument.Id = Id;
                model.InstructionsDocument.DocumentName = DocumentName;

                string Savepath = "";

                if (FileAttachment != null)
                {
                    string FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + FileAttachment.FileName;
                    string Direc_FileName = FileName;

                    Savepath = Path.Combine(StorageRoot, Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot);

                    if (!Directory.Exists(fullPaths))
                        Directory.CreateDirectory(fullPaths);

                    if (Directory.Exists(fullPaths))
                    {
                        using (var stream = new FileStream(Savepath, FileMode.Create))
                        {
                            await FileAttachment.CopyToAsync(stream);
                            stream.Flush();
                            stream.Close();
                        }
                    }

                    model.InstructionsDocument.OrgFileName = FileAttachment.FileName;
                    model.InstructionsDocument.FileName = FileName;

                    await _adminService.InsertInstructionsDocument(model, userId);
                }
            }
            catch (Exception ex) { }
            return Json(new { msge = "Success" });
        }


        [HttpPost]
        public async Task<IActionResult> GetInstructionsDocumentList(int id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.InstructionsDocumentList = await _adminService.GetInstructionsDocumentList();
                totalrows = (model.InstructionsDocumentList.Count > 0 ? model.InstructionsDocumentList.Count : 0);
                totalRowsAfterFiltering = (model.InstructionsDocumentList.Count > 0 ? model.InstructionsDocumentList.Count : 0);
            }
            catch (Exception Ex) { }
            return Json(new { data = model.InstructionsDocumentList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteInstructionsDocument(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeleteInstructionsDocument(ID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        public FileResult ViewInstructionsDocumentFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(StorageRoot) + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        #endregion

        #region Notes

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePENotes(int Id, int ClientId, string Notes)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();

            string message = "";
            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);

                await _adminService.InsertPENotes(Id, ClientId, Notes, userId);

                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }
        [HttpPost]
        public async Task<IActionResult> PENotesList(int ClientId)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.PENotesList = await _adminService.GetPENotesList(ClientId);
            }
            catch (Exception Ex) { }
            //return Json(new { data = model.PECredentialingLicenseList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
            return PartialView("_PENotes", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePENotes(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeletePENotes(ID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }

        #endregion

        #region PE Checklist for AllocatedTo 
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PEChkListSendMailUpdateStatusAllocatedTo(int ClientID, string MailFor = " ", string CheckListFor = " ", string ChecklistStatus=" ", string Status=" ", string PESinglePUID = " ", string TaskValueAllocatedTo = " ")
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            try
            {

                await _menuUtils.SetMenu(HttpContext.Session);
                var userId = HttpContext.Session.GetString("UserID");


                if (CheckListFor == "Billing CheckList")
                {
                    await _adminService.UpdatePECheckListStatusAllocatedTo(ClientID, MailFor, Status, userId);
                }
                else if (CheckListFor == "Payment Category Change")
                {
                    await _adminService.UpdatePEPaymentCategoryChangeCheckListAllocatedTo(ClientID, MailFor, Status, userId);
                }
                else if (CheckListFor == "Single Payer Update")
                {
                    await _adminService.UpdatePESinglePayerUpdateCheckListAllocatedTo(ClientID, MailFor, Status, ChecklistStatus,userId, PESinglePUID, TaskValueAllocatedTo);
                }
                else if (CheckListFor == "Practice Location Change")
                {
                      await _adminService.UpdatePEPracticeLocationChangeCheckListAllocatedTo(ClientID, MailFor, Status, userId);
                }
                else if (CheckListFor == "Closed Client")
                {
                    await _adminService.UpdatePEClosedClientCheckListAllocatedTo(ClientID, MailFor, Status, userId);
                }
                else if (CheckListFor == "Client Bank Change")
                {
                    await _adminService.UpdatePEClientBankChangeCheckListAllocated(ClientID, MailFor, Status, userId);
                }
                //else if (CheckListFor == "Commerical EFTs")
                //{
                //    await _adminService.UpdatePECommericalEFTsCheckList(ClientID, MailFor, Status, userId, To, CC, Subject, Body, Savepath);
                //}
                else if (CheckListFor == "Medicare Change of Info")
                {
                    await _adminService.UpdatePEMedicareChangeOfInfoCheckListAllocatedTo(ClientID, MailFor, Status, userId);
                }
                else if (CheckListFor == "Out of State MCD")
                {
                    await _adminService.UpdatePEOutOfStateMCDCheckListAllocatedTo(ClientID, MailFor, Status, userId);
                }
            }
            catch (Exception ex) { }
            return Json(new { msge = "Success" });

            
        }
        #endregion

        #region EFT BulkUpload
        [HttpPost]

        public async Task<IActionResult> UploadPEEFTFile(IFormFile EFTFileUpload, int ClientDetailsID)
        {
            if (EFTFileUpload != null && EFTFileUpload.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/PEEFT");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, EFTFileUpload.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await EFTFileUpload.CopyToAsync(stream);
                }

                DataTable dt = ReadExcelToDataTable(filePath);

                // 2. Add metadata columns
                string userId = HttpContext.Session.GetString("UserID");               

                foreach (DataRow row in dt.Rows)
                {
                    row["ClientDetailsID"] = row["ClientDetailsID"];
                    row["EFTPayerName"] = row["EFTPayerName"];
                    row["EFTBankType"] = row["EFTBankType"];
                    row["EFTComments"] = row["EFTComments"]; 
                    row["CreatedBy"] = userId;
                    row["CreatedOn"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    row["IsDeleted"] = "0";
                }
                // 3. Convert to XML
                string xmlData = ConvertDataTableToXml(dt);

                // 4. Call stored procedure
                var xmlParam = new SqlParameter("@XmlData", xmlData) { SqlDbType = SqlDbType.Xml };
                await _adminService.PEEFTFileUpload(xmlData);              

                return Json(new { success = true, fileName = EFTFileUpload.FileName });     
            }

            return Json(new { success = false, message = "File upload failed." });
        }
       

            private DataTable ReadExcelToDataTable(string filePath)
            {
                DataTable dt = new DataTable();

                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs);  // supports .xlsx
                    ISheet sheet = workbook.GetSheetAt(0);      // read first sheet

                    if (sheet == null) return dt;

                    // --- Read header row ---
                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    for (int j = 0; j < cellCount; j++)
                    {
                        string columnName = headerRow.GetCell(j)?.ToString();
                        if (string.IsNullOrWhiteSpace(columnName))
                            columnName = "Column" + j;

                        dt.Columns.Add(columnName);
                    }

                    // --- Read data rows ---
                    for (int i = 1; i <= sheet.LastRowNum; i++) // skip header row
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;

                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < cellCount; j++)
                        {
                            dr[j] = row.GetCell(j)?.ToString();
                        }
                        dt.Rows.Add(dr);
                    }
                }

                return dt;
            }
        private string ConvertDataTableToXml(DataTable dt)
        {
            DataSet ds = new DataSet("Root");   // Root node
            dt.TableName = "PEEFT";             // Each row becomes <PEEFT>...</PEEFT>
            ds.Tables.Add(dt.Copy());

            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw, XmlWriteMode.IgnoreSchema);
                return sw.ToString();
            }
        }

        //public async Task<IActionResult> UploadPEEFTFile(PEEFTFileUploadModel fileUpload)
        //{
        //    return 
        //}

        #endregion

        #region Client's Bank Info 
        [HttpPost]
        //
        public async Task<IActionResult> PEClientsBankInfoList([FromBody] int id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.PEClientsBankInfoList = await _adminService.GetPEClientsBankInfoList(id);
                model.PEDocumentTitleList = await _adminService.GetPEDocumentTitleList();
            }
            catch (Exception Ex) { }
            return PartialView("_PEClientsBankInfo", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePEClientsBankInfo([FromBody] PEClientsBankInfo pEClientsBankInfo)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();

            string message = "";
            var userId = HttpContext.Session.GetString("UserID");
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PEClientsBankInfo = pEClientsBankInfo;

                await _adminService.InsertPEClientBankInfo(model, userId);

                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }
        [HttpPost]
        public async Task<IActionResult> EditPEClientsBankInfo(int Id)
        {
            string message = "";
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            
            ProviderEntrollment model = new ProviderEntrollment();
            try
            {
                model.PEClientsBankInfo  = await _adminService.GetPEClientsBankInfoByID(Id);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }           
            return Json(new { data = model.PEClientsBankInfo, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });

        }
        [HttpPost]
        public async Task<IActionResult> DeletePEClientsBankInfo(int ID)
        {
            if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
            {
                return Json("");
            }
            ProviderEntrollment model = new ProviderEntrollment();
            string message = "";
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeletePEClientsBankInfo(ID, UserId);
                message = "Success";
            }
            catch (Exception ex)
            {
                message = "Failed";
            }
            return Json(new { Message = message });
        }
        #endregion

        #region Clients Bank Info Upload

        public async Task<IActionResult> UploadPEClientsBankInfoFile(IFormFile ClientsBankInfoFileUpload, int ClientDetailsID)
        {
            if (ClientsBankInfoFileUpload != null && ClientsBankInfoFileUpload.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/PEEFT");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, ClientsBankInfoFileUpload.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ClientsBankInfoFileUpload.CopyToAsync(stream);
                }

                DataTable dt = ReadExcelToDataTable(filePath);

                // 2. Add metadata columns
                string userId = HttpContext.Session.GetString("UserID");

                foreach (DataRow row in dt.Rows)
                {
                    row["ClientDetailsID"] = row["ClientDetailsID"];
                    row["BankName"] = row["BankName"];
                    row["AccountType"] = row["AccountType"];
                    row["BankCategory"] = row["BankCategory"];
                    row["EffectiveDate"] = row["EffectiveDate"];
                    row["EndDate"] = row["EndDate"];
                    row["AddressLine1"] = row["AddressLine1"];
                    row["AddressLine2"] = row["AddressLine2"];
                    row["StateCode"] = row["StateCode"];
                    row["CityName"] = row["CityName"];
                    row["ZipCode"] = row["ZipCode"];
                    row["RoutingNumber"] = row["RoutingNumber"];
                    row["AccountNumber"] = row["AccountNumber"];
                    row["CreatedBy"] = userId;
                    row["CreatedOn"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    row["IsDeleted"] = "0";
                }
                // 3. Convert to XML
                string xmlData = ConvertDataTableToXmlInClientsBankInfo(dt);

                // 4. Call stored procedure
                var xmlParam = new SqlParameter("@XmlData", xmlData) { SqlDbType = SqlDbType.Xml };
                await _adminService.PEClientsBankInfoFileUpload(xmlData);

                return Json(new { success = true, fileName = ClientsBankInfoFileUpload.FileName });
            }

            return Json(new { success = false, message = "File upload failed." });
        }

        private string ConvertDataTableToXmlInClientsBankInfo(DataTable dt)
        {
            DataSet ds = new DataSet("Root");  
            dt.TableName = "ClientsBankInfo";            
            ds.Tables.Add(dt.Copy());

            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw, XmlWriteMode.IgnoreSchema);
                return sw.ToString();
            }
        }
        [HttpPost]
        public async Task<IActionResult> PEClientsBankInfoBankLetterList([FromBody] int id)
        {
            ProviderEntrollment model = new ProviderEntrollment();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                model.PEDocumentTitleList = await _adminService.GetPEDocumentTitleList();
                model.PEBankLetterDocumentList  = await _adminService.GetPEDocumentFromClientsBankInfoList(id);
             
            }
            catch (Exception Ex) { }           
            return PartialView("_PEClientsBankInfoBankLetterUploadList", model);
        }

        #endregion

    }
}
