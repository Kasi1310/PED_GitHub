using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PED.ViewModels.ProviderEntrollment;
using WiseX.Data;
using Microsoft.Extensions.Configuration;
using NPOI.OpenXml4Net.OPC;
using Microsoft.AspNetCore.Http;
using System;
using WiseX.Helpers;
using WiseX.Services;

namespace PED.Controllers
{
    [Route("api/[controller]")]
    public class WebhookController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HttpClient _httpClient;
       // private readonly IConfiguration _configuration;
        private readonly string _secret;

        public WebhookController(ApplicationDbContext db, IConfiguration config)
        {
            //this.dummyEmail = _configuration["AppSettings:DummyEmailTo"].ToString();
            _db = db;
            //_configuration= config;
            _secret = config["AppSettings:WebhookSecret"];
            _httpClient = new HttpClient(); // Manual creation
        }


        // PUBLIC: Receives from 3rd-party
        [HttpPost("relay")]
        public async Task<IActionResult> Relay()
        {
            var body = await new StreamReader(Request.Body).ReadToEndAsync();

            var internalUrl = $"{Request.Scheme}://{Request.Host}/api/webhook/receive";
            var request = new HttpRequestMessage(HttpMethod.Post, internalUrl)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("X-Webhook-Token", _secret);

            var response = await _httpClient.SendAsync(request);
            return StatusCode((int)response.StatusCode);
        }

        // INTERNAL: Updates DB
        [HttpPost("receive")]
        public async Task<IActionResult> Receive([FromBody] WebhookPayload payload)
        {
            var token = Request.Headers["X-Webhook-Token"];
            if (token != _secret)
                return Unauthorized();

            var order = await _db.Orders.FindAsync(payload.Id);
            if (order == null)
            {
                _db.Orders.Add(new Order { Id = payload.Id, Status = payload.Status });
            }
            else
            {
                order.Status = payload.Status;
            }

            await _db.SaveChangesAsync();
            return Ok();
        }
        //[HttpPost("receive")]
        //public async Task<IActionResult> Receive([FromBody] WebhookPayload payload)
        //{
        //    if (HttpContext.Session.GetString("SessionRoleAccess").Replace("\"", "") == "R")
        //    {
        //        return Json("");
        //    }
        //    //ProviderEntrollment model = new ProviderEntrollment();

        //    string message = "";
        //    var userId = HttpContext.Session.GetString("UserID");
        //    try
        //    {
        //        await _menuUtils.SetMenu(HttpContext.Session);
        //        //model.PEDetails = pEDetails;

        //        await _adminService.InsertPEDetails(payload);

        //        message = "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        message = "Failed";
        //    }
        //    return Json(new { Message = message });
        //}
    }
}


//using Microsoft.AspNetCore.Mvc;

//namespace PED.Controllers
//{
//    public class WebhookController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
