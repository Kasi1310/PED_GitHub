using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;
using WiseX.Models;

namespace WiseX.ViewModels.Admin
{
    public class Allocation
    {
        public int ChartStatusCount;
        public int AutoAllocationStatus;
        public AllocationDetails AllocationDetails;
        public ProjectChartsCount ProjectChartsCount;
        public ResourceDetails ReviewTeam;
        public ResourceDetails AuditTeam;
        public ProjectDetailsUsers projectDetailsUsers;
        public List<ResourceDetails> ReviewTeamList;
        public List<ResourceDetails> AuditTeamList;
        public List<ProjectDetailsUsers> ProjectDetailsUsersList;
        public List<AllocatedList> AllocatedList;
        public List<ProjectChartsCount> ProjectChartsCountList;
        public List<SelfAllocation> SelfAllocationList;
        public string DemograhicsTemplateFileURL { get; set; }

    }
    public class AllocationDetails : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public int Project { get; set; }
        public int Charts { get; set; }
        public int ActualCharts { get; set; }
        public string[] Resource { get; set; }
        public string[] AuditResource { get; set; }
        public string ReviewResourceID { get; set; }
        public string AuditResourceID { get; set; }
        public int ActiveStatus { get; set; }
        public int AllocationType { get; set; }
        public string ReviewResourceName { get; set; }
        public string AuditResourceName { get; set; }
        public int AssignedCharts { get; set; }
        public int OldChartCount { get; set; }
        public int ChartsTotal { get; set; }
        public int NotAllocated { get; set; }
        public int Allocated { get; set; }
        public int InProgress { get; set; }
        public int New { get; set; }
    }
    public class AllocatedList
    {
        [Key]
        public int Id { get; set; }
        public string Batch { get; set; }
        public int ProjectID { get; set; }
        public string ReviewResourceID { get; set; }
        public string AuditResourceID { get; set; }
        public int ChartCount { get; set; }
        public string ReviewResource { get; set; }
        public string AuditResource { get; set; }
        public string ProjectName { get; set; }
        public int ActiveStatus { get; set; }
    }
   
    public class ProjectChartsCount
    {
        [Key]
        public int ChartsTotal { get; set; }
        public int NotAllocated { get; set; }
        public int Allocated { get; set; }
        public int InProgress { get; set; }
        public int New { get; set; }
        public int ChartCount { get; set; }
    }
    public class ChartStatusCount
    {
        [Key]
        public int Count { get; set; }
    }
    public class SelfAllocationData : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public List<SelfAllocation> SelfAllocationList { get; set; }
    }
    public class SelfAllocation 
    {
        [Key]
        public int Id { get; set; }
        public int Sno { get; set; }
        public string ProjectName { get; set; }
        public string FileName { get; set; }
        public string TotalPage { get; set; }
        public string CoderName { get; set; }
        public string CodingTeam { get; set; }
        public string AuditTeam { get; set; }
        public string ErrorText { get; set; }
        public int Error { get; set; }
        public int ErrorFlag { get; set; }
    }
  
}
