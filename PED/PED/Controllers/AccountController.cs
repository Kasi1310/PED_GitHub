#region Using

using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using WiseX.Models;
using WiseX.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using WiseX.Services;
using WiseX.Data;
using System.Collections.Generic;
using WiseX.Helpers;
using Newtonsoft.Json;
using System;
using Microsoft.EntityFrameworkCore;
using WiseX.ViewModels.Admin;
//using LogLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using PED.ViewModels.Contract;
using PED.Helpers;
using System.Text;


#endregion

namespace WiseX.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //LogFiles _objLog = null;
        String LogRoot = null;

        private readonly ILogger _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AdminService _adminService;
        private readonly CommonService _commonService;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ILoggerFactory loggerFactory, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _adminService = new AdminService(applicationDbContext);
            _commonService = new CommonService(applicationDbContext);
            this.LogRoot = Path.Combine(_hostingEnvironment.WebRootPath, _configuration["AppSettings:LogPath"].ToString());
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await EnsureLoggedOut();
            HttpContext.Session.Clear();
            var viewModel = new LoginViewModel { ReturnUrl = returnUrl };
            return View(viewModel);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model, string returnUrl = null)
        {
            object message;
            UserSessionDetails userSessionDetails = new UserSessionDetails();

            // MenuAccessRole menu = new MenuAccessRole();
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in.");

                //Get the User Name after Login Success 
                ApplicationUser user = await _userManager.FindByNameAsync(model.Username);
                RolePermissions RoleAccess = new RolePermissions();
                RoleAccess = await _commonService.GetRoleAccessControls(user.Id);

                if (RoleAccess != null)
                {
                    IdentityRole identityRole = new IdentityRole();
                    int NotificationCount = await _commonService.GetNotification(user.Id);
                    HttpContext.Session.SetString("UserID", user.Id);
                    HttpContext.Session.Set("SessionUserID", user.Id);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.Set("SessionUserName", user.UserName);
                    HttpContext.Session.SetString("RoleAccess", RoleAccess.PermissionsType);
                    HttpContext.Session.Set("SessionRoleAccess", RoleAccess.PermissionsType);
                    HttpContext.Session.SetString("NotificationCount", Convert.ToInt32(NotificationCount).ToString());
                    HttpContext.Session.Set("SessionNotificationCount", NotificationCount);
                    HttpContext.Session.SetString("SessionPwdValidationFlag", "1");
                    //Get the Roles assigned for the particular user and set in a session
                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    //var userId = HttpContext.Session.GetString(user.Id);
                    ApplicationRole role = await _roleManager.FindByNameAsync(roles[0].ToString());

                    var SessionID = HttpContext.Session.Id;
                    HttpContext.Session.SetString("SessionID", SessionID);

                    // Get the Roles of the user in a String and set in the session
                    var key = "UserRoles";
                    var str = JsonConvert.SerializeObject(roles);
                    HttpContext.Session.SetString(key, str);

                    HttpContext.Session.Remove("UserMenuAccess");
                    var userMenus = HttpContext.Session.Get<List<MenuAccessRole>>("UserMenuAccess");

                    //To check Password Expiry
                    userSessionDetails = await _commonService.GetUserSessionDetails(user.Id);
                    if (userSessionDetails != null)
                    {
                        TimeSpan timeSpan = DateTime.Now.Subtract(userSessionDetails.LastPasswordChangedDate.Value);
                        if (timeSpan.Days > Convert.ToInt32(_configuration["AppSettings:PasswordExpireDays"]))
                        {
                            HttpContext.Session.SetString("SessionPwdValidationFlag", "0");
                            message = "PasswordExpired";
                        }
                        else
                        {
                            message = "HomePage";
                            string sessionResult = string.Empty;
                            sessionResult = await SetSessionValues(user, 1);
                        }
                    }
                    else
                    {
                        HttpContext.Session.SetString("SessionPwdValidationFlag", "0");
                        message = "NewLogin";
                    }

                    return Json(new { message });
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return Json(new { message = "failure" });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {

            await EnsureLoggedOut();

            return View(new RegisterViewModel());
        }


        public async Task<string> SetSessionValues(ApplicationUser user, int flag)
        {
            string RoleID, RoleName;
            //Get the Roles assigned for the particular user and set in a session
            IList<string> roles = await _userManager.GetRolesAsync(user);
            ApplicationRole role = await _roleManager.FindByNameAsync(roles[0].ToString());

            if (role.Id != "")
            {
                HttpContext.Session.SetString("RoleID", role.Id);
                HttpContext.Session.Set("SessionRoleID", role.Id);
            }
            if (role.Name != "")
            {
                HttpContext.Session.SetString("RoleName", role.Name);
                HttpContext.Session.Set("SessionRoleName", role.Name);
            }

            var UserID = HttpContext.Session.GetString("UserID");
            var SessionId = HttpContext.Session.GetString("SessionID");
            UserSession userSession = new UserSession();
            userSession.SessionID = SessionId;
            userSession.UserID = UserID;
            userSession.Flag = flag;

            //To Store the details of Logged user in table for attendance purpose
            var RetId = await _commonService.UpdateUserSession(userSession);
            HttpContext.Session.SetString("RetId", RetId.ToString());
            return "success";
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                //    "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation(3, "User created a new account with password.");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            AddErrors(result);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /account/error
        [AllowAnonymous]
        public async Task<IActionResult> Error()
        {
            // We do not want to use any existing identity information
            await EnsureLoggedOut();

            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();

            //Retrive the Active Session and Update Logout Time
            var GetID = HttpContext.Session.GetString("RetId");
            UserSession userSession = new UserSession();
            userSession.Id = Convert.ToInt32(GetID);
            userSession.Flag = 3;
            await _commonService.UpdateUserSession(userSession);

            // Clear the principal to ensure the user does not retain any authentication
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            _logger.LogInformation(4, "User logged out.");
            HttpContext.Session.Remove("SessionID");
            return RedirectToLocal();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new
            {
                ReturnUrl = returnUrl
            });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new
                {
                    ReturnUrl = returnUrl
                });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            // If the user does not have an account, then ask the user to create an account.
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["LoginProvider"] = info.LoginProvider;
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel
            {
                Email = email
            });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            // We do not want to use any existing identity information
            //await EnsureLoggedOut();

            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                // If the user is found AND Email is confirmed
                if (user != null)// && await _userManager.IsEmailConfirmedAsync(user))
                {
                    // Generate the reset password token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Build the password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                    var userId = HttpContext.Session.GetString("UserID");

                    List<SMTPServerDetails> SMTP = new List<SMTPServerDetails>();
                    SMTP = await _commonService.GetSMTPServerDetails("");

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<html>");
                    sb.Append("<head>");
                    sb.Append("<title></title>");
                    sb.Append("<meta charset='utf-8' />");
                    sb.Append("</head>");
                    sb.Append("<body>");
                    sb.Append("<br />");
                    sb.Append("<table width='50%'>");
                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append("Hi, <br /><br />");
                    sb.Append("Find your password reset link.");
                    sb.Append("<br /><br />");
                    //sb.Append(passwordResetLink + "<br /><br />");  // Old
                    sb.Append("<a href=\"" + passwordResetLink + "\">" + passwordResetLink + "</a><br /><br />"); // New
                    sb.Append("Regards,<br /> ");
                    sb.Append("PED Admin");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    //sb.Append("<tr>");
                    //sb.Append("<img src='http://cms.medicount.com/img/MedicountHeaderLogo.png' width='150px' />");
                    //sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<br />");
                    sb.Append("</body>");
                    sb.Append("</html>");

                    if (SMTP.Count > 0)
                    {
                        CommonMail.SendMail(SMTP[0].SMTPServer, SMTP[0].SMTPPort, SMTP[0].SMTPSSL, SMTP[0].SMTPUserName, SMTP[0].SMTPPassword, model.Email, "Forgot Password-Reset Link", sb.ToString(), "", "");
                    }

                    // Log the password reset link
                    //logger.Log(LogLevel.Warning, passwordResetLink);

                    // Send the user to Forgot Password Confirmation view
                    return View("ForgotPasswordConfirmation");
                }

                // To avoid account enumeration and brute force attacks, don't
                // reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            return View(model);


        }


        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem
            {
                Text = purpose,
                Value = purpose
            }).ToList();
            return View(new SendCodeViewModel
            {
                Providers = factorOptions,
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            });
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl = null)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private async Task EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (User.Identity.IsAuthenticated)
                await LogOff();
        }
        //
        // GET: /Account/ChangePassword
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword()
        {
            var UserID = HttpContext.Session.GetString("UserID");
            ApplicationUser user = await _userManager.FindByIdAsync(UserID);
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.UserName = user.UserName;
            model.UserId = user.Id;
            return View(model);
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [AllowAnonymous]

        public async Task<JsonResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            //int x = 0;
            //x /= x;

            string Message = string.Empty;
            string returnValue = string.Empty;
            var UserID = HttpContext.Session.GetString("UserID");
            try
            {
                var user = await _userManager.FindByIdAsync(UserID);
                var NewPassword = model.Newpassword;
                var Result = await _userManager.ChangePasswordAsync(user, model.Currentpassword, model.Newpassword);
                if (!Result.Succeeded)
                {
                    foreach (var err in Result.Errors)
                    {
                        if (err.Code.ToLower() == "passwordmismatch")
                            returnValue = "1";
                        else
                            returnValue = "2";
                    }
                }
                else
                {
                    string sessionResult = string.Empty;
                    sessionResult = await SetSessionValues(user, 2);
                    returnValue = "success";
                    HttpContext.Session.SetString("SessionPwdValidationFlag", "1");
                }
            }
            catch (Exception ex)
            {
                //_objLog.WriteToFiledownloderServicelog("UNC Exception" + "    " + ex.Message, LogPath, _IsWriteLog);
            }
            return Json(returnValue);
        }

        public async Task<IActionResult> AjaxSessionExpire()
        {
            try
            {
                // Set as Unauthorized.
                this.Response.StatusCode = 403;
                return this.Json(new
                {
                    Type = "SESSION_TIMEOUT",
                    Message = "Session timedout."
                });
            }
            catch { throw; }
        }


    }
}