using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace PED.ViewModels.Admin
{
    public class AnnualChargeRate : EntityBase
    {
        public AnnualChargeRateDet AnnualChargeRateDet;
        public List<AnnualChargeRateList> AnnualChargeRateList;
    }

    public class AnnualChargeRateDet : EntityBase
    {
        public int ID { get; set; }
        public string CompanyID { get; set; }
        public string  FilePath { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AnnualChargeRateList
    {
        [Key]
        public int ID { get; set; }
        public string CompanyID { get; set; }
        public string FilePath { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
    }
}
