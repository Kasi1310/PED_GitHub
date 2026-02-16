using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;
using WiseX.Models;

namespace WiseX.ViewModels.Admin
{
    public class ChartManagement
    {
        public SearchConditionFilter SearchConditionFilter;
        public ChartHistoryList ChartHistory;
        public GetReallocateTeam GetReallocateTeam;
        public ReAssignUserList GetReAssignUserList;
        public ReAssignUser ReAssignUser;
        public List<Masters_ChartStatus> Masters_ChartStatusList;
        public IList<ChartHistoryList> ChartHistoryList;
        public IList<SearchConditionFilterList> SearchConditionFilterList;
        public List<ResourceDetails> ResourceDetailsList;
        public List<ProjectDetailsUsers> ProjectDetailsUsersList;
        public IList<Masters_X_HoldComments> HoldCommentList;
        public List<Masters_MoveToChartStatus> MoveToChartStatus;
        public List<ChartCurrentHistory> ChartCurrentHistoryList;
        public List<GetProjectTeams> GetProjectTeamsList;
        public List<GetReallocateTeam> GetReallocateTeamList;
        public List<ReAssignUserList> ReAssignUsersList;
        public  int CheckCurrentChartStatus;
    }

    public class CheckCurrentChartStatus
    {
        [Key]
        public int Count { get; set; }
    }

    public class MoveToChartStatus :EntityBase
    {
        public string[] ChartId { get; set; }
        public int MoveToChartStatu { get; set; }
    }

    public class ChartHistoryList
    {
        [Key]
        public int SlNo { get; set; }
        public int Id { get; set; }
        public int ChartID { get; set; }
        public string FileName { get; set; }
        public int MenuID { get; set; }
        public int SubMenuID { get; set; }
        public string MenuItem { get; set; }
        public string SubMenuItem { get;set;}
        public string ActionAttributes1 { get; set;}
        public string ActionAttributes2 { get; set; }
        public string ActionAttributes3 { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string  DateTime { get; set; }
        public string ActionMonth { get; set; }
        public string ActionTime { get; set; }
        public string ActionBy { get; set; }
    }

    public class SearchConditionFilterList
    {
        [Key]
        public int ROWID { get; set; }
        public int Id { get; set; }
        public string ScanDate { get; set; }
        public string FileName { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string Specialty { get; set; }
        public string ReviewerName { get; set; }
        public string ReviewedOn { get; set; }
        public string Auditor { get; set; }
        public int CoderResID { get; set; }
        public int AuditResID { get; set; }
        public string AuditedOn { get; set; }
        public string SuperAuditOn { get; set; }
        public int ChartStatusID { get; set; }
        public string ChartStatus { get; set; }
        public string ReviewHoldedBy { get; set; }
        public string AuditHoldedBy { get; set; }
        public string ReviewHoldReason { get; set; }
        public int ReviewHoldReasonID { get; set; }
        public string AuditHoldReason { get; set; }
        public string ReviewHoldComment { get; set; }
        public string AuditHoldComment { get; set; }
        public string ReviewHoldedOn { get; set; }
        public string AuditHoldedOn { get; set; }
        public string SuperAuditBy { get; set; }
        public int TotalCount { get; set; }
        public int IsAuditSample { get; set; }
        public int IsSuperAuditSample { get; set; }
    }

    public class ChartCurrentHistory
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string CodeReviewBy { get; set; }
        public string AuditedBy { get; set; }
        public string SuperAuditedBy { get; set; }
        public string ClientFeedbackBy { get; set; }
        public string ChartStatus { get; set; }
    }

    public class GetProjectTeams
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class GetReallocateTeam
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class ReAssignUserList
    {
        [Key]
        public string Id { get; set; }
        public string Value { get; set; }
    }
    public class ReAssignUser:EntityBase
    {
        public int ChartID { get; set; }
        public int MoveTo { get; set; }
        public string ReassignTo { get; set; }
        public string Comment { get; set; }
        public string Userid { get; set; }
       
    }
}
