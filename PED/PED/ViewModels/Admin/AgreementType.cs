using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace PED.ViewModels.Admin
{
    public class AgreementType : EntityBase
    {
        [Key]
        public string Id { get; set; }
        public AgreementTypeDetails AgreementTypeDetails;
        public List<AgreementTypeDetailsList> AgreementTypeDetailsList;
        
    }

    public class AgreementTypeDetails : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public byte Active { get; set; }
        public byte IsDeletable { get; set; }
    }
    public class AgreementTypeDetailsList 
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool Active { get; set; }
      
    }
}
