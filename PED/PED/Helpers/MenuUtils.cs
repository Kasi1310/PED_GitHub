using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Data;
using WiseX.Services;

namespace WiseX.Helpers
{
    public class MenuUtils
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly AdminService _adminService;

        public MenuUtils(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _adminService = new AdminService(_applicationDbContext);
        }

        public async Task<string> SetMenu(ISession httpContextSession)
        {
            string RoleID = httpContextSession.GetString("RoleID");
            string UserID = httpContextSession.GetString("UserID");
            var userMenus = await _adminService.MenuAccessRole(RoleID, UserID);
            httpContextSession.Set("UserMenuAccess", userMenus);
            return "";
        }
    }
}
