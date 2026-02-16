using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WiseX.ViewModels.Admin
{
    public class Roles
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public byte Permissions { get; set; }
        public string PermissionsType { get; set; }
        public byte Active { get; set; }
        public bool Selected { get; set; } 
        public byte IsDeletable { get; set; }
    }
    public class RolePermissions
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PermissionsType { get; set; }
      
    }

}
