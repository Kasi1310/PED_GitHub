using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WiseX.Data;
using Microsoft.EntityFrameworkCore;
using WiseX.Models;
using System.Data.SqlClient;

namespace WiseX.Services
{
    public class HelperService : DbContext
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public HelperService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }


        public void SaveErrorLog(ErrorLog error)
        {
            try
            {
                var Errorlog = new SqlParameter("@Data", error.GetXml());
                 _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC SaveErrorLog @Data", Errorlog);
            }
            catch (Exception ex)
            {

            }
        }
    }


}