using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Hosting.Internal;
using WiseX.ViewModels.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using WiseX.Models;
using Microsoft.Extensions.Configuration;

namespace WiseX.Helpers
{
    public class FilesHelper
    {
       
        String DeleteURL = null;
        String DeleteType = null;
        String StorageRoot = null;
        String UrlBase = null;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public FilesHelper(String DeleteURL, String DeleteType, String UrlBase, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            this.DeleteURL = DeleteURL;
            this.DeleteType = DeleteType;
        }      
        
        

      

        

    }
   
    

}
