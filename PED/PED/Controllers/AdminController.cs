using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WiseX.Models;
using WiseX.ViewModels.Admin;
using System.Collections.Generic;
using WiseX.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WiseX.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Data;
using WiseX.Helpers;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Hosting.Internal;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using static WiseX.Helpers.FilesHelper;
using OfficeOpenXml;
using PED.ViewModels.Admin;
using PED.ViewModels.Contract;
using System.Globalization;

namespace WiseX.Controllers
{
    //[Authorize]
    [SessionTimeout]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
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

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext, RoleManager<ApplicationRole> roleManager, IHostingEnvironment hostingEnvironment, IConfiguration configuration, IAuthorizationService authorizationService)
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


        //To Fill Roles drop down we can use GetRoles() Function        
        protected List<Roles> GetRoles()
        {
            List<Roles> ListUserRoles = new List<Roles>();
            try
            {
                //User Login based Role Name 
                var Roles = HttpContext.Session.GetString("UserRoles");
                List<string> obj = JsonConvert.DeserializeObject<List<string>>(Roles);
                //All Roles
                ListUserRoles = _roleManager.Roles.Select(r => new Roles
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList();
                //If User Role is Administrator then Drop down should not display Administrator. 
                if (obj.Contains("SuperAdministrator"))
                {
                    ListUserRoles = _roleManager.Roles.Where(m => m.Name != "SuperAdministrator").Select(r => new Roles
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).OrderBy(m => m.Name).ToList();
                }
                else
                {
                    ListUserRoles = _roleManager.Roles.Select(r => new Roles
                    {
                        Id = r.Id,
                        Name = r.Name
                    }).OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception Ex)
            {

            }
            return ListUserRoles;
        }
        //Roles View - To Load the List with Data
        public async Task<IActionResult> Roles()
        {
            List<Roles> rolelst = new List<Roles>();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                rolelst = await _adminService.GetRolesDescStatus("", 0);
            }
            catch (Exception ex)
            {

            }
            return View("Roles", rolelst);
        }

        //To Check if Role Name Exsit or Not
        [HttpPost]
        public async Task<JsonResult> IsRoleNameExsit(string RoleName)
        {
            try
            {
                string value = RoleName;
                bool Roles = _roleManager.Roles.Any(x => x.Name == RoleName);
                if (Roles)
                {
                    return Json(new { message = "Failed" });
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }
        //To Create New Role
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateRoles([FromBody] Roles roles)
        {
            User model = new User();
            List<Roles> rolelst = new List<Roles>();
            try
            {
                string Rid = roles.Id;
                string Name = roles.Name;
                var RoleNmae = _roleManager.FindByNameAsync(Name);
                if (roles.Id == "0")
                {
                    //Create role
                    var myroles = new ApplicationRole
                    {
                        Name = roles.Name
                    };
                    var result = await _roleManager.CreateAsync(myroles);
                    var Roles = _roleManager.GetRoleIdAsync(myroles);
                    await _adminService.UpdateRoleDetails(Roles.Result, roles.Status, roles.Permissions);

                }
                else
                {
                    //Update role
                    await _adminService.UpdateRoleDetails(roles.Id, roles.Status, roles.Permissions);
                }
            }
            catch (Exception Ex)
            {

            }
            rolelst = await _adminService.GetRolesDescStatus("", 0);
            return PartialView("_RoleList", rolelst);
        }
        [HttpPost]
        public async Task<ActionResult> EditRole(string RoleId)
        {
            Roles roles = new Roles();
            try
            {
                var ListUserRoles = await _adminService.GetRolesDescStatus(RoleId, 1);
                foreach (var item in ListUserRoles)
                {
                    roles.Id = item.Id;
                    roles.Name = item.Name;
                    roles.Active = item.Active;
                    roles.Permissions = item.Permissions;
                }
            }
            catch (Exception ex)
            { }
            return Json(roles);
        }
        //To Delete Roles
        [HttpPost]
        public async Task<IActionResult> DeleteRoles(string RoleId)
        {
            List<Roles> ListUserRoles = new List<Roles>();
            IList<MenuItem> menulist = new List<MenuItem>();
            try
            {
                var user = await _adminService.GetUserRole(RoleId);
                menulist = await _adminService.GetMenuList(RoleId);
                if (user.Count > 0 || menulist.Where(x => x.selected == true).Count() >= 1)
                {
                    return Json(new { message = "Exist" });
                }
                else
                {
                    ApplicationRole identityRole = await _roleManager.FindByIdAsync(RoleId);
                    if (identityRole != null && menulist.Where(x => x.selected == false).Count() != 1)
                    {
                        await _adminService.DeleteRoleDescDetails(RoleId);
                        //  var result = await _roleManager.DeleteAsync(identityRole);
                    }
                }
            }
            catch (Exception Ex)
            {

            }
            ListUserRoles = await _adminService.GetRolesDescStatus("", 0);
            return PartialView("_RoleList", ListUserRoles);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeePosition()
        {
            Position position = new Position();
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                position.PositionList = await _adminService.GetPositionListStatus(0);
            }
            catch (Exception ex) { }
            return View("EmployeePosition", position);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveEmployeePosition([FromBody] PositionDetails positionDetails)
        {
            Position model = new Position();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");
                await _menuUtils.SetMenu(HttpContext.Session);
                model.PositionDetails = positionDetails;
                await _adminService.UpdateEmployeePositionDetails(model, userId);
                model.PositionList = await _adminService.GetPositionListStatus(0);
            }
            catch (Exception ex)
            {

            }
            return PartialView("_EmployeePositionList", model.PositionList);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditEmployeePosition([FromBody] int ID)
        {
            Position model = new Position();
            try
            {
                model.PositionList = await _adminService.GetPositionListStatus(ID);
                {
                    PositionDetails res = new PositionDetails();
                    if (model.PositionList.Count > 0)
                    {
                        foreach (var item in model.PositionList)
                        {
                            if (item.Id == ID)
                            {
                                res.Id = item.Id;
                                res.Name = item.Name.ToString();
                                res.Status = item.Status;
                            }
                        }
                        model.PositionDetails = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_EmployeePositionFrom", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteEmployeePosition(int ID)
        {
            Position model = new Position();
            try
            {
                await _adminService.DeleteEmployeePositionDetails(ID);
                model.PositionList = await _adminService.GetPositionListStatus(0);
            }
            catch (Exception ex) { }
            return PartialView("_EmployeePositionList", model.PositionList);
        }


        [HttpGet]
        public async Task<IActionResult> AgreementType()
        {
            AgreementType AgreementType = new AgreementType();
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                AgreementType.AgreementTypeDetailsList = await _adminService.GetAgreementTypeDescStatus(0, 0);
            }
            catch (Exception ex) { }
            return View("AgreementType", AgreementType);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveAgreementType([FromBody] AgreementTypeDetails agreementType)
        {
            AgreementType model = new AgreementType();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");
                await _menuUtils.SetMenu(HttpContext.Session);
                model.AgreementTypeDetails = agreementType;
                await _adminService.UpdateAgreementTypeDetails(model, userId);
                model.AgreementTypeDetailsList = await _adminService.GetAgreementTypeDescStatus(0, 0);
            }
            catch (Exception ex)
            {

            }
            return PartialView("_AgreementTypeList", model.AgreementTypeDetailsList);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditAgreementType([FromBody] int AgreementID)
        {
            AgreementType model = new AgreementType();
            try
            {
                model.AgreementTypeDetailsList = await _adminService.GetAgreementTypeDescStatus(AgreementID, 0);
                {
                    AgreementTypeDetails res = new AgreementTypeDetails();
                    if (model.AgreementTypeDetailsList.Count > 0)
                    {
                        foreach (var item in model.AgreementTypeDetailsList)
                        {
                            if (item.Id == AgreementID)
                            {
                                res.Id = item.Id;
                                res.Name = item.Name.ToString();
                                res.Description = item.Description.ToString();
                                res.Status = item.Status;
                            }
                        }
                        model.AgreementTypeDetails = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_AgreementTypeForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAgreementType(int AgreementTypeID)
        {
            AgreementType model = new AgreementType();
            try
            {
                await _adminService.DeleteAgreementTypeDetails(AgreementTypeID);
                model.AgreementTypeDetailsList = await _adminService.GetAgreementTypeDescStatus(AgreementTypeID, 0);
            }
            catch (Exception ex) { }
            return PartialView("_AgreementTypeList", model.AgreementTypeDetailsList);
        }


        //Initial Page of User during the Load
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            Users users = new Users();
            try
            {
                users.UserList = await _adminService.GetUserList(userId);
                users.RolesList = await _adminService.GetRolesDescStatus("", 1);
                users.ClientList = await _adminService.GetClients();
                if (users.UserDetails != null)
                {
                    users.UserDetails.UserDetailId = 0;
                }
            }
            catch (Exception ex) { }
            return View("Users", users);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Users([FromBody] Users users)
        {
            IdentityResult identityUserResult = new IdentityResult();
            IdentityResult identityRoleResult;
            List<UserListInfo> userlistinfo = new List<UserListInfo>();
            int ProjectID = 0;
            if (HttpContext.Session.GetString("ProjectID") != null)
                ProjectID = Int32.Parse(HttpContext.Session.GetString("ProjectID"));
            string UserId = HttpContext.Session.GetString("UserID");

            var myuser = new ApplicationUser();
            myuser = new ApplicationUser
            {
                Email = users.Email,
                PhoneNumber = users.PhoneNumber,
                UserName = users.UserName

            };
            try
            {
                //To ensure that the datatypes match the model given data types
                if (ModelState.IsValid)
                {
                    var existingUserDetails = await _userManager.FindByNameAsync(users.UserName);
                    //Exisiting User
                    if (existingUserDetails != null)
                    {
                        var phoneNumber = users.PhoneNumber;
                        if (existingUserDetails.PhoneNumber != phoneNumber)
                        {
                            var setPhoneNumber = await _userManager.SetPhoneNumberAsync(existingUserDetails, phoneNumber);
                            if (!setPhoneNumber.Succeeded)
                            {
                                throw new ApplicationException($"Unexpected Error Occured while updating Phonenumber of '{ existingUserDetails.UserName}' ");
                            }
                        }
                        var emailId = users.Email;
                        if (existingUserDetails.Email != emailId)
                        {
                            var setEmailId = await _userManager.SetEmailAsync(existingUserDetails, emailId);
                            if (!setEmailId.Succeeded)
                            {
                                throw new ApplicationException($"Unexpected Error Occured while updating Email Id of '{ existingUserDetails.UserName}' ");
                            }

                        }
                        var hasPassword = users.PasswordHash;
                        if (existingUserDetails.PasswordHash != hasPassword)
                        {
                            var code = await _userManager.GeneratePasswordResetTokenAsync(existingUserDetails);
                            var setPassword = await _userManager.ResetPasswordAsync(existingUserDetails, code, hasPassword);
                            if (!setPassword.Succeeded)
                            {
                                throw new ApplicationException($"Unexpected Error Occured while updating Password Id of '{ existingUserDetails.UserName}' ");
                            }
                        }
                        await _userManager.UpdateAsync(existingUserDetails);
                        //To remove user from existing roles
                        //var identityRoleResult1 = await _userManager.GetRolesAsync(username);


                        IList<string> existingRoles = await _userManager.GetRolesAsync(existingUserDetails);
                        if (existingRoles.Count > 0)
                        {
                            await _userManager.RemoveFromRolesAsync(existingUserDetails, existingRoles);
                        }
                        await _userManager.AddToRolesAsync(existingUserDetails, users.UserDetails.ClientRoles.Select(m => m.RoleName));

                        users.UserDetails.UserID = existingUserDetails.Id;
                    }

                    //New User
                    else
                    {
                        //To bind application user 
                        //Creates a Users with the given password
                        identityUserResult = await _userManager.CreateAsync(myuser, users.PasswordHash);

                        ApplicationUser user = await _userManager.FindByNameAsync(myuser.UserName);
                        users.UserDetails.UserID = user.Id;

                        //To check whether role exists and add user to that role  
                        if (identityUserResult.Succeeded)
                        {
                            identityRoleResult = await _userManager.AddToRolesAsync(myuser, users.UserDetails.ClientRoles.Select(m => m.RoleName));
                        }
                        else
                        {
                            // throw new ApplicationException($"User Name already exist'{ existingUserDetails.UserName}' ");
                            return Json(new { message = "UserExistsError" });
                        }
                    }

                    //To update user other details
                    await _adminService.UpdateUserDetails(users, UserId);

                    //To get users list
                    userlistinfo = await _adminService.GetUserList(UserId);
                }
                else
                {
                    return View(users);
                }
            }
            catch (Exception e)
            {
                var check = e;
            }
            ModelState.Clear();

            //To get users list
            userlistinfo = await _adminService.GetUserList(UserId);
            users.UserList = userlistinfo;
            return PartialView("_UsersList", users);
            //return View(users);

        }

        [ResponseCache(NoStore = true, Duration = 0)]
        public List<UserListInfo> GetUserListInfo()
        {
            List<UserListInfo> UserListInfo = new List<UserListInfo>();
            try
            {
            }
            catch (Exception ex) { }
            return UserListInfo;
        }
        // To Bring the Data to the User Form When Edit is clicked
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserEdit([FromBody] Users users)
        {
            var id = users.Id;
            IList<UserDetailsTemp> userDetailsTempList;
            int ProjectID = 0;
            if (HttpContext.Session.GetString("ProjectID") != null)
                ProjectID = Int32.Parse(HttpContext.Session.GetString("ProjectID"));
            var userId = HttpContext.Session.GetString("UserID");
            users.ClientList = await _adminService.GetClientUserdetails(id);

            //Gets all the UserDetails
            users.UserList = await _adminService.GetUserList(userId);
            users.RolesList = GetRoles();
            UserDetails userDetailsTemp = new UserDetails();
            userDetailsTempList = await _adminService.GetUserDetails(id);
            List<UserProjectRole> userProjectRoleList = new List<UserProjectRole>();
            try
            {
                if (userDetailsTempList.Count > 0)
                {
                    foreach (var item in userDetailsTempList)
                    {
                        userDetailsTemp.UserDetailId = item.UserDetailId;
                        userDetailsTemp.Gender = item.Gender;
                        userDetailsTemp.FirstName = item.FirstName;
                        userDetailsTemp.LastName = item.LastName;
                        users.PhoneNumber = item.PhoneNumber;
                        users.Email = item.Email;
                        users.UserName = item.UserName;
                        users.PasswordHash = item.PasswordHash;
                        userDetailsTemp.Active = item.Active;
                        userDetailsTemp.UserRole = item.RoleID;
                        userDetailsTemp.Client = item.ClientId;
                        userDetailsTemp.Permissions = item.Permissions;

                    }
                    users.UserDetails = userDetailsTemp;
                }
            }
            catch (Exception e)
            {

            }
            ModelState.Clear();
            return PartialView("_UserForm", users);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetClient(string UserId)
        {
            Users users = new Users();
            try
            {
                users.ClientList = await _adminService.GetEditClients(UserId);
            }
            catch (Exception ex) { }
            return Json(users.ClientList);
        }

        //To Check if user Name Exsit or Not
        [HttpPost]
        public async Task<JsonResult> IsUserNameExsit(string UserName)
        {
            try
            {
                string value = UserName;
                Users users = new Users();
                int ProjectID = 0;
                if (HttpContext.Session.GetString("ProjectID") != null)
                    ProjectID = Int32.Parse(HttpContext.Session.GetString("ProjectID"));
                var UserID = HttpContext.Session.GetString("UserID");

                users.UserList = await _adminService.GetUserList(UserID);
                bool data = users.UserList.Any(m => m.UserName.ToUpper() == value.ToUpper());
                if (data)
                {
                    return Json(new { message = "Failed" });
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }


        [HttpGet]
        public async Task<IActionResult> Clients()
        {
            Clients clients = new Clients();
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                clients.ClientsDetailsList = await _adminService.GetClients(0, 0);
                clients.EmployeePositionList = await _adminService.GetEmployeePositionList();
                clients.AccountExecutiveList = await _adminService.GetAccountExecutiveList();
                clients.ResidencyCodeList = await _adminService.GetResidencyCodeList();
                //clients.StatesList = new List<StatesList>();
                //clients.CitiesList = await _adminService.GetCitiesList();
                clients.CitiesList = new List<CitiesList>();
                clients.StatesList = await _adminService.GetStatesList();
                clients.ClientsEmployeeList = new List<ClientsEmployeeList>();
                clients.ContractAgreementTypeList = await _commonService.GetContractAgreementType();
                clients.ClientsDetails = new ClientsDetails();
                clients.RoleAccess = HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "");

                clients.InsuranceContractList = await _adminService.GetInsuranceContract(0);
                clients.ClientsInsuranceContractList = new List<ClientsInsuranceContractList>();
                clients.AnnualChargeRateList = new List<AnnualChargeRateList>();

            }
            catch (Exception ex)
            {

            }
            return View("Clients", clients);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveClients([FromBody] ClientsDetails clientsDetails)
        {
            Clients model = new Clients();
            ClientContract clientContract = new ClientContract();
            var userId = HttpContext.Session.GetString("UserID");
            int ClientId = 0;
            try
            {
                await _menuUtils.SetMenu(HttpContext.Session);
                clientsDetails.CreatedBy = HttpContext.Session.GetString("UserID");
                model.ClientsDetails = clientsDetails;
                clientContract = await _adminService.UpdateClientsDetails(model, userId);
                model.ClientsDetailsList = await _adminService.GetClients(0, 0);
                model.ClientsEmployeeList = new List<ClientsEmployeeList>();
                model.ContractAgreementTypeList = await _commonService.GetContractAgreementType();

                model.ClientsInsuranceContractList = new List<ClientsInsuranceContractList>();
                model.AnnualChargeRateList = new List<AnnualChargeRateList>();
            }
            catch (Exception ex)
            {

            }
            //return Json(new { ClientId = clientContract.ClientId, ContractId = clientContract.ContractId });
            return PartialView("_ClientsList", model.ClientsDetailsList);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveClientContractFiles(IFormFile file, int ClientId, int ContractId)
        {
            var userId = HttpContext.Session.GetString("UserID");
            Clients clients = new Clients();
            try
            {
                clients.ClientsDetailsList = await _adminService.GetClients(ClientId, 0);
                ContractFileDetails model = new ContractFileDetails();
                if (file != null)
                {
                    var clientdata = clients.ClientsDetailsList.FirstOrDefault();
                    string FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Direc_FileName = clientdata.CompanyId + "\\" + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;

                    string Savepath = Path.Combine(StorageRoot, Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "\\" + clientdata.CompanyId);

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
                    model.ClientID = ClientId;
                    model.ContractID = ContractId;
                    model.FileName = FileName;
                    model.OrgFileName = file.FileName;
                    model.FileSize = file.Length;
                    model.MimeType = Path.GetExtension(file.FileName);
                    model.UserId = userId;
                    model.FilePath = Savepath;
                    await _commonService.UpdateContractFileDetails(model);
                    clients.ClientsDetailsList = await _adminService.GetClients(0, 0);
                    clients.ClientsEmployeeList = new List<ClientsEmployeeList>();
                    clients.AccountExecutiveList = await _adminService.GetAccountExecutiveList();
                    clients.ResidencyCodeList = await _adminService.GetResidencyCodeList();
                    clients.StatesList = new List<StatesList>();
                    clients.CitiesList = await _adminService.GetCitiesList();
                    clients.ClientsInsuranceContractList = new List<ClientsInsuranceContractList>();
                    clients.AnnualChargeRateList = new List<AnnualChargeRateList>();
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_ClientsList", clients.ClientsDetailsList);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveClientContractOtheFiles(IFormFile file, int ClientId, int ContractId)
        {
            string retMessage = string.Empty;
            var userId = HttpContext.Session.GetString("UserID");
            Clients clients = new Clients();
            try
            {
                clients.ClientsDetailsList = await _adminService.GetClients(ClientId, 0);
                ContractFileDetails model = new ContractFileDetails();
                if (file != null)
                {
                    var clientdata = clients.ClientsDetailsList.FirstOrDefault();
                    string FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Direc_FileName = clientdata.CompanyId + "\\OtherFiles\\" + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Savepath = Path.Combine(StorageRoot, Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "\\" + clientdata.CompanyId);

                    if (!Directory.Exists(fullPaths))
                        Directory.CreateDirectory(fullPaths);

                    if (!Directory.Exists(fullPaths + "\\OtherFiles"))
                        Directory.CreateDirectory(fullPaths + "\\OtherFiles");

                    if (Directory.Exists(fullPaths))
                    {
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
                    await _commonService.UpdateClientContractOtheFiles(model);
                    clients.ClientsDetailsList = await _adminService.GetClients(0, 0);
                    clients.ClientsEmployeeList = new List<ClientsEmployeeList>();
                    clients.AccountExecutiveList = await _adminService.GetAccountExecutiveList();
                    clients.ResidencyCodeList = await _adminService.GetResidencyCodeList();
                    clients.StatesList = new List<StatesList>();
                    clients.CitiesList = await _adminService.GetCitiesList();
                    clients.ClientsInsuranceContractList = new List<ClientsInsuranceContractList>();
                    clients.AnnualChargeRateList = new List<AnnualChargeRateList>();
                }
                retMessage = "success";
            }
            catch (Exception ex)
            {
                retMessage = "fail";
            }
            return Json(new { retMessage });
            //  return PartialView("_ClientsList", model.ClientsDetailsList);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveClientContractPassThruFiles(IFormFile file, int ClientId, int ContractId)
        {
            string retMessage = string.Empty;
            var userId = HttpContext.Session.GetString("UserID");
            Clients clients = new Clients();
            try
            {
                clients.ClientsDetailsList = await _adminService.GetClients(ClientId, 0);
                ContractFileDetails model = new ContractFileDetails();
                if (file != null)
                {
                    var clientdata = clients.ClientsDetailsList.FirstOrDefault();
                    string FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Direc_FileName = clientdata.CompanyId + "\\PassThruFiles\\" + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Savepath = Path.Combine(StorageRoot, Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "\\" + clientdata.CompanyId);

                    if (!Directory.Exists(fullPaths))
                        Directory.CreateDirectory(fullPaths);

                    if (!Directory.Exists(fullPaths + "\\PassThruFiles"))
                        Directory.CreateDirectory(fullPaths + "\\PassThruFiles");

                    if (Directory.Exists(fullPaths))
                    {
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
                    await _commonService.UpdateClientContractPassThruFiles(model);
                    clients.ClientsDetailsList = await _adminService.GetClients(0, 0);
                    clients.ClientsEmployeeList = new List<ClientsEmployeeList>();
                    clients.AccountExecutiveList = await _adminService.GetAccountExecutiveList();
                    clients.ResidencyCodeList = await _adminService.GetResidencyCodeList();
                    clients.StatesList = new List<StatesList>();
                    clients.CitiesList = await _adminService.GetCitiesList();
                    clients.ClientsInsuranceContractList = new List<ClientsInsuranceContractList>();
                    clients.AnnualChargeRateList = new List<AnnualChargeRateList>();
                }
                retMessage = "success";
            }
            catch (Exception ex)
            {
                retMessage = "fail";
            }
            return Json(new { retMessage });
            //  return PartialView("_ClientsList", model.ClientsDetailsList);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveClientContractCrewLogFiles(IFormFile file, int ClientId, int ContractId)
        {
            string retMessage = string.Empty;
            var userId = HttpContext.Session.GetString("UserID");
            Clients clients = new Clients();
            try
            {
                clients.ClientsDetailsList = await _adminService.GetClients(ClientId, 0);
                ContractFileDetails model = new ContractFileDetails();
                if (file != null)
                {
                    var clientdata = clients.ClientsDetailsList.FirstOrDefault();
                    string FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Direc_FileName = clientdata.CompanyId + "\\CrewLogFiles\\" + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Savepath = Path.Combine(StorageRoot, Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "\\" + clientdata.CompanyId);

                    if (!Directory.Exists(fullPaths))
                        Directory.CreateDirectory(fullPaths);

                    if (!Directory.Exists(fullPaths + "\\CrewLogFiles"))
                        Directory.CreateDirectory(fullPaths + "\\CrewLogFiles");

                    if (Directory.Exists(fullPaths))
                    {
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
                    await _commonService.UpdateClientContractCrewLogFiles(model);
                    clients.ClientsDetailsList = await _adminService.GetClients(0, 0);
                    clients.ClientsEmployeeList = new List<ClientsEmployeeList>();
                    clients.AccountExecutiveList = await _adminService.GetAccountExecutiveList();
                    clients.ResidencyCodeList = await _adminService.GetResidencyCodeList();
                    clients.StatesList = new List<StatesList>();
                    clients.CitiesList = await _adminService.GetCitiesList();
                    clients.ClientsInsuranceContractList = new List<ClientsInsuranceContractList>();
                    clients.AnnualChargeRateList = new List<AnnualChargeRateList>();
                }
                retMessage = "success";
            }
            catch (Exception ex)
            {
                retMessage = "fail";
            }
            return Json(new { retMessage });
            //  return PartialView("_ClientsList", model.ClientsDetailsList);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddClientsEmployee([FromBody] ClientsEmployeeDetails employeeDetails)
        {
            Clients model = new Clients();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");
                await _menuUtils.SetMenu(HttpContext.Session);
                model.ClientsEmployeeList = await _adminService.AddClientEmployeeDetails(employeeDetails, userId);
                model.ClientsDetailsList = await _adminService.GetClients(0, 0);
                model.ReferenceId = model.ClientsEmployeeList.Select(df => df.ReferenceID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return PartialView("_ClientEmployeeList", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditClients([FromBody] int AgreementID)
        {
            Clients model = new Clients();
            try
            {
                model.ClientsDetailsList = await _adminService.GetClients(AgreementID, 0);
                model.EmployeePositionList = await _adminService.GetEmployeePositionList();
                model.ContractAgreementTypeList = await _commonService.GetContractAgreementType();
                model.AccountExecutiveList = await _adminService.GetAccountExecutiveList();
                model.ResidencyCodeList = await _adminService.GetResidencyCodeList();
                //model.StatesList = new List<StatesList>();
                //model.CitiesList = await _adminService.GetCitiesList();
                model.CitiesList = new List<CitiesList>();
                model.StatesList = await _adminService.GetStatesList();
                model.RoleAccess = HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "");


                model.InsuranceContractList = await _adminService.GetInsuranceContract(0);

                ClientsDetails res = new ClientsDetails();
                if (model.ClientsDetailsList.Count > 0)
                {
                    foreach (var item in model.ClientsDetailsList)
                    {
                        if (item.ID == AgreementID)
                        {
                            res.ID = item.ID;
                            res.CompanyId = item.CompanyId;
                            res.CompanyName = item.CompanyName.ToString();
                            res.ContractSignedDate = item.ContractStartDate;
                            res.ContractEffDate = item.ContractRenewedDate.ToString();
                            res.ContractExpiryDate = item.ContractExpiryDate.ToString();
                            res.AccountExecutiveID = item.AccountExecutiveID;
                            res.AgreementTypeId = item.AgreementTypeId;
                            res.ContractLength = item.ContractLength;
                            res.Amount = item.Amount;
                            res.Transport = item.Transport;
                            res.LocationStreetAddress = item.LocationStreetAddress;
                            res.CityId = item.CityId;
                            res.StateId = item.StateId;
                            res.County = item.County;
                            res.ZipCode = item.ZipCode;
                            res.BusinessEndDate = item.BusinessEndDate;
                            res.ReferenceID = item.ReferenceID;
                            res.Active = item.Active;
                            res.Status = item.Status;
                            res.TransportCharges = item.TransportCharges;
                            res.ResidencyCodeID = item.ResidencyCodeID;
                            res.Year1 = item.Year1;
                            res.Year2 = item.Year2;
                            res.Year3 = item.Year3;
                            res.Year4 = item.Year4;
                            res.FeesChangedDate = item.FeesChangedDate;
                            res.InsuranceContractRefID = item.InsuranceContractRefID;

                            res.BillingType = item.BillingType;

                            model.ClientsEmployeeList = await _adminService.GetClientEmployeeList(item.ReferenceID);
                            model.ClientsInsuranceContractList = await _adminService.GetClientInsuranceContractList(item.InsuranceContractRefID);
                            model.AnnualChargeRateList = await _adminService.GetAnnualChargeRateList(item.CompanyId);

                        }
                    }
                    model.ClientsDetails = res;
                }

            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_ClientsForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteClients(int ClientID)
        {
            Clients model = new Clients();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");

                await _adminService.DeleteClients(ClientID, userId);
                model.ClientsDetailsList = await _adminService.GetClients(ClientID, 0);
            }
            catch (Exception Ex)
            {

            }
            return PartialView("_ClientsList", model.ClientsDetailsList);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetState(int CityId)
        {
            Clients model = new Clients();
            try
            {
                model.StatesList = await _adminService.GetStateList(CityId);
            }
            catch (Exception Ex)
            {

            }
            return Json(model.StatesList);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetCity(int StateId)
        {
            Clients model = new Clients();
            try
            {
                model.CitiesList = await _adminService.GetCityList(StateId);
            }
            catch (Exception Ex)
            {

            }
            return Json(model.CitiesList);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteClientEmployee(int EmployeeID, int RefID)
        {
            Clients model = new Clients();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeleteClientEmployee(EmployeeID, UserId);
                model.ClientsEmployeeList = await _adminService.GetClientEmployeeList(RefID);
            }
            catch (Exception Ex)
            {

            }
            return PartialView("_ClientEmployeeList", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ClientEmployeeEdit(int EmployeeID, int RefID)
        {
            Clients model = new Clients();
            try
            {
                model.ClientsEmployeeList = await _adminService.GetClientEmployeeList(RefID);
                model.EmployeePositionList = await _adminService.GetEmployeePositionList();
                if (model.ClientsEmployeeList.Count > 0)
                {
                    ClientsDetails elientEmp = new ClientsDetails();
                    foreach (var item in model.ClientsEmployeeList)
                    {
                        if (item.ID == EmployeeID)
                        {
                            elientEmp.EmpID = item.ID;
                            elientEmp.EmployeePosition = item.PositionId;
                            elientEmp.ReferenceID = item.ReferenceID;
                            elientEmp.FirstName = item.FirstName;
                            elientEmp.LastName = item.LastName;
                            elientEmp.EmailId = item.EmailId;
                        }
                    }
                    model.ClientsDetails = elientEmp;
                }
            }
            catch (Exception Ex)
            {

            }
            return PartialView("_ClientEmployeeDetails", model);
        }
        [HttpPost]
        public async Task<IActionResult> DownloadClients(int ClientStatusID=0, int ERID=0)
        {
            List<DownloadClientsDetailsList> model = new List<DownloadClientsDetailsList>();
            string UserId = HttpContext.Session.GetString("UserID");
            model = await _adminService.ExportClients(ClientStatusID, ERID);
            var data = model;
            string sFileName = @"Clients.xlsx";
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
        [AllowAnonymous]
        public async Task<IActionResult> ClientBulkUpload([FromBody] Clients clients)
        {
            string retMessage = string.Empty;
            try
            {
                Clients Bulkclients = new Clients();
                string UserID = HttpContext.Session.GetString("UserID");
                Bulkclients.BulkClientsDetailsList = clients.BulkClientsDetailsList;
                await _adminService.UpdateBulkDetails(Bulkclients, UserID);
                CreateContractFolders(clients.BulkClientsDetailsList);
                retMessage = "success";
            }
            catch (Exception ex)
            {
                retMessage = "fail";
            }
            return Json(retMessage);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ClientBulkUploadValidation(IFormFile file, int Type)
        {
            Clients Bulkclients = new Clients();
            List<BulkClientsDetailsValidationList> clientsValidationList = new List<BulkClientsDetailsValidationList>();
            try
            {
                if (file != null)
                {
                    string FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Direc_FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    var rootpath = Path.Combine(ClientsStorageRoot, Direc_FileName);
                    string UserID = HttpContext.Session.GetString("UserID");
                    if (Direc_FileName.EndsWith(".xlsx"))
                    {
                        using (var stream = new FileStream(rootpath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        FileInfo FileNamePath = new FileInfo(rootpath);
                        using (ExcelPackage xlPackage = new ExcelPackage(FileNamePath))
                        {
                            if (xlPackage.Workbook.Worksheets["ClientMaster"] != null)
                            {
                                ExcelWorksheet workSheet = xlPackage.Workbook.Worksheets["ClientMaster"];
                                var Start = workSheet.Dimension.Start;
                                var End = workSheet.Dimension.End;

                                var Header = workSheet.Dimension.Start.Row;
                                int ColumnCount = workSheet.Dimension.End.Column;
                                if (ColumnCount > 23 || ColumnCount < 23)
                                {
                                    return Json(new { message = "ColumnCountFailed" });
                                }
                                else if (ColumnCount == 23)
                                {
                                    List<BulkClientsDetails> clientsList = new List<BulkClientsDetails>();

                                    for (int row = Start.Row; row <= End.Row; row++)
                                    {
                                        if (row >= 2)
                                        {
                                            BulkClientsDetails clients = new BulkClientsDetails();
                                            clients.Client = workSheet.Cells[row, 1].Text.Trim().Replace(",", "''");
                                            clients.Description = workSheet.Cells[row, 2].Text.Trim().Replace(",", "''");
                                            clients.ResidencyCode = workSheet.Cells[row, 3].Text.Trim().Replace(",", "''");
                                            clients.Fee1 = workSheet.Cells[row, 4].Text.Trim().Replace(",", "''");
                                            clients.Fee2 = workSheet.Cells[row, 5].Text.Trim().Replace(",", "''");
                                            clients.Fee3 = workSheet.Cells[row, 6].Text.Trim().Replace(",", "''");
                                            clients.Fee4 = workSheet.Cells[row, 7].Text.Trim().Replace(",", "''");
                                            clients.ContractStartSate = workSheet.Cells[row, 8].Text.Trim().Replace(",", "''");
                                            clients.ContractRenewed = workSheet.Cells[row, 9].Text.Trim().Replace(",", "''");
                                            clients.ContractExpires = workSheet.Cells[row, 10].Text.Trim().Replace(",", "''");
                                            clients.AccountOwner = workSheet.Cells[row, 11].Text.Trim().Replace(",", "''");
                                            clients.Chief = workSheet.Cells[row, 12].Text.Trim().Replace(",", "''");
                                            clients.Cemail = workSheet.Cells[row, 13].Text.Trim().Replace(",", "''");
                                            clients.Fiscal = workSheet.Cells[row, 14].Text.Trim().Replace(",", "''");
                                            clients.Femail = workSheet.Cells[row, 15].Text.Trim().Replace(",", "''");
                                            clients.PhysicalLocationStreet = workSheet.Cells[row, 16].Text.Trim().Replace(",", "''");
                                            clients.PhysicalLocationCity = workSheet.Cells[row, 17].Text.Trim().Replace(",", "''");
                                            clients.PhysicalLocationState = workSheet.Cells[row, 18].Text.Trim().Replace(",", "''");
                                            clients.PhysicalLocationZip = workSheet.Cells[row, 19].Text.Trim().Replace(",", "''");
                                            clients.NonTransportBilled = workSheet.Cells[row, 20].Text.Trim().Replace(",", "''");
                                            clients.NonTransportCharges = workSheet.Cells[row, 21].Text.Trim().Replace(",", "''");
                                            clients.ContractFileName = workSheet.Cells[row, 22].Text.Trim().Replace(",", "''");
                                            clients.Notes = workSheet.Cells[row, 23].Text.Trim().Replace(",", "''");

                                            clients.ContractFilePath = Path.Combine(StorageRoot, clients.Client + "_" + clients.Description, clients.ContractFileName);
                                            if (clients.ContractFileName != "")
                                                clients.ContractFileMimeType = Path.GetExtension(clients.ContractFileName);
                                            else
                                                clients.ContractFileMimeType = "";

                                            if (clients.Client != "" && clients.Description != "")
                                                clientsList.Add(clients);
                                        }
                                    }
                                    Bulkclients.BulkClientsDetailsList = clientsList;
                                    clientsValidationList = await _adminService.UpdateBulkClientsValidation(Bulkclients, UserID);

                                    //CreateContractFolders(clientsList);
                                }
                            }
                            else
                            {
                                return Json(new { message = "HeadeFormatFailed" });
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return PartialView("_BulkClientsList", clientsValidationList);
        }

        public void CreateContractFolders(List<BulkClientsDetails> clientsList)
        {
            if (clientsList.Count > 0)
            {
                foreach (var clientdata in clientsList)
                {
                    string fullPath = Path.Combine(StorageRoot + "\\" + clientdata.Client);
                    //string fullPath = Path.Combine(StorageRoot + "\\" + clientdata.Client + "_" + clientdata.Description);

                    if (!Directory.Exists(fullPath))
                        Directory.CreateDirectory(fullPath);
                }

            }
        }

        public ActionResult Change_Password()
        {
            return View();
        }


        public async Task<ActionResult> MenuAccess()
        {
            //Authorization
            if (!_authorizationService.AuthorizeAsync(User, this.ControllerContext, "MenuAccessPolicy").Result.Succeeded)
                return RedirectToAction("AccessDenied", "Home");
            //To set menu
            await _menuUtils.SetMenu(HttpContext.Session);

            //To Fill the Roles Dropdown
            Users users = new Users();
            users.RolesList = GetRoles();
            return View("MenuAccess", users);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RoleModules([FromBody] RoleModules roleModules)
        {
            await _adminService.InsertMenuAccess(roleModules);
            return Json("success");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GetMenusDetails([FromBody] RoleModules roleModules)
        {
            var nodes = new List<MenuItem>();
            Menu Menuobj = new Menu();
            try
            {
                Menuobj.MenuList = await _adminService.GetMenuList(roleModules.RoleId);
                //Menuobj.MenuList = lstitem;

                for (int i = 0; i < Menuobj.MenuList.Count; i++)
                {
                    Menuobj.MenuList[i].state = new state();
                    if (Menuobj.MenuList[i].parent == "#" && Menuobj.MenuList[i].selected == true)
                        Menuobj.MenuList[i].state.opened = true;
                    else
                        Menuobj.MenuList[i].state.selected = Menuobj.MenuList[i].selected;
                }

            }
            catch (Exception ex)
            {

            }
            return Json(Menuobj.MenuList.ToList());

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUsers([FromBody] Users users)
        {
            Users ValidUsers = new Users();
            List<UserListInfo> userlistinfo = new List<UserListInfo>();
            var userId = HttpContext.Session.GetString("UserID");
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                //Find the User Role based on ID
                var user = await _userManager.FindByIdAsync(users.Id);
                var rolesForUser = await _userManager.GetRolesAsync(user);
                //Remove UserDetails from Database
                await _adminService.DeleteUsers(users.Id);
                //  await _userManager.DeleteAsync(user);

            }
            catch (Exception Ex)
            {

            }
            ValidUsers.UserList = await _adminService.GetUserList(userId);
            return PartialView("_UsersList", ValidUsers);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ViewClients(int AgreementID)
        {
            ClientsViewDetails model = new ClientsViewDetails();
            try
            {
                model = await _adminService.ViewClients(AgreementID, 0);
            }
            catch (Exception ex)
            {

            }
            return PartialView("_ClientsView", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertContractType([FromBody] ContractStatusDet model)
        {
            ContractStatusDetails ContractStatus = new ContractStatusDetails();
            model.CreatedOn = DateTime.Now;
            model.ModifiedOn = DateTime.Now;
            model.CreatedBy = HttpContext.Session.GetString("UserID");
            ContractStatus.ContractStatusDet = model;
            if (model.Id == 0)
            {
                await _adminService.InsertContractType(model, 0);
            }
            else
            {
                model.ModifiedOn = DateTime.Now;
                model.ModifiedBy = HttpContext.Session.GetString("UserID");
                await _adminService.InsertContractType(model, model.Id);
            }
            ContractStatus.ContractStatusList = await _adminService.GetContractTypeDetails(0);
            return PartialView("_ContractStatusList", ContractStatus);
        }
        [HttpGet]
        public async Task<IActionResult> ContractStatus()
        {
            ContractStatusDetails ContractStatus = new ContractStatusDetails();
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                ContractStatus.ContractStatusList = await _adminService.GetContractTypeDetails(0);
            }
            catch (Exception ex) { }
            return View("ContractStatus", ContractStatus);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditContractType([FromBody] int ID)
        {
            String status = null;
            ContractStatusDetails model = new ContractStatusDetails();
            try
            {
                model.ContractStatusList = await _adminService.GetContractTypeDetails(ID);
                {
                    ContractStatusDet res = new ContractStatusDet();
                    if (model.ContractStatusList.Count > 0)
                    {
                        foreach (var item in model.ContractStatusList)
                        {
                            if (item.Id == ID)
                            {
                                res.Id = item.Id;
                                res.ContractStatus = item.ContractStatus.ToString();
                                status = item.Active;
                                if (status.ToLower() == "yes")
                                {
                                    res.Active = 1;
                                }
                                else { res.Active = 0; }
                            }
                        }
                        model.ContractStatusDet = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_ContractStatusForm", model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteContractStatus(int ID)
        {
            ContractStatusDetails model = new ContractStatusDetails();
            try
            {
                await _adminService.DeleteContractDetails(ID);
                model.ContractStatusList = await _adminService.GetContractTypeDetails(0);
            }
            catch (Exception ex) { }
            return PartialView("_ContractStatusList", model);
        }
        [HttpPost]
        public async Task<JsonResult> IsspanCompanyIdExist(int CID)
        {
            try
            {
                int value = CID;
                Clients clients = new Clients();
                List<ClientsCompanyDetails> check = new List<ClientsCompanyDetails>();
                check = await _adminService.GetCompanyList(CID);
                if (check.Count != 0)
                {
                    return Json(new { message = "Failed" });
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }
        [HttpPost]
        public async Task<JsonResult> IsContractStatusExist(String ContractStatus)
        {
            try
            {
                String value = ContractStatus;
                List<ContractStatusList> check = new List<ContractStatusList>();
                check = await _adminService.GetContractList(ContractStatus);
                if (check.Count != 0)
                {
                    return Json(new { message = "Failed" });
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }

        [HttpGet]
        public async Task<PartialViewResult> FilterClients(int ClientStatusID, int ERID)
        {
            var clients = await _adminService.GetFilterClients(0, 0, ClientStatusID, ERID);
            return PartialView("_ClientsList", clients);
        }

        #region Insurance Contract
        [HttpGet]
        public async Task<IActionResult> InsuranceContract()
        {
            InsuranceContract objInsuranceContract = new InsuranceContract();
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                objInsuranceContract.InsuranceContractList = await _adminService.GetInsuranceContract(0);
                objInsuranceContract.InsuranceContractDet = new InsuranceContractDet();
            }
            catch (Exception ex) { }
            return View("InsuranceContract", objInsuranceContract);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditInsuranceContract([FromBody] int ID)
        {
            String status = null;
            InsuranceContract model = new InsuranceContract();
            try
            {
                model.InsuranceContractList = await _adminService.GetInsuranceContract(ID);
                {
                    InsuranceContractDet res = new InsuranceContractDet();
                    if (model.InsuranceContractList.Count > 0)
                    {
                        foreach (var item in model.InsuranceContractList)
                        {
                            if (item.Id == ID)
                            {
                                res.Id = item.Id;
                                res.Name = item.Name.ToString();
                                status = item.Active;
                                if (status.ToLower() == "yes")
                                {
                                    res.Active = true;
                                }
                                else { res.Active = false; }
                            }
                        }
                        model.InsuranceContractDet = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_InsuranceContractForm", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertInsuranceContract([FromBody] InsuranceContractDet model)
        {
            InsuranceContract objInsuranceContract = new InsuranceContract();
            model.CreatedBy = HttpContext.Session.GetString("UserID");
            objInsuranceContract.InsuranceContractDet = model;
            if (model.Id == 0)
            {
                await _adminService.InsertInsuranceContract(model.Id, model.Name, model.Active, model.CreatedBy);
            }
            else
            {
                await _adminService.InsertInsuranceContract(model.Id, model.Name, model.Active, model.CreatedBy);
            }
            objInsuranceContract.InsuranceContractList = await _adminService.GetInsuranceContract(0);
            return PartialView("_InsuranceContractList", objInsuranceContract);
        }

        [HttpPost]
        public async Task<JsonResult> IsInsuranceContractExist(String Name)
        {
            try
            {
                String value = Name;
                List<InsuranceContractList> check = new List<InsuranceContractList>();
                check = await _adminService.GetInsuranceContractListByName(Name);
                if (check.Count != 0)
                {
                    return Json(new { message = "Failed" });
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteInsuranceContract(int ID)
        {
            InsuranceContract model = new InsuranceContract();
            try
            {
                await _adminService.DeleteInsuranceContractDetails(ID);
                model.InsuranceContractList = await _adminService.GetInsuranceContract(0);
            }
            catch (Exception ex) { }
            return PartialView("_InsuranceContractList", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteClientInsuranceContract(int ID, int RefID)
        {
            Clients model = new Clients();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeleteClientInsuranceContract(ID, UserId);
                model.ClientsInsuranceContractList = await _adminService.GetClientInsuranceContractList(RefID);
            }
            catch (Exception Ex)
            {

            }
            return PartialView("_ClientsInsuranceContractList", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ClientInsuranceContractEdit(int ID, int RefID)
        {
            Clients model = new Clients();
            try
            {
                model.ClientsInsuranceContractList = await _adminService.GetClientInsuranceContractList(RefID);
                model.InsuranceContractList = await _adminService.GetInsuranceContract(0);
                if (model.ClientsInsuranceContractList.Count > 0)
                {
                    ClientsDetails clientsDetails = new ClientsDetails();
                    foreach (var item in model.ClientsInsuranceContractList)
                    {
                        if (item.ID == ID)
                        {
                            clientsDetails.InsuranceID = item.ID;
                            clientsDetails.DocumentType = item.DocumentType;
                            clientsDetails.InsuranceContractID = item.InsuranceContractID;
                            clientsDetails.InsuranceContractRefID = item.RefID;
                            clientsDetails.EffectiveDate = item.EffectiveDate;
                            clientsDetails.TermedDate = item.TermedDate;
                        }
                    }
                    model.ClientsDetails = clientsDetails;
                }
            }
            catch (Exception Ex)
            {

            }
            return PartialView("_ClientsInsuranceContractDetails", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddClientsInsuranceContract([FromBody] ClientsInsuranceContractDetails insuranceContractDetails)
        {
            int id = 0;
            Clients model = new Clients();
            InsuranceContract objInsuranceContract = new InsuranceContract();
            try
            {
                var userId = HttpContext.Session.GetString("UserID");
                await _menuUtils.SetMenu(HttpContext.Session);
                model.ClientsInsuranceContractList = await _adminService.AddClientsInsuranceContract(insuranceContractDetails, userId);
                model.ClientsDetailsList = await _adminService.GetClients(0, 0);
                model.InsuranceContractRefID = model.ClientsInsuranceContractList.Select(df => df.RefID).FirstOrDefault();
                id = model.ClientsInsuranceContractList.Select(df => df.ID).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return Json(new { InsuranceContractId = id, InsuranceContractRefId = model.InsuranceContractRefID });
            //return PartialView("_ClientsInsuranceContractList", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveInsuranceContractFiles(IFormFile file, int InsuranceContractRefId, int InsuranceContractId)
        {
            var userId = HttpContext.Session.GetString("UserID");
            Clients clients = new Clients();
            try
            {
                //clients.ClientContractDetails = await _adminService.GetClientContractList(ClientId, 0);
                ContractFileDetails model = new ContractFileDetails();
                if (file != null)
                {
                    //var clientdata = clients.ClientsDetailsList.FirstOrDefault();
                    string FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Direc_FileName = "InsuranceContract\\" + DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;

                    string Savepath = Path.Combine(StorageRoot, Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "InsuranceContract\\");

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

                    await _adminService.UpdateClientInsuranceContractFileDetails(InsuranceContractId, FileName, Savepath, file.FileName, userId);
                    clients.ClientsDetailsList = await _adminService.GetClients(0, 0);
                    clients.ClientsEmployeeList = new List<ClientsEmployeeList>();
                    clients.AccountExecutiveList = await _adminService.GetAccountExecutiveList();
                    clients.ResidencyCodeList = await _adminService.GetResidencyCodeList();
                    clients.StatesList = new List<StatesList>();
                    clients.CitiesList = await _adminService.GetCitiesList();
                    clients.InsuranceContractList = await _adminService.GetInsuranceContract(0);

                    clients.ClientsInsuranceContractList = await _adminService.GetClientInsuranceContractList(InsuranceContractRefId);
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_ClientsInsuranceContractList", clients);
        }
        #endregion

        #region Annual Charge Rate

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAnnualChargeRate(int ID, string CompanyID)
        {
            Clients model = new Clients();
            try
            {
                string UserId = HttpContext.Session.GetString("UserID");
                await _adminService.DeleteAnnualChargeRate(ID, UserId);
                model.AnnualChargeRateList = await _adminService.GetAnnualChargeRateList(CompanyID);
            }
            catch (Exception Ex)
            {

            }
            return PartialView("_AnnualChargeRateList", model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveAnnualChargeRate(IFormFile file, string CompanyID)
        {
            var userId = HttpContext.Session.GetString("UserID");
            Clients clients = new Clients();
            try
            {
                //clients.ClientContractDetails = await _adminService.GetClientContractList(ClientId, 0);
                ContractFileDetails model = new ContractFileDetails();
                if (file != null)
                {
                    //var clientdata = clients.ClientsDetailsList.FirstOrDefault();
                    string FileName = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss").Replace(@"-", "").Replace(@":", "").Replace(@" ", "") + "_" + file.FileName;
                    string Direc_FileName = "AnnualChargeRate\\" + FileName;

                    string Savepath = Path.Combine(StorageRoot, Direc_FileName);
                    string fullPaths = Path.Combine(StorageRoot + "AnnualChargeRate\\");

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

                    await _adminService.InsertAnnualChargeRate(FileName, CompanyID, userId);
                    clients.ClientsDetailsList = await _adminService.GetClients(0, 0);
                    clients.ClientsEmployeeList = new List<ClientsEmployeeList>();
                    clients.AccountExecutiveList = await _adminService.GetAccountExecutiveList();
                    clients.ResidencyCodeList = await _adminService.GetResidencyCodeList();
                    clients.StatesList = new List<StatesList>();
                    clients.CitiesList = await _adminService.GetCitiesList();
                    clients.InsuranceContractList = await _adminService.GetInsuranceContract(0);

                    clients.ClientsInsuranceContractList = new List<ClientsInsuranceContractList>(); //await _adminService.GetClientInsuranceContractList(0);

                    clients.AnnualChargeRateList = await _adminService.GetAnnualChargeRateList(CompanyID);
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_AnnualChargeRateList", clients);
        }
        #endregion

        #region AddressType
        [HttpGet]
        public async Task<IActionResult> AddressType()
        {
            AddressType objAddressType = new AddressType();
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                objAddressType.AddressTypeList = await _adminService.GetAddressType(0);
                objAddressType.AddressTypeDet = new AddressTypeDet();
            }
            catch (Exception ex) { }
            return View("AddressType", objAddressType);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditAddressType([FromBody] int ID)
        {
            String status = null;
            AddressType model = new AddressType();
            try
            {
                model.AddressTypeList = await _adminService.GetAddressType(ID);
                {
                    AddressTypeDet res = new AddressTypeDet();
                    if (model.AddressTypeList.Count > 0)
                    {
                        foreach (var item in model.AddressTypeList)
                        {
                            if (item.Id == ID)
                            {
                                res.Id = item.Id;
                                res.Name = item.Name.ToString();
                                status = item.Active;
                                if (status.ToLower() == "yes")
                                {
                                    res.Active = true;
                                }
                                else { res.Active = false; }
                            }
                        }
                        model.AddressTypeDet = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_AddressTypeForm", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertAddressType([FromBody] AddressTypeDet model)
        {
            AddressType objAddressType = new AddressType();
            model.CreatedBy = HttpContext.Session.GetString("UserID");
            objAddressType.AddressTypeDet = model;
            //if (model.Id == 0)
            //{
            //    await _adminService.InsertAddressType(model.Id, model.Name, model.Active, model.CreatedBy);
            //}
            //else
            //{
            await _adminService.InsertAddressType(model.Id, model.Name, model.Active, model.CreatedBy);
            //}
            objAddressType.AddressTypeList = await _adminService.GetAddressType(0);
            return PartialView("_AddressTypeList", objAddressType);
        }

        [HttpPost]
        public async Task<JsonResult> IsAddressTypeExist(String Name)
        {
            try
            {
                String value = Name;
                List<AddressTypeList> check = new List<AddressTypeList>();
                check = await _adminService.GetAddressTypeListByName(Name);
                if (check.Count != 0)
                {
                    return Json(new { message = "Failed" });
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAddressType(int ID)
        {
            AddressType model = new AddressType();
            try
            {
                await _adminService.DeleteAddressTypeDetails(ID);
                model.AddressTypeList = await _adminService.GetAddressType(0);
            }
            catch (Exception ex) { }
            return PartialView("_AddressTypeList", model);
        }
        #endregion

        #region ContactType
        [HttpGet]
        public async Task<IActionResult> ContactType()
        {
            ContactType objContactType = new ContactType();
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                objContactType.ContactTypeList = await _adminService.GetContactType(0);
                objContactType.ContactTypeDet = new ContactTypeDet();
            }
            catch (Exception ex) { }
            return View("ContactType", objContactType);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditContactType([FromBody] int ID)
        {
            String status = null;
            ContactType model = new ContactType();
            try
            {
                model.ContactTypeList = await _adminService.GetContactType(ID);
                {
                    ContactTypeDet res = new ContactTypeDet();
                    if (model.ContactTypeList.Count > 0)
                    {
                        foreach (var item in model.ContactTypeList)
                        {
                            if (item.Id == ID)
                            {
                                res.Id = item.Id;
                                res.Name = item.Name.ToString();
                                status = item.Active;
                                if (status.ToLower() == "yes")
                                {
                                    res.Active = true;
                                }
                                else { res.Active = false; }
                            }
                        }
                        model.ContactTypeDet = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_ContactTypeForm", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertContactType([FromBody] ContactTypeDet model)
        {
            ContactType objContactType = new ContactType();
            model.CreatedBy = HttpContext.Session.GetString("UserID");
            objContactType.ContactTypeDet = model;
            //if (model.Id == 0)
            //{
            //    await _adminService.InsertContactType(model.Id, model.Name, model.Active, model.CreatedBy);
            //}
            //else
            //{
            await _adminService.InsertContactType(model.Id, model.Name, model.Active, model.CreatedBy);
            //}
            objContactType.ContactTypeList = await _adminService.GetContactType(0);
            return PartialView("_ContactTypeList", objContactType);
        }

        [HttpPost]
        public async Task<JsonResult> IsContactTypeExist(String Name)
        {
            try
            {
                String value = Name;
                List<ContactTypeList> check = new List<ContactTypeList>();
                check = await _adminService.GetContactTypeListByName(Name);
                if (check.Count != 0)
                {
                    return Json(new { message = "Failed" });
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteContactType(int ID)
        {
            ContactType model = new ContactType();
            try
            {
                await _adminService.DeleteContactTypeDetails(ID);
                model.ContactTypeList = await _adminService.GetContactType(0);
            }
            catch (Exception ex) { }
            return PartialView("_ContactTypeList", model);
        }
        #endregion

        #region DocumentTitle
        [HttpGet]
        public async Task<IActionResult> DocumentTitle()
        {
            DocumentTitle objDocumentTitle = new DocumentTitle();
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                objDocumentTitle.DocumentTitleList = await _adminService.GetDocumentTitle(0);
                objDocumentTitle.DocumentTitleDet = new DocumentTitleDet();
            }
            catch (Exception ex) { }
            return View("DocumentTitle", objDocumentTitle);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditDocumentTitle([FromBody] int ID)
        {
            String status = null;
            DocumentTitle model = new DocumentTitle();
            try
            {
                model.DocumentTitleList = await _adminService.GetDocumentTitle(ID);
                {
                    DocumentTitleDet res = new DocumentTitleDet();
                    if (model.DocumentTitleList.Count > 0)
                    {
                        foreach (var item in model.DocumentTitleList)
                        {
                            if (item.Id == ID)
                            {
                                res.Id = item.Id;
                                res.Name = item.Name.ToString();
                                status = item.Active;
                                if (status.ToLower() == "yes")
                                {
                                    res.Active = true;
                                }
                                else { res.Active = false; }
                            }
                        }
                        model.DocumentTitleDet = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_DocumentTitleForm", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertDocumentTitle([FromBody] DocumentTitleDet model)
        {
            DocumentTitle objDocumentTitle = new DocumentTitle();
            model.CreatedBy = HttpContext.Session.GetString("UserID");
            objDocumentTitle.DocumentTitleDet = model;
            //if (model.Id == 0)
            //{
            //    await _adminService.InsertDocumentTitle(model.Id, model.Name, model.Active, model.CreatedBy);
            //}
            //else
            //{
            await _adminService.InsertDocumentTitle(model.Id, model.Name, model.Active, model.CreatedBy);
            //}
            objDocumentTitle.DocumentTitleList = await _adminService.GetDocumentTitle(0);
            return PartialView("_DocumentTitleList", objDocumentTitle);
        }

        [HttpPost]
        public async Task<JsonResult> IsDocumentTitleExist(String Name)
        {
            try
            {
                String value = Name;
                List<DocumentTitleList> check = new List<DocumentTitleList>();
                check = await _adminService.GetDocumentTitleListByName(Name);
                if (check.Count != 0)
                {
                    return Json(new { message = "Failed" });
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteDocumentTitle(int ID)
        {
            DocumentTitle model = new DocumentTitle();
            try
            {
                await _adminService.DeleteDocumentTitleDetails(ID);
                model.DocumentTitleList = await _adminService.GetDocumentTitle(0);
            }
            catch (Exception ex) { }
            return PartialView("_DocumentTitleList", model);
        }
        #endregion

        #region LicenseType
        [HttpGet]
        public async Task<IActionResult> LicenseType()
        {
            LicenseType objLicenseType = new LicenseType();
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                objLicenseType.LicenseTypeList = await _adminService.GetLicenseType(0);
                objLicenseType.LicenseTypeDet = new LicenseTypeDet();
            }
            catch (Exception ex) { }
            return View("LicenseType", objLicenseType);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditLicenseType([FromBody] int ID)
        {
            String status = null;
            LicenseType model = new LicenseType();
            try
            {
                model.LicenseTypeList = await _adminService.GetLicenseType(ID);
                {
                    LicenseTypeDet res = new LicenseTypeDet();
                    if (model.LicenseTypeList.Count > 0)
                    {
                        foreach (var item in model.LicenseTypeList)
                        {
                            if (item.Id == ID)
                            {
                                res.Id = item.Id;
                                res.Name = item.Name.ToString();
                                status = item.Active;
                                if (status.ToLower() == "yes")
                                {
                                    res.Active = true;
                                }
                                else { res.Active = false; }
                            }
                        }
                        model.LicenseTypeDet = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_LicenseTypeForm", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertLicenseType([FromBody] LicenseTypeDet model)
        {
            LicenseType objLicenseType = new LicenseType();
            model.CreatedBy = HttpContext.Session.GetString("UserID");
            objLicenseType.LicenseTypeDet = model;
            //if (model.Id == 0)
            //{
            //    await _adminService.InsertLicenseType(model.Id, model.Name, model.Active, model.CreatedBy);
            //}
            //else
            //{
            await _adminService.InsertLicenseType(model.Id, model.Name, model.Active, model.CreatedBy);
            //}
            objLicenseType.LicenseTypeList = await _adminService.GetLicenseType(0);
            return PartialView("_LicenseTypeList", objLicenseType);
        }

        [HttpPost]
        public async Task<JsonResult> IsLicenseTypeExist(String Name)
        {
            try
            {
                String value = Name;
                List<LicenseTypeList> check = new List<LicenseTypeList>();
                check = await _adminService.GetLicenseTypeListByName(Name);
                if (check.Count != 0)
                {
                    return Json(new { message = "Failed" });
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteLicenseType(int ID)
        {
            LicenseType model = new LicenseType();
            try
            {
                await _adminService.DeleteLicenseTypeDetails(ID);
                model.LicenseTypeList = await _adminService.GetLicenseType(0);
            }
            catch (Exception ex) { }
            return PartialView("_LicenseTypeList", model);
        }
        #endregion

        #region CertificationLevel
        [HttpGet]
        public async Task<IActionResult> CertificationLevel()
        {
            CertificationLevel objCertificationLevel = new CertificationLevel();
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                objCertificationLevel.CertificationLevelList = await _adminService.GetCertificationLevel(0);
                objCertificationLevel.CertificationLevelDet = new CertificationLevelDet();
            }
            catch (Exception ex) { }
            return View("CertificationLevel", objCertificationLevel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditCertificationLevel([FromBody] int ID)
        {
            String status = null;
            CertificationLevel model = new CertificationLevel();
            try
            {
                model.CertificationLevelList = await _adminService.GetCertificationLevel(ID);
                {
                    CertificationLevelDet res = new CertificationLevelDet();
                    if (model.CertificationLevelList.Count > 0)
                    {
                        foreach (var item in model.CertificationLevelList)
                        {
                            if (item.Id == ID)
                            {
                                res.Id = item.Id;
                                res.Name = item.Name.ToString();
                                status = item.Active;
                                if (status.ToLower() == "yes")
                                {
                                    res.Active = true;
                                }
                                else { res.Active = false; }
                            }
                        }
                        model.CertificationLevelDet = res;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_CertificationLevelForm", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertCertificationLevel([FromBody] CertificationLevelDet model)
        {
            CertificationLevel objCertificationLevel = new CertificationLevel();
            model.CreatedBy = HttpContext.Session.GetString("UserID");
            objCertificationLevel.CertificationLevelDet = model;
            //if (model.Id == 0)
            //{
            //    await _adminService.InsertCertificationLevel(model.Id, model.Name, model.Active, model.CreatedBy);
            //}
            //else
            //{
            await _adminService.InsertCertificationLevel(model.Id, model.Name, model.Active, model.CreatedBy);
            //}
            objCertificationLevel.CertificationLevelList = await _adminService.GetCertificationLevel(0);
            return PartialView("_CertificationLevelList", objCertificationLevel);
        }

        [HttpPost]
        public async Task<JsonResult> IsCertificationLevelExist(String Name)
        {
            try
            {
                String value = Name;
                List<CertificationLevelList> check = new List<CertificationLevelList>();
                check = await _adminService.GetCertificationLevelListByName(Name);
                if (check.Count != 0)
                {
                    return Json(new { message = "Failed" });
                }
            }
            catch (Exception ex) { }
            return Json("Success");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteCertificationLevel(int ID)
        {
            CertificationLevel model = new CertificationLevel();
            try
            {
                await _adminService.DeleteCertificationLevelDetails(ID);
                model.CertificationLevelList = await _adminService.GetCertificationLevel(0);
            }
            catch (Exception ex) { }
            return PartialView("_CertificationLevelList", model);
        }
        #endregion

        #region UserGroup
        [HttpGet]
        public async Task<IActionResult> UserGroup()
        {
            UserGroup objUserGroup = new UserGroup();
            await _menuUtils.SetMenu(HttpContext.Session);
            try
            {
                objUserGroup.UserGroupList = await _adminService.GetUserGroup(0);
                objUserGroup.UserGroupDet = new UserGroupDet();

                objUserGroup.UserNameList = await _adminService.GetUserNameList();
            }
            catch (Exception ex) { }
            return View("UserGroup", objUserGroup);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EditUserGroup([FromBody] int ID)
        {
            String status = null;
            UserGroup model = new UserGroup();
            try
            {
                model.UserGroupList = await _adminService.GetUserGroup(ID);
                {
                    UserGroupDet res = new UserGroupDet();
                    if (model.UserGroupList.Count > 0)
                    {
                        foreach (var item in model.UserGroupList)
                        {
                            if (item.Id == ID)
                            {
                                res.Id = item.Id;
                                res.GroupName = item.GroupName;
                                res.UserId = item.UserId;
                                status = item.Active;
                                if (status.ToLower() == "yes")
                                {
                                    res.Active = true;
                                }
                                else { res.Active = false; }
                            }
                        }
                        model.UserGroupDet = res;

                        model.UserNameList = await _adminService.GetUserNameList();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            ModelState.Clear();
            return PartialView("_UserGroupForm", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> InsertUserGroup([FromBody] UserGroupDet model)
        {
            UserGroup objUserGroup = new UserGroup();
            model.CreatedBy = HttpContext.Session.GetString("UserID");
            objUserGroup.UserGroupDet = model;

            await _adminService.InsertUserGroup(model.Id, model.GroupName, model.UserId, model.Active, model.CreatedBy);

            objUserGroup.UserGroupList = await _adminService.GetUserGroup(0);
            return PartialView("_UserGroupList", objUserGroup);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUserGroup(int ID)
        {
            UserGroup model = new UserGroup();
            try
            {
                await _adminService.DeleteUserGroupDetails(ID);
                model.UserGroupList = await _adminService.GetUserGroup(0);
            }
            catch (Exception ex) { }
            return PartialView("_UserGroupList", model);
        }
        #endregion
    }

}
