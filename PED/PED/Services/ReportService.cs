using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Data;
using WiseX.Models;
using WiseX.ViewModels.Admin;

namespace WiseX.Services
{
    public class ReportService : DbContext
    {
        private readonly ApplicationDbContext _applicationDbContext;
        string connectionString = string.Empty;

        public ReportService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            connectionString = _applicationDbContext.Database.GetDbConnection().ConnectionString;
        }
        
    }
}
