using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace PED.ViewModels.Admin
{
    public class DocumentTitle : EntityBase
    {
        [Key]
        public string Id { get; set; }
        public DocumentTitleDet DocumentTitleDet;
        public List<DocumentTitleList> DocumentTitleList;
    }
    public class DocumentTitleDet : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
    public class DocumentTitleList
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public String Active { get; set; }

    }
}


