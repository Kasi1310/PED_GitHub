using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PED.ViewModels.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WiseX.Controllers;
using WiseX.Data;
using WiseX.Helpers;
using WiseX.Models;
using System.Linq.Dynamic.Core;
using WiseX.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PED.Helpers;
using PED.ViewModels.Admin;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;

namespace PED.Controllers
{
    [SessionTimeout]
    public class ContractController : Controller
    {
        String StorageRoot = null;
        String TemplateRoot = null;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AdminService _adminService;
        private readonly CommonService _commonService;
        private readonly IConfiguration _configuration;
        private readonly MenuUtils _menuUtils;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHostingEnvironment _hostingEnvironment;
        string sortColumnName, sortDirection, val, dummyEmail = string.Empty;
        int Start, Length, totalrows, totalRowsAfterFiltering, ContractId;
        List<SMTPServerDetails> SMTP = new List<SMTPServerDetails>();
        public ContractController(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            //Intialize
            _hostingEnvironment = hostingEnvironment;
            _applicationDbContext = applicationDbContext;
            _adminService = new AdminService(applicationDbContext);
            _commonService = new CommonService(_applicationDbContext);
            _menuUtils = new MenuUtils(applicationDbContext);
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _authorizationService = authorizationService;
            this.StorageRoot = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["AppSettings:ContractUploadPath"].ToString());
            this.TemplateRoot = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["AppSettings:MailTemplate"].ToString());
            this.dummyEmail = _configuration["AppSettings:DummyEmailTo"].ToString();
        }

        [HttpPost]
        public async Task<IActionResult> GetSearchClient(string Prefix)
        {
            Contracts model = new Contracts();
            try
            {
                if (Prefix == null)
                    Prefix = "";
                model.SearchClientList = await _commonService.GetSearchClient(Prefix);
            }
            catch (Exception Ex) { }
            return Json(model.SearchClientList);
        }

        [HttpPost]
        public async Task<IActionResult> GetSearchClientEmployee(string Prefix, int clientId, string Mailto)
        {
            Contracts model = new Contracts();
            try
            {
                if (Prefix == null)
                    Prefix = "";
                if (Mailto == null)
                    Mailto = "";
                model.SearchClientEmployeeList = await _commonService.GetSearchClientEmployee(Prefix, clientId, Mailto);
            }
            catch (Exception Ex) { }
            return Json(model.SearchClientEmployeeList);
        }

        //[ResponseCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> Contracts()
        {
            Contracts model = new Contracts();
            try
            {
                model.ClientsDetails = new ClientsDetails();
                model.ClientContractDetails = new ClientContractDetails();
                List<ContractStatus> contractstatuslist = new List<ContractStatus>();
                string UserId = HttpContext.Session.GetString("UserID");
                model.ContractUSersList = await _commonService.GetUsers(UserId);
                model.ContractAgreementTypeList = await _commonService.GetContractAgreementType();
                model.ClientsDetailsList = await _adminService.GetClients(0, 0);
                model.ContractStatus = contractstatuslist;

                model.InsuranceContractList = await _adminService.GetInsuranceContract(0);
            }
            catch (Exception ex) { }
            return View(model);
        }
        [ResponseCache(NoStore = true, Duration = 0)]
        //To Load the Error Chart List Dynamically
        [HttpPost]
        public async Task<IActionResult> LoadList(string Page)
        {
            Contracts model = new Contracts();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                sortColumnName = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"] + "][name]"];
                sortDirection = HttpContext.Request.Form["order[0][dir]"];
                val = Page;
                Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
                Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
                //Pass zero to get all the charts
                model.ContractDetailsList = await _commonService.GetContractDetailsList(Start, Length, UserId);
                totalrows = (model.ContractDetailsList.Count > 0 ? model.ContractDetailsList.First().TotalCount : 0);
                totalRowsAfterFiltering = (model.ContractDetailsList.Count > 0 ? model.ContractDetailsList.First().TotalCount : 0);
                //Sorting 
                model.ContractDetailsList = model.ContractDetailsList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();
                //Pageing
                model.ContractDetailsList = model.ContractDetailsList.Skip(Start).Take(Length).ToList();
                //foreach (var item in model.ContractDetailsList)
                //{
                //    //item.FilePath = "http://localhost:10561/Contracts/1/09282019093435_01-scansmpl.pdf.PdfCompressor-2496619.pdf";
                //    item.FileName = _configuration["AppSettings:AppURL"].ToString() + _configuration["AppSettings:ContractUploadPath"].ToString() + item.FileName;
                //}
            }
            catch (Exception Ex) { }
            return Json(new { data = model.ContractDetailsList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }


        [ResponseCache(NoStore = true, Duration = 0)]
        //To Load the Error Chart List Dynamically
        [HttpPost]
        public async Task<IActionResult> ContractLoadList(int id)
        {
            Contracts model = new Contracts();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                sortColumnName = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"] + "][name]"];
                sortDirection = HttpContext.Request.Form["order[0][dir]"];
                //string val = Page;
                Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
                Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
                //Pass zero to get all the charts
                model.ClientContractDetailsList = await _commonService.GetClientContractDetailsList(Start, Length, id);

                totalrows = (model.ClientContractDetailsList.Count > 0 ? model.ClientContractDetailsList.First().TotalCount : 0);

                totalRowsAfterFiltering = (model.ClientContractDetailsList.Count > 0 ? model.ClientContractDetailsList.First().TotalCount : 0);
                //Sorting 
                model.ClientContractDetailsList = model.ClientContractDetailsList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();
                //Pageing
                model.ClientContractDetailsList = model.ClientContractDetailsList.Skip(Start).Take(Length).ToList();
                //foreach (var item in model.ClientContractDetailsList)
                //{
                //    //item.FilePath = "http://localhost:10561/Contracts/1/09282019093435_01-scansmpl.pdf.PdfCompressor-2496619.pdf";
                //    //item.FileName = _configuration["AppSettings:AppURL"].ToString() + _configuration["AppSettings:ContractUploadPath"].ToString() + item.FileName;
                //    //model.ClientContractDetails.Sentby = item.Sentby;
                //}
            }
            catch (Exception Ex) { }
            return Json(new { data = model.ClientContractDetailsList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<ActionResult> ClientContractsLog(int contractId)
        {
            Contracts model = new Contracts();
            try
            {
                model.ContractLogList = await _commonService.GetContractLogList(contractId);
            }
            catch (Exception ex) { }
            return PartialView("_ContractsLog", model);
        }

        [HttpPost]
        public async Task<ActionResult> ClientContractsLog1(int contractId)
        {
            Contracts model = new Contracts();
            try
            {
                model.ContractLogList = await _commonService.GetContractLogList(contractId);
            }
            catch (Exception ex) { }
            return Json(new { data = model.ContractLogList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<ActionResult> ClientContractsOtherDocument(int contractId)
        {
            Contracts model = new Contracts();
            try
            {
                model.ClientContractOtherDocumentList = await _commonService.GetClientContractOtherDocument(contractId);

                //totalrows = (model.ClientContractOtherDocumentList.Count > 0 ? model.ClientContractOtherDocumentList.First().TotalCount : 0);

                //totalRowsAfterFiltering = (model.ClientContractOtherDocumentList.Count > 0 ? model.ClientContractOtherDocumentList.First().TotalCount : 0);
                ////Sorting 
                //model.ClientContractOtherDocumentList = model.ClientContractOtherDocumentList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();
                ////Pageing
                //model.ClientContractOtherDocumentList = model.ClientContractOtherDocumentList.Skip(Start).Take(Length).ToList();
                //foreach (var item in model.ClientContractOtherDocumentList)
                //{
                //    //item.FilePath = "http://localhost:10561/Contracts/1/09282019093435_01-scansmpl.pdf.PdfCompressor-2496619.pdf";
                //    item.FileName = _configuration["AppSettings:AppURL"].ToString() + _configuration["AppSettings:ContractUploadPath"].ToString() + item.FileName;
                //}
            }
            catch (Exception ex) { }
            return Json(new { data = model.ClientContractOtherDocumentList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<ActionResult> ClientContractsPassThruContract(int contractId)
        {
            Contracts model = new Contracts();
            try
            {
                model.ClientContractPassThruContractList = await _commonService.GetClientContractPassThruContract(contractId);

                //totalrows = (model.ClientContractOtherDocumentList.Count > 0 ? model.ClientContractOtherDocumentList.First().TotalCount : 0);

                //totalRowsAfterFiltering = (model.ClientContractOtherDocumentList.Count > 0 ? model.ClientContractOtherDocumentList.First().TotalCount : 0);
                ////Sorting 
                //model.ClientContractOtherDocumentList = model.ClientContractOtherDocumentList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();
                ////Pageing
                //model.ClientContractOtherDocumentList = model.ClientContractOtherDocumentList.Skip(Start).Take(Length).ToList();
                //foreach (var item in model.ClientContractOtherDocumentList)
                //{
                //    //item.FilePath = "http://localhost:10561/Contracts/1/09282019093435_01-scansmpl.pdf.PdfCompressor-2496619.pdf";
                //    item.FileName = _configuration["AppSettings:AppURL"].ToString() + _configuration["AppSettings:ContractUploadPath"].ToString() + item.FileName;
                //}
            }
            catch (Exception ex) { }
            return Json(new { data = model.ClientContractPassThruContractList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<ActionResult> ClientContractsCrewLogList(int contractId)
        {
            Contracts model = new Contracts();
            try
            {
                model.ClientContractCrewLogList = await _commonService.GetClientContractCrewLogList(contractId);

                //totalrows = (model.ClientContractOtherDocumentList.Count > 0 ? model.ClientContractOtherDocumentList.First().TotalCount : 0);

                //totalRowsAfterFiltering = (model.ClientContractOtherDocumentList.Count > 0 ? model.ClientContractOtherDocumentList.First().TotalCount : 0);
                ////Sorting 
                //model.ClientContractOtherDocumentList = model.ClientContractOtherDocumentList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();
                ////Pageing
                //model.ClientContractOtherDocumentList = model.ClientContractOtherDocumentList.Skip(Start).Take(Length).ToList();
                //foreach (var item in model.ClientContractOtherDocumentList)
                //{
                //    //item.FilePath = "http://localhost:10561/Contracts/1/09282019093435_01-scansmpl.pdf.PdfCompressor-2496619.pdf";
                //    item.FileName = _configuration["AppSettings:AppURL"].ToString() + _configuration["AppSettings:ContractUploadPath"].ToString() + item.FileName;
                //}
            }
            catch (Exception ex) { }
            return Json(new { data = model.ClientContractCrewLogList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<ActionResult> ClientContractsNotesDetails(int ClientId)
        {
            Contracts model = new Contracts();
            try
            {
                model.ClientContractNotesDetailsList = await _commonService.GetClientContractNotesDetails(ClientId);
            }
            catch (Exception ex) { }
            return Json(new { data = model.ClientContractNotesDetailsList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<ActionResult> EditNotes(int Id)
        {
            Contracts model = new Contracts();
            try
            {
                model.ClientContractNotesDetailsList = await _commonService.GetClientContractNote(Id);
            }
            catch (Exception ex) { }
            return Json(new { data = model.ClientContractNotesDetailsList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        [HttpPost]
        public async Task<IActionResult> ContractDetails(int Id)
        {
            string UserId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            Contracts contracts = new Contracts();
            try
            {
                contracts.ClientContractDetails = new ClientContractDetails();
                contracts.ContractAgreementTypeList = await _commonService.GetContractAgreementType();
                contracts.ContractUSersList = await _commonService.GetUsers(UserId);
                contracts.Contractdetails = await _commonService.GetContractDetails(Id);
            }
            catch (Exception ex) { }
            return PartialView("_ContractForm", contracts);
        }
        [HttpPost]
        public async Task<IActionResult> ContractNotesDetails(int Id)
        {
            await _menuUtils.SetMenu(HttpContext.Session);
            Contracts contracts = new Contracts();
            try
            {
                contracts.ContractAgreementTypeList = await _commonService.GetContractAgreementType();
                contracts.Contractdetails = await _commonService.GetContractDetails(Id);
            }
            catch (Exception ex) { }
            return PartialView("_CommentsForm", contracts);
        }

        [HttpPost]
        public async Task<IActionResult> ClientContractDetails(int Id)
        {
            Contracts model = new Contracts();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                model.ClientsDetailsList = await _commonService.GetClients(Id, 0);
                model.ContractStatus = await _commonService.GetContractStatusList(Id);
                model.ClientContractDetailsList = new List<ClientContractDetail>();
                ClientsDetails res = new ClientsDetails();
                AEDetailsList lst = new AEDetailsList();
                if (model.ClientContractDetailsList.Count > 0)
                {
                    var Clientdata = model.ClientsDetailsList.FirstOrDefault();
                    foreach (var item in model.ClientContractDetailsList)
                    {
                        item.FileName = _configuration["AppSettings:AppURL"].ToString() + _configuration["AppSettings:ContractUploadPath"].ToString() + Clientdata.CompanyId + "_" + Clientdata.CompanyName + "//" + item.FileName;
                    }
                }
                if (model.ClientsDetailsList.Count > 0)
                {
                    foreach (var item in model.ClientsDetailsList)
                    {
                        if (item.ID == Id)
                        //if (item.CompanyId == Id.ToString())
                        {
                            res.ID = item.ID;
                            res.CompanyId = item.CompanyId;
                            res.CompanyName = item.CompanyName.ToString();
                            res.ResidencyCode = item.ResidencyCode.ToString();
                            res.ContractSignedDate = item.ContractStartDate;
                            res.ContractEffDate = item.ContractRenewedDate.ToString();
                            res.ContractExpiryDate = item.ContractExpiryDate.ToString();
                            res.AccountExecutiveName = item.AccountExecutiveName;
                            res.ContractLength = item.ContractLength;
                            res.Amount = item.Amount;
                            res.LocationStreetAddress = item.LocationStreetAddress;
                            res.CityId = item.CityId;
                            res.StateId = item.StateId;
                            res.ZipCode = item.ZipCode;
                            res.ReferenceID = item.ReferenceID;
                            res.Active = item.Active;
                            res.Status = item.Status;
                            res.Transport = item.Transport;
                            res.TransportCharges = item.TransportCharges;
                            res.Year1 = item.Year1;
                            res.Year2 = item.Year2;
                            res.Year3 = item.Year3;
                            res.Year4 = item.Year4;
                            res.FeesChangedDate = item.FeesChangedDate;
                            res.Notes = item.Notes;
                        }
                    }
                    if (res.Notes != null)
                    {
                        foreach (var item in model.ContractStatus)
                        {
                            if (res.Notes.ToLower() == item.Value.ToLower())
                            {
                                res.ContractStatusID = item.Id;
                            }
                        }
                    }
                    model.ClientsDetails = res;
                }
            }
            catch (Exception ex)
            { }
            return PartialView("_ClientSearch", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditContract(int Id)
        {
            Contracts contracts = new Contracts();
            try
            {
                //await _menuUtils.SetMenu(HttpContext.Session);
                //ClientContractDetails model = new ClientContractDetails();
                //contracts.ContractAgreementTypeList = await _commonService.GetContractAgreementType();
                //contracts.ContractdetailsList = await _commonService.GetContractDetails(Id);
                //foreach (var item in contracts.ContractdetailsList)
                //{
                //    model.ID = item.ID;
                //    model.ContractSignedDate = item.ContractSignedDate;
                //    model.ContractExpiryDate = item.ContractExpiryDate;
                //    model.AgreementType = item.AggrementTypeId;
                //    model.Status = item.Active;
                //    model.ClientId = item.ClientId;

                //}
                //contracts.ClientContractDetails = model;
            }
            catch (Exception ex)
            {

            }
            //return Json(new { msge = "Success" });
            return PartialView("_ContractForm", contracts);
        }
        [HttpPost]
        public async Task<IActionResult> EmailContractSendToAE([FromBody] EmailContractDetails emailContractDetails)
        {
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                var userId = HttpContext.Session.GetString("UserID");
                Contracts contracts = new Contracts();
                UserOtherDetails userOtherDetails = new UserOtherDetails();
                ClientContractDetails model = new ClientContractDetails();
                StreamReader str = new StreamReader(TemplateRoot);
                string MailText = str.ReadToEnd();
                str.Close();
                bool mail = true;

                SMTP = await _commonService.GetSMTPServerDetails(emailContractDetails.SendTo.ToString());
                if (SMTP.Count > 0)
                {
                    MailText = MailText.Replace("[newusername]", SMTP[0].SentoUserName);
                    MailText = MailText.Replace("[comment]", emailContractDetails.Comments);
                    MailText = MailText.Replace("[Status]", emailContractDetails.Status.Trim());
                    string subjectStatus = string.Empty;
                    if (emailContractDetails.StatusID == 2)
                    {
                        subjectStatus = "Contract Sent for Approval";
                    }
                    else if (emailContractDetails.StatusID == 5)
                    {
                        subjectStatus = "Contract Pending for Approval";
                    }
                    else
                    {
                        subjectStatus = "Contract Approved";
                    }
                    userOtherDetails = await _commonService.GetUserOtherDetails(emailContractDetails.SendTo);
                    if (userOtherDetails.Email.Trim() != "")
                    {
                        if (dummyEmail.Trim() != "")
                            userOtherDetails.Email = dummyEmail;
                        SMTP = await _commonService.GetSMTPServerDetails(emailContractDetails.SendTo.ToString());
                        CommonMail.SendMail(SMTP[0].SMTPServer, SMTP[0].SMTPPort, SMTP[0].SMTPSSL, SMTP[0].SMTPUserName, SMTP[0].SMTPPassword, userOtherDetails.Email.Trim(), subjectStatus, MailText, "", "");
                    }
                }
                emailContractDetails.Email = mail;
                emailContractDetails.UserId = userId;
                await _commonService.UpdateContracMailDetails(emailContractDetails);

                int NotificationCount = await _commonService.GetNotification(userId);
                HttpContext.Session.SetString("NotificationCount", Convert.ToInt32(NotificationCount).ToString());
                HttpContext.Session.Set("SessionNotificationCount", NotificationCount);

                IList<NotificationDetails> NotificationDetails = new List<NotificationDetails>();
                NotificationDetails = await _commonService.GetNotificationDetailsList(userId);
                //Save List in Session
                var s = JsonConvert.SerializeObject(NotificationDetails);
                HttpContext.Session.SetString("my-message", s);
            }
            catch (Exception ex) { }
            return Json(new { msge = "Success" });
        }
        [HttpPost]
        public async Task<IActionResult> EmailContractApprove([FromBody] EmailContractDetails emailContractDetails)
        {
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                var userId = HttpContext.Session.GetString("UserID");
                Contracts contracts = new Contracts();
                UserOtherDetails userOtherDetails = new UserOtherDetails();
                ClientContractDetails model = new ClientContractDetails();

                AEDetailsList lst = new AEDetailsList();
                StreamReader str = new StreamReader(TemplateRoot);
                string MailText = str.ReadToEnd();
                str.Close();
                lst = await _commonService.GetAEDetails(emailContractDetails.ContractID);
                contracts.ClientsDetailsList = await _commonService.GetClients(lst.ClientId, 0);
                var data = contracts.ClientsDetailsList.FirstOrDefault();
                MailText = MailText.Replace("[newusername]", data.CompanyName.Trim());
                MailText = MailText.Replace("[Status]", emailContractDetails.Status.Trim());
                bool mail = true;

                SMTP = await _commonService.GetSMTPServerDetails(userId);
                if (SMTP.Count > 0)
                {
                    userOtherDetails = await _commonService.GetUserOtherDetails(emailContractDetails.SendTo);
                    if (userOtherDetails.Email.Trim() != "")
                    {
                        if (dummyEmail.Trim() != "")
                            userOtherDetails.Email = dummyEmail;
                        CommonMail.SendMail(SMTP[0].SMTPServer, SMTP[0].SMTPPort, SMTP[0].SMTPSSL, SMTP[0].SMTPUserName, SMTP[0].SMTPPassword, userOtherDetails.Email, "Contracts", MailText, "", "");
                    }
                }

                emailContractDetails.ClientId = lst.ClientId;
                emailContractDetails.Email = mail;
                emailContractDetails.UserId = userId;
                emailContractDetails.StatusID = 3;
                await _commonService.UpdateContracMailDetails(emailContractDetails);

                int NotificationCount = await _commonService.GetNotification(userId);
                HttpContext.Session.SetString("NotificationCount", Convert.ToInt32(NotificationCount).ToString());
                HttpContext.Session.Set("SessionNotificationCount", NotificationCount);

                IList<NotificationDetails> NotificationDetails = new List<NotificationDetails>();
                NotificationDetails = await _commonService.GetNotificationDetailsList(userId);
                //Save List in Session
                var s = JsonConvert.SerializeObject(NotificationDetails);
                HttpContext.Session.SetString("my-message", s);

            }
            catch (Exception ex) { }
            return Json(new { msge = "Success" });
        }
        [HttpPost]
        public async Task<IActionResult> EmailContractReject([FromBody] EmailContractDetails emailContractDetails)
        {
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                var userId = HttpContext.Session.GetString("UserID");
                Contracts contracts = new Contracts();
                UserOtherDetails userOtherDetails = new UserOtherDetails();
                ClientContractDetails model = new ClientContractDetails();

                AEDetailsList lst = new AEDetailsList();
                StreamReader str = new StreamReader(TemplateRoot);
                string MailText = str.ReadToEnd();
                str.Close();
                lst = await _commonService.GetAEDetails(emailContractDetails.ContractID);
                contracts.ClientsDetailsList = await _commonService.GetClients(lst.ClientId, 0);
                var data = contracts.ClientsDetailsList.FirstOrDefault();
                MailText = MailText.Replace("[newusername]", data.CompanyName.Trim());
                MailText = MailText.Replace("[Status]", emailContractDetails.Status.Trim());
                bool mail = true;

                SMTP = await _commonService.GetSMTPServerDetails(userId);
                if (SMTP.Count > 0)
                {
                    userOtherDetails = await _commonService.GetUserOtherDetails(emailContractDetails.SendTo);
                    if (userOtherDetails.Email.Trim() != "")
                    {
                        if (dummyEmail.Trim() != "")
                            userOtherDetails.Email = dummyEmail;
                        CommonMail.SendMail(SMTP[0].SMTPServer, SMTP[0].SMTPPort, SMTP[0].SMTPSSL, SMTP[0].SMTPUserName, SMTP[0].SMTPPassword, userOtherDetails.Email, "Contracts", MailText, "", "");
                    }
                }

                emailContractDetails.ClientId = lst.ClientId;
                emailContractDetails.Email = mail;
                emailContractDetails.UserId = userId;
                emailContractDetails.StatusID = 4;
                await _commonService.UpdateContracMailDetails(emailContractDetails);

                int NotificationCount = await _commonService.GetNotification(userId);
                HttpContext.Session.SetString("NotificationCount", Convert.ToInt32(NotificationCount).ToString());
                HttpContext.Session.Set("SessionNotificationCount", NotificationCount);

                IList<NotificationDetails> NotificationDetails = new List<NotificationDetails>();
                NotificationDetails = await _commonService.GetNotificationDetailsList(userId);
                //Save List in Session
                var s = JsonConvert.SerializeObject(NotificationDetails);
                HttpContext.Session.SetString("my-message", s);

            }
            catch (Exception ex) { }
            return Json(new { msge = "Success" });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteContract(int Id)
        {
            await _menuUtils.SetMenu(HttpContext.Session);
            Contracts model = new Contracts();

            int ClientId = await _commonService.DeleteContract(Id);
            //model.ClientContractDetailsList = await _commonService.GetClientContractDetails(ClientId);
            //model.ContractAgreementTypeList = await _commonService.GetContractAgreementType();
            //foreach (var item in model.ClientContractDetailsList)
            //{
            //    //item.FilePath = "http://localhost:10561/Contracts/1/09282019093435_01-scansmpl.pdf.PdfCompressor-2496619.pdf";
            //    item.FileName = _configuration["AppSettings:AppURL"].ToString() + _configuration["AppSettings:ContractUploadPath"].ToString() + ClientId + "//" + item.FileName;
            //}

            return Json(new { ClientId = ClientId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOtherDocument(int Id)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            Contracts model = new Contracts();

            int ClientId = await _commonService.DeleteOtherDocument(Id, userId);

            return Json(new { ClientId = ClientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePassThruContract(int Id)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            Contracts model = new Contracts();

            int ClientId = await _commonService.DeletePassThruContract(Id, userId);

            return Json(new { ClientId = ClientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCrewLogList(int Id)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            Contracts model = new Contracts();

            int ClientId = await _commonService.DeleteCrewLog(Id, userId);

            return Json(new { ClientId = ClientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientContractNotes(int Id)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            Contracts model = new Contracts();

            int ClientId = await _commonService.DeleteClientContarctNotes(Id, userId);

            return Json(new { ClientId = ClientId });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveContractDetails([FromBody] ClientContractEditDetails clientContractDetails)
        {
            var userId = HttpContext.Session.GetString("UserID");
            Contracts contracts = new Contracts();
            // int ContractId;
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                contracts.ClientContractEditDetails = clientContractDetails;
                ContractId = await _commonService.UpdateEditContractDetails(clientContractDetails, userId);
            }
            catch (Exception ex)
            { }
            return Json(new { contractId = ContractId, msge = "Success" });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveContractNotesDetails([FromBody] NotesDetails NotesDetails)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                await _commonService.UpdateNotesDetails(NotesDetails, userId);
            }
            catch (Exception ex)
            { }
            return Json(new { msge = "Success" });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveClientContractNotes(int Id, int ClientID, int ContractId,int InsuranceContractId, string Notes)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                await _commonService.UpdateClientContarctNotesDetails(Id, ClientID, ContractId, InsuranceContractId, Notes, userId);
            }
            catch (Exception ex)
            { }
            return Json(new { msge = "Success" });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveContractFiles(IFormFile file, int ClientId, int ContractId)
        {
            var userId = HttpContext.Session.GetString("UserID");
            Contracts contracts = new Contracts();
            ContractFileDetails model = new ContractFileDetails();
            if (file != null)
            {
                Clients cls = new Clients();
                cls.ClientsDetailsList = await _adminService.GetClients(ClientId, 0);
                var clientdata = cls.ClientsDetailsList.FirstOrDefault();

                //4
                var VersionDetails = await _commonService.SelectContractScreen(ClientId);
                //01042021065031_#501 Bhanumathi Check Services Pvt Ltd V4_V4.pdf
                string FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + Path.GetFileNameWithoutExtension(file.FileName) + "_V" + VersionDetails + Path.GetExtension(file.FileName);
                //501\01042021065041_#501 Bhanumathi Check Services Pvt Ltd V4_V4.pdf
                string Direc_FileName = clientdata.CompanyId + "\\" + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + Path.GetFileNameWithoutExtension(file.FileName) + "_V" + VersionDetails + Path.GetExtension(file.FileName);
                //D:\Common\CMS\Contracts\501\01042021065041_#501 Bhanumathi Check Services Pvt Ltd V4_V4.pdf
                string Savepath = Path.Combine(StorageRoot, Direc_FileName);
                //D:\Common\CMS\Contracts\\501
                string fullPaths = Path.Combine(StorageRoot + "\\" + clientdata.CompanyId);

                if (!Directory.Exists(fullPaths))
                    Directory.CreateDirectory(fullPaths);

                if (Directory.Exists(fullPaths))
                {
                    //D:\Common\CMS\Contracts\501\01042021065041_#501 Bhanumathi Check Services Pvt Ltd V4_V4.pdf
                    using (var stream = new FileStream(Savepath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        stream.Flush();
                        stream.Close();
                    }
                }
                model.ClientID = ClientId;
                model.ContractID = ContractId;
                model.FileName = FileName;
                model.OrgFileName = file.FileName;
                model.FileSize = file.Length;
                model.MimeType = Path.GetExtension(file.FileName);
                model.UserId = userId;
                model.FilePath = Savepath;

                await _commonService.UpdateContractFileDetails(model);

                contracts.ClientsDetailsList = await _commonService.GetClients(ClientId, 0);
                contracts.ContractStatus = await _commonService.GetContractStatusList(0);
                // contracts.ClientContractDetailsList = await _commonService.GetClientContractDetails(ClientId);
                ClientsDetails res = new ClientsDetails();
                if (contracts.ClientsDetailsList.Count > 0)
                {
                    foreach (var item in contracts.ClientsDetailsList)
                    {
                        if (item.ID == ClientId)
                        {
                            res.ID = item.ID;
                            res.CompanyId = item.CompanyId;
                            res.CompanyName = item.CompanyName.ToString();
                            res.ResidencyCode = item.ResidencyCode.ToString();
                            res.ContractSignedDate = item.ContractStartDate;
                            res.ContractEffDate = item.ContractRenewedDate.ToString();
                            res.ContractExpiryDate = item.ContractExpiryDate.ToString();
                            res.AccountExecutiveName = item.AccountExecutiveName;
                            res.ContractLength = item.ContractLength;
                            res.Amount = item.Amount;
                            res.LocationStreetAddress = item.LocationStreetAddress;
                            res.CityId = item.CityId;
                            res.StateId = item.StateId;
                            res.ZipCode = item.ZipCode;
                            res.ReferenceID = item.ReferenceID;
                            res.Active = item.Active;
                            res.Status = item.Status;
                            res.Year1 = item.Year1;
                            res.Year2 = item.Year2;
                            res.Year3 = item.Year3;
                            res.Year4 = item.Year4;
                            res.FeesChangedDate = item.FeesChangedDate;
                            res.Notes = item.Notes;
                        }
                    }
                    contracts.ClientsDetails = res;

                }
            }
            return PartialView("_ClientSearch", contracts);
        }

        public async Task<IActionResult> ContractApproval()
        {
            Contracts model = new Contracts();
            try
            {
                model.ContractAgreementTypeList = await _commonService.GetContractAgreementType();
            }
            catch (Exception ex) { }
            return View(model);
        }
        public async Task<IActionResult> Comments()
        {
            Notes model = new Notes();
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<IActionResult> LoadCommentsList(string Page)
        {

            Notes model = new Notes();

            string UserId = HttpContext.Session.GetString("UserID");
            try
            {
                sortColumnName = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"] + "][name]"];
                sortDirection = HttpContext.Request.Form["order[0][dir]"];
                val = Page;
                Start = Convert.ToInt32(HttpContext.Request.Form["start"]);
                Length = Convert.ToInt32(HttpContext.Request.Form["length"]);
                model.CommentsDetailsList = await _commonService.GetCommentsList(Start, Length);
                totalrows = (model.CommentsDetailsList.Count > 0 ? model.CommentsDetailsList.First().TotalCount : 0);
                totalRowsAfterFiltering = (model.CommentsDetailsList.Count > 0 ? model.CommentsDetailsList.First().TotalCount : 0);
                //Sorting 
                model.CommentsDetailsList = model.CommentsDetailsList.AsQueryable().OrderBy(sortColumnName + " " + sortDirection).ToList();
                //Pageing
                model.CommentsDetailsList = model.CommentsDetailsList.Skip(Start).Take(Length).ToList();
            }
            catch (Exception Ex) { }
            return Json(new { data = model.CommentsDetailsList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });

        }

        [HttpPost]
        public async Task<IActionResult> UpdateViewDetails(int companyId, string Version, int ContractId, string Status)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                await _commonService.UpdateViewHistroyDetails(companyId, Version, ContractId, userId, Status);
            }
            catch (Exception ex)
            { }
            return Json(new { msge = "Success" });
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailToClient(string Subject, string Body, string MailTo, string Attachment)
        {
            try
            {
                string cc = "";
                string[] mailIds = MailTo.Split(",");
                Body = Body.Replace("\n", "<br/>");
                Body = Body + "<br/><br/>";
                await _menuUtils.SetMenu(HttpContext.Session);
                var userId = HttpContext.Session.GetString("UserID");
                Contracts contracts = new Contracts();
                ClientContractDetails model = new ClientContractDetails();
                AEDetailsList lst = new AEDetailsList();

                SMTP = await _commonService.GetSMTPServerDetails(userId);

                if (SMTP.Count > 0)
                {
                    if (dummyEmail.Trim() != "")
                    {
                        CommonMail.SendMail(SMTP[0].SMTPServer, SMTP[0].SMTPPort, SMTP[0].SMTPSSL, SMTP[0].SMTPUserName, SMTP[0].SMTPPassword, dummyEmail, Subject, Body, cc, Path.Combine(_configuration["AppSettings:ContractUploadPath"].ToString(), Attachment));
                    }
                    else
                    {
                        foreach (string mailId in mailIds)
                        {
                            if (mailId.Trim() != "")
                            {
                                CommonMail.SendMail(SMTP[0].SMTPServer, SMTP[0].SMTPPort, SMTP[0].SMTPSSL, SMTP[0].SMTPUserName, SMTP[0].SMTPPassword, mailId, Subject, Body, cc, Path.Combine(_configuration["AppSettings:ContractUploadPath"].ToString(), Attachment));
                            }
                        }
                    }


                }
            }
            catch (Exception ex) { }
            return Json(new { msge = "Success" });
        }

        public async Task<IActionResult> ViewContractFile(string fileName, int contractId, int id, string Version, string Status)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            await _commonService.UpdateViewHistroyDetails(id, Version, contractId, userId, "Contract Viewed");
            return GetFileFromPath(fileName);
        }

        public async Task<IActionResult> ViewOtherContractFile(string fileName, int clientId)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            await _commonService.UpdateViewHistroyDetails(clientId, "v", clientId, userId, "Other Documents Viewed");
            return GetFileFromPath(fileName);
        }

        public async Task<IActionResult> ViewPassThruContractFile(string fileName, int clientId)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            await _commonService.UpdateViewHistroyDetails(clientId, "v", clientId, userId, "Pass-Thru Contract Documents Viewed");
            return GetFileFromPath(fileName);
        }

        public async Task<IActionResult> ViewCrewLogListFile(string fileName, int clientId)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            await _commonService.UpdateViewHistroyDetails(clientId, "v", clientId, userId, "Crew Log Documents Viewed");
            return GetFileFromPath(fileName);
        }

        public FileStreamResult GetFileFromPath(string fileName)
        {
            var fileStream = new FileStream(_configuration["AppSettings:ContractUploadPath"].ToString() + HttpUtility.HtmlDecode(fileName), FileMode.Open, FileAccess.Read);
            string Extn = Path.GetExtension(fileName.Trim());

            switch (Extn)
            {
                case ".pdf":
                    Response.ContentType = "application/pdf";
                    break;
                case ".doc":
                    Response.ContentType = "application/msword";
                    break;
                case ".docx":
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xls":
                    Response.ContentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
            }

            var cd = new ContentDispositionHeaderValue("inline")
            {
                FileNameStar = Path.GetFileName(fileName)
            };
            Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());

            var fsResult = File(fileStream, Response.ContentType);
            return fsResult;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateClientStatus([FromBody] UpdateContractStatus updateContractStatus)
        {
            Contracts model = new Contracts();
            var Id = updateContractStatus.ID;
            var CompanyID = updateContractStatus.CompanyId;
            var NotesId = updateContractStatus.ContractStatusID;
            var NotesData = "";
            var userId = HttpContext.Session.GetString("UserID");
            model.ContractStatus = await _commonService.GetContractStatusList(Id);

            foreach (var item in model.ContractStatus)
            {
                if (item.Id == NotesId)
                {
                    NotesData = item.Value;
                }
            }
            var Notes = "Contract Status Changed to " + NotesData;
            await _commonService.UpdateContractStatus(Id, CompanyID, NotesData);
            await _commonService.UpdateViewHistroyDetails(Id, "V1", 0, userId, Notes);
            return Json(new { msge = "Success" });
        }

        #region InsuranceContract

        [HttpPost]
        public async Task<IActionResult> InsuranceContractList(int id)
        {
            Contracts model = new Contracts();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {

                model.ClientInsuranceContractFileDetailsList = await _commonService.GetClientInsuranceContractFileDetailsList(id);
                totalrows = (model.ClientInsuranceContractFileDetailsList.Count > 0 ? model.ClientInsuranceContractFileDetailsList.Count : 0);
                totalRowsAfterFiltering = (model.ClientInsuranceContractFileDetailsList.Count > 0 ? model.ClientInsuranceContractFileDetailsList.Count : 0);
            }
            catch (Exception Ex) { }
            return Json(new { data = model.ClientInsuranceContractFileDetailsList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        public async Task<IActionResult> ViewInsuranceContractFile(string fileName, int insuranceContractId, int id)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            //await _commonService.UpdateViewHistroyDetails(id, Version, contractId, userId, "Contract Viewed");
            return GetInsuranceFileFromPath(fileName, insuranceContractId);
        }

        public FileStreamResult GetInsuranceFileFromPath(string fileName, int insuranceContractId)
        {
            //var fileStream = new FileStream(_configuration["AppSettings:ContractUploadPath"].ToString() + "InsuranceContract\\" + insuranceContractId + "\\" + HttpUtility.HtmlDecode(fileName), FileMode.Open, FileAccess.Read);
            var fileStream = new FileStream(_configuration["AppSettings:ContractUploadPath"].ToString() + "InsuranceContract\\" + HttpUtility.HtmlDecode(fileName), FileMode.Open, FileAccess.Read);
            string Extn = Path.GetExtension(fileName.Trim());

            switch (Extn)
            {
                case ".pdf":
                    Response.ContentType = "application/pdf";
                    break;
                case ".doc":
                    Response.ContentType = "application/msword";
                    break;
                case ".docx":
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xls":
                    Response.ContentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
            }

            var cd = new ContentDispositionHeaderValue("inline")
            {
                FileNameStar = Path.GetFileName(fileName)
            };
            Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());

            var fsResult = File(fileStream, Response.ContentType);
            return fsResult;
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> DownloadContractNotes([FromBody] int ClientId)
        {
            List<DownloadContractNotes> model = new List<DownloadContractNotes>();
            string UserId = HttpContext.Session.GetString("UserID");
            model = await _commonService.ExportContractNotes(ClientId);
            var data = model.Select(x=>new { x.InsuranceContractName ,x.Notes,x.AddedBy,x.AddedOn});
            string sFileName = @"ContractNotes.xlsx";
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


        #region Annual charge Rate

        [HttpPost]
        public async Task<IActionResult> GetAnnualChargeRateList(string companyId)
        {
            Contracts model = new Contracts();
            string UserId = HttpContext.Session.GetString("UserID");
            try
            {

                model.AnnualChargeRateList = await _adminService.GetAnnualChargeRateList(companyId);
                totalrows = (model.AnnualChargeRateList.Count > 0 ? model.AnnualChargeRateList.Count : 0);
                totalRowsAfterFiltering = (model.AnnualChargeRateList.Count > 0 ? model.AnnualChargeRateList.Count : 0);
            }
            catch (Exception Ex) { }
            return Json(new { data = model.AnnualChargeRateList, draw = HttpContext.Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalRowsAfterFiltering });
        }

        public async Task<IActionResult> ViewAnnualChargeRateFile(string fileName)
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            //await _commonService.UpdateViewHistroyDetails(id, Version, contractId, userId, "Contract Viewed");
            return GetAnnualChargeRateFileFromPath(fileName);
        }

        public FileStreamResult GetAnnualChargeRateFileFromPath(string fileName)
        {
            //var fileStream = new FileStream(_configuration["AppSettings:ContractUploadPath"].ToString() + "InsuranceContract\\" + insuranceContractId + "\\" + HttpUtility.HtmlDecode(fileName), FileMode.Open, FileAccess.Read);
            var fileStream = new FileStream(_configuration["AppSettings:ContractUploadPath"].ToString() + "AnnualChargeRate\\" + HttpUtility.HtmlDecode(fileName), FileMode.Open, FileAccess.Read);
            string Extn = Path.GetExtension(fileName.Trim());

            switch (Extn)
            {
                case ".pdf":
                    Response.ContentType = "application/pdf";
                    break;
                case ".doc":
                    Response.ContentType = "application/msword";
                    break;
                case ".docx":
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xls":
                    Response.ContentType = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
            }

            var cd = new ContentDispositionHeaderValue("inline")
            {
                FileNameStar = Path.GetFileName(fileName)
            };
            Response.Headers.Add(HeaderNames.ContentDisposition, cd.ToString());

            var fsResult = File(fileStream, Response.ContentType);
            return fsResult;
        }

        #endregion
    }
}