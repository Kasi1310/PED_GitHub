using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PED.Models;
using Microsoft.AspNetCore.Identity;

namespace WiseX.Models
{
    public class ApplicationRole :IdentityRole
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }   
}
