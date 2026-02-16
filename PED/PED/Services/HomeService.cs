using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using WiseX.Data;
using WiseX.ViewModels.Home;
using System.Data;

namespace WiseX.Services
{
    public class HomeService : DbContext
    {
        private readonly ApplicationDbContext _applicationDbContext;
        string connectionString = string.Empty;

        public HomeService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            //connectionString = _applicationDbContext.Database.GetDbConnection().ConnectionString;
            connectionString = DBConnection.ConnectionString;
        }


        public async Task<ChartProperties> GetBoxes()
        {
            ChartProperties lst = new ChartProperties();
            try
            {
                lst = await _applicationDbContext.ChartProperties.FromSql("EXEC [Dashboard].[GetBoxes]").FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }

        public async Task<List<ChartBoxProperties>> GetBoxesList(string BoxName, string UserID)
        {
            List<ChartBoxProperties> lst = new List<ChartBoxProperties>();
            var DashBoardName = new SqlParameter("@BoxName", BoxName);
            // var ParamUserID = new SqlParameter("@UserID", UserID);
            try
            {
                // lst = await _applicationDbContext.ChartBoxProperties.FromSql("EXEC [Dashboard].[GetBoxsDetails] @BoxName,@UserID", DashBoardName, ParamUserID).ToListAsync();
                lst = await _applicationDbContext.ChartBoxProperties.FromSql("EXEC [Dashboard].[GetBoxsDetails] @BoxName", DashBoardName).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }

        public async Task<List<ChartBoxPropertiesLoad>> GetBoxesList1(int Start, int Length, string BoxName, string UserID)
        {
            List<ChartBoxPropertiesLoad> lst = new List<ChartBoxPropertiesLoad>();
            var DashBoardName = new SqlParameter("@BoxName", BoxName);
            var ParamStart = new SqlParameter("@Start", Start);
            var ParamLength = new SqlParameter("@Length", Length);
            // var ParamUserID = new SqlParameter("@UserID", UserID);
            try
            {
                // lst = await _applicationDbContext.ChartBoxPropertiesLoad.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName,@UserID", ParamStart, ParamLength, DashBoardName, ParamUserID).ToListAsync();
                lst = await _applicationDbContext.ChartBoxPropertiesLoad.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName", ParamStart, ParamLength, DashBoardName).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }

        public async Task<List<PECredentialingLicenseBoxesList>> GetPECredentialingLicenseBoxesList(int Start, int Length, string BoxName, string UserID)
        {
            List<PECredentialingLicenseBoxesList> lst = new List<PECredentialingLicenseBoxesList>();
            var DashBoardName = new SqlParameter("@BoxName", BoxName);
            var ParamStart = new SqlParameter("@Start", Start);
            var ParamLength = new SqlParameter("@Length", Length);
            // var ParamUserID = new SqlParameter("@UserID", UserID);
            try
            {
                // lst = await _applicationDbContext.ChartBoxPropertiesLoad.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName,@UserID", ParamStart, ParamLength, DashBoardName, ParamUserID).ToListAsync();
                lst = await _applicationDbContext.PECredentialingLicenseBoxesList.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName", ParamStart, ParamLength, DashBoardName).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }


        public async Task<List<PEContractBoxesList>> GetPEContractBoxesList(int Start, int Length, string BoxName, string UserID)
        {
            List<PEContractBoxesList> lst = new List<PEContractBoxesList>();
            var DashBoardName = new SqlParameter("@BoxName", BoxName);
            var ParamStart = new SqlParameter("@Start", Start);
            var ParamLength = new SqlParameter("@Length", Length);
            // var ParamUserID = new SqlParameter("@UserID", UserID);
            try
            {
                // lst = await _applicationDbContext.ChartBoxPropertiesLoad.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName,@UserID", ParamStart, ParamLength, DashBoardName, ParamUserID).ToListAsync();
                lst = await _applicationDbContext.PEContractBoxesList.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName", ParamStart, ParamLength, DashBoardName).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }

        public async Task<List<PECheckListBoxes>> GetPECheckListBoxes(int Start, int Length, string BoxName, string UserID)
        {
            List<PECheckListBoxes> lst = new List<PECheckListBoxes>();
            var DashBoardName = new SqlParameter("@BoxName", BoxName);
            var ParamStart = new SqlParameter("@Start", Start);
            var ParamLength = new SqlParameter("@Length", Length);
            try
            {
                lst = await _applicationDbContext.PECheckListBoxes.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName", ParamStart, ParamLength, DashBoardName).AsNoTracking().ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }

        public async Task<List<PECheckListBoxesFroAllocatedUser>> GetPECheckListBoxesForAllocatedUser(int Start, int Length, string BoxName, string UserID)
        {
            List<PECheckListBoxesFroAllocatedUser> lst = new List<PECheckListBoxesFroAllocatedUser>();
            var DashBoardName = new SqlParameter("@BoxName", BoxName);
            var ParamStart = new SqlParameter("@Start", Start);
            var ParamLength = new SqlParameter("@Length", Length);
            try
            {
                lst = await _applicationDbContext.PECheckListBoxesFroAllocatedUser.FromSql("EXEC [Dashboard].[GetBoxsDetailsLoad] @Start,@Length,@BoxName", ParamStart, ParamLength, DashBoardName).AsNoTracking().ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return lst;
        }

    }
}
