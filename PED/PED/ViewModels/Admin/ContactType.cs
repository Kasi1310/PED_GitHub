using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace PED.ViewModels.Admin
{
    public class ContactType : EntityBase
    {
        [Key]
        public string Id { get; set; }
        public ContactTypeDet ContactTypeDet;
        public List<ContactTypeList> ContactTypeList;
    }
    public class ContactTypeDet : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
    public class ContactTypeList
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public String Active { get; set; }

    }
}


