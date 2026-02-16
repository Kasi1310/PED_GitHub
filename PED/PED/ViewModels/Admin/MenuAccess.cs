using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WiseX.Helpers;

namespace WiseX.ViewModels.Admin
{
    public class MenuItem
    {
        public int id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public state state { get; set; }
        //public bool opened { get; set; }
        //public bool disabled { get; set; }
        public bool selected { get; set; }
        public string li_attr { get; set; }
        public string a_attr { get; set; }
    }

    public class state
    {
        [Key]
        public bool selected { get; set; }
        public bool opened { get; set; }
       // public bool disabled { get; set; }

    }

    public class Menu
    {
        public IList<MenuItem> MenuList;
    }
    public class RoleModules:EntityBase
    {

        [Key]
        public string RoleId { get; set; }
        public List<SubModuleItem> SubModuleList;     

    }
    public class SubModuleItem
    {       
        public string SubModuleId { get; set; }
        public string ModuleId { get; set; }
    }
   
}

