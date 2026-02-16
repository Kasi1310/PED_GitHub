using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace PED.ViewModels.Admin
{
    public class ContractStatusDetails : EntityBase
    {
        [Key]
        public string Id { get; set; }
        public ContractStatusDet ContractStatusDet;
        public List<ContractStatusList> ContractStatusList;
    }
    public class ContractStatusDet : EntityBase
    {
        public int Id { get; set; }
        public string ContractStatus { get; set; }
        public byte Active { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
      
    }
    public class ContractStatusList
    {
        [Key]
        public int Id { get; set; }
        public string ContractStatus { get; set; }
        public String Active { get; set; }

    }
}
