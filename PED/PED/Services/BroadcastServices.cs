using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace WiseX.Services
{
    public class BroadcastServices : DbContext
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public BroadcastServices(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task UpdateBroadcastReadStatus(string UserID ,int ProjectId)
        {
            try
            {
                var paramUserId = new SqlParameter("@UserId", UserID);
                var PID = new SqlParameter("@ProjectId", ProjectId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateBroadcastReadStatus @UserId,@ProjectId", paramUserId,PID);
            }
            catch (Exception Ex)
            {

            }

        }
       
        public async Task DeleteBroadCast(int Id)
        {
            try
            {
                var Broadcast = new SqlParameter("@Id", Id);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteBroadCastDetails @Id", Broadcast);
            }
            catch (Exception Ex)
            {

            }
        }
       
    }
}
