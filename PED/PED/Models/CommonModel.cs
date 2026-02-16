using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;
namespace WiseX.Models
{
    public class CommonModel
    {

    }

    public class UserSession : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public int Flag { get; set; }
        public string UserID { get; set; }
        public string SessionID { get; set; }
    }

    public class ChartListModel
    {
        public IList<ChartDetails> ChartList;
        public ProjectDetails ProjectDetails;
        public List<ProjectDetails> ProjectDetailsList;
        public ResourceDetails ResourceDetails;
        public List<ResourceDetails> ResourceDetailsList;
        public ProjectDropDown ProjectDropDown;
        public List<ProjectDropDown> ProjectDropDownList;
        //To Set Rules for Chart
        public ChartProjectRules ChartProjectRules;
        public ChartsCleanup chartsCleanup;
        public List<ChartsCleanup> ChartCleanupList;
        public IList<ChartFileDetails> ChartFileDetails;

        public bool LicenseMessage = false;
    }

    public class ChartFileDetails
    {
        [Key]
        public int Id { get; set; }
        public int FileId { get; set; }
        public int ChartId { get; set; }
        public string ScanDate { get; set; }
        public string FileName { get; set; }
        public int ProjectID { get; set; }
        public string Project { get; set; }
        public int TotalCharges { get; set; }
        public int TotalPatients { get; set; }
        public int TotalCount { get; set; }
    }

    public class ChartDetails
    {
        [Key]
        public int Id { get; set; }
        //public int FileId { get; set; }
        public string ScanDate { get; set; }
        public string FileName { get; set; }
        public string MemberID { get; set; }
        public string MemberName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public int ProjectID { get; set; }
        public int AllocationID { get; set; }
        public string Project { get; set; }
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string Specialty { get; set; }
        public string ReviewerID { get; set; }
        public string ReviewerName { get; set; }
        public string ReviewedOn { get; set; }
        public string ReviewStartedOn { get; set; }
        public string AuditorID { get; set; }
        public string Auditor { get; set; }
        public string AuditedOn { get; set; }
        public int ChartStatusID { get; set; }
        public string ChartStatus { get; set; }
        //public string RenProviderID { get; set; }
        //public string RenProviderName { get; set; }
        public string ReviewHoldedBy { get; set; }
        public string ReviewHoldReason { get; set; }
        public string ReviewHoldComment { get; set; }
        public string ReviewHoldedOn { get; set; }
        public string AuditHoldedBy { get; set; }
        public string AuditHoldReason { get; set; }
        public string AuditHoldComment { get; set; }
        public string AuditHoldedOn { get; set; }
        public string SuperAuditorID { get; set; }
        public string SuperAuditor { get; set; }
        public string SuperAuditedOn { get; set; }
        public int ResourceID { get; set; }
        public string Resource { get; set; }

        public int TotalCount { get; set; }
        public int AuditorHasAction { get; set; }
        public int AuditeeHasAction { get; set; }
    }

    public class EncounterList
    {
        public List<DosDetails> DosList;
        public List<DiagDetails> DiagList;
        public List<DosDetailsDosComments> DosCommentList;
        public List<DiagDetailsDiagComments> DiagCommentList;
        public List<CACDiagDetails> CACDiagDetailsList;
    }

    public class SPChartLineDetailsList
    {
        public List<SPDosDetails> DosDetailsList;
        public List<SPProcedureCodeDetails> ProcedureCodeDetailsList;
        public List<SPModifierDetails> ModifierDetailsList;
        public List<SPDiagnosisDetails> DiagnosisDetailsList;
    }

    public class SPChartLineDetails : EntityBase
    {
        [Key]
        public int LineDetailsID { get; set; }
        public string CurrentPage { get; set; }
        public string EditLine { get; set; }
        public SPDosDetails SPDosDetails { get; set; }
        public SPProviderDetails SPProviderDetails { get; set; }
        public SPRenProviderDetails SPRenProviderDetails { get; set; }
        public SPProcedureCodeDetails ProcedureCodeDetails { get; set; }
        public List<SPModifierDetails> ModifiersList { get; set; }
        public List<SPDiagnosisDetails> DiagnosisList { get; set; }
    }

    public class SPProviderDetails
    {
        [Key]
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
    }

    public class SPRenProviderDetails
    {
        [Key]
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
    }

    public class SPDosDetails
    {
        [Key]
        public int LineDetailsID { get; set; }
        public int? LineNo { get; set; }
        public int ChartID { get; set; }
        public string FromDOS { get; set; }
        public string ToDOS { get; set; }
        public string Status { get; set; }
    }

    public class SPProcedureCodeDetails
    {
        [Key]
        public int? ProcCodeDetailsID { get; set; }
        public int? LineDetailsID { get; set; }
        public string ProcedureCode { get; set; }
    }

    public class SPModifierDetails
    {
        [Key]
        public int? ModifierID { get; set; }
        public int? LineDetailsID { get; set; }
        public string Modifier { get; set; }
    }

    public class SPDiagnosisDetails
    {
        [Key]
        public int? DiagDetailsID { get; set; }
        public int? LineDetailsID { get; set; }
        public string Diagnosis { get; set; }
    }

    public class DosDiagDetails : EntityBase
    {
        [Key]
        public int DosDiagDetailsID { get; set; }
        public string EditDiag { get; set; }
        public string CurrentPage { get; set; }
        public DosDetails DosDetails { get; set; }
        public List<DiagDetails> DiagList { get; set; }
        public List<DosDetailsDosComments> DosCommentList;
        public List<DiagDetailsDiagComments> DiagCommentList;
        public OtherDiagValues OtherDiagListValues { get; set; }
        public List<DiagValues> DiagListValues { get; set; }
        public List<CACDiagDetails> CACDiagList { get; set; }

    }

    public class DosDetails
    {
        [Key]
        public int DosID { get; set; }
        public int? AuditDOSID { get; set; }
        public int ChartID { get; set; }
        public string FromDOS { get; set; }
        public string ToDOS { get; set; }
        public int VisitTypeID { get; set; }
        public string VisitType { get; set; }
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
        public int? AuditActionID { get; set; }
        public string AuditAction { get; set; }
        public int? AuditReasonID { get; set; }
        public string AuditReason { get; set; }
        public string DosComment { get; set; }
        public string Status { get; set; }
    }
    public class DiagDetails : EntityBase
    {
        [Key]
        public int? DiagID { get; set; }
        public int? AuditDiagID { get; set; }
        public int? DosID { get; set; }
        public int? LineNumber { get; set; }
        public string Diag { get; set; }
        public string PageNumber { get; set; }
        public string POS { get; set; }
        public int PrimaryCommentsId { get; set; }
        public string PrimaryComment { get; set; }
        public List<DiagDetailsDiagComments> DiagCommentList;
        public int? AuditActionID { get; set; }
        public string AuditAction { get; set; }
        public int? AuditReasonID { get; set; }
        public string AuditReason { get; set; }
        public string DiagComment { get; set; }
        public string ClientComment { get; set; }
        public string OtherComments { get; set; }
        public int AnnotationId { get; set; }
        public int AnnotateActionId { get; set; }
        public string ColorCode { get; set; }
        public int HCCValue { get; set; }
    }
    public class OtherDiagValues : EntityBase
    {
        [Key]
        public int? Id { get; set; }
        public List<DiagValues> DiagValuesList { get; set; }
    }
    public class DiagValues 
    {
        [Key]
        public int? Id { get; set; }
        public string Diag { get; set; }
    }
    public class ValidataionResult
    {
        [Key]

        //public List<string> ErrorDiags { get; set; }
        public string ResultDiags { get; set; } // To Check Proc Result 
        //public string ErrorCodes { get; set; }
        public int ErrorType { get; set; }
        public string DuplicateDosDiags { get; set; }
        public string AddCodes { get; set; }
        public string DelCodes { get; set; }
        public string PrimaryDiagCodes { get; set; }
        public string SecondaryDiagCodes { get; set; }
        public string GenderCodes { get; set; }
        public string AgeCodes { get; set; }
        public string PregnancyCodes { get; set; }
        public string NewbornCodes { get; set; }

        public List<string> PrimaryDiagList { get; set; }
        public List<string> SecondaryDiagList { get; set; }

        public List<string> GenderCodeList { get; set; }
        public List<string> AgeCodeList { get; set; }
        public List<string> PregnancyCodeList { get; set; }
        public List<string> NewbornCodeList { get; set; }

        //public int AgeError { get; set; }
        //public int GenderError { get; set; }

    }
    public class DosDetailsDosComments
    {
        [Key]
        public int? DosDetailsDosCommentsID { get; set; }
        public int? AuditDosDetailsDosCommentsID { get; set; }
        public int? DosDetailsID { get; set; }
        public int? DosCommentsID { get; set; }
        public string DosComment { get; set; }
    }
    public class DiagDetailsDiagComments
    {
        [Key]
        public int DiagDetailsDiagCommentsId { get; set; }
        public int? AuditDiagDetailsDiagCommentsID { get; set; }
        public int? LineNumber { get; set; }
        public int? DiagDetailsID { get; set; }
        public int? DiagCommentsID { get; set; }
        public string DiagComment { get; set; }
    }

    public class HoldChartDetails : EntityBase
    {
        public int ChartID { get; set; }
        public int HoldReasonID { get; set; }
        public string HoldComment { get; set; }
        public int HoldTypeID { get; set; }
        public string HoldedBy { get; set; }
    }

    public class Masters_X_Providers
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class Masters_X_VisitTypes
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
    public class Masters_X_PrimaryComments
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class Masters_X_ICD
    {
        [Key]
        public int ICDId { get; set; }
        public string ICD { get; set; }
        public string ICDColor { get; set; }
        public int HCCValue { get; set; }
        //public string Description { get; set; }
    }
    public class Masters_X_DosComments
    {
        [Key]
        public int DOSId { get; set; }
        public string Comment { get; set; }
    }
    public class Masters_X_DiagComments
    {
        [Key]
        public int DiagId { get; set; }
        public string Comment { get; set; }
        public byte IsDosRequired { get; set; }
        public byte IsDiagRequired { get; set; }
        public byte IsPageNoRequired { get; set; }
    }
    public class Masters_X_HoldComments
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public class Masters_X_AuditActions
    {
        [Key]
        public int Id { get; set; }
        public string Action { get; set; }
    }

    public class Masters_X_AuditReasons
    {
        [Key]
        public int Id { get; set; }
        public string Reason { get; set; }
    }
    public class Masters_ChartStatus
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }

    }
    public class ChartsCleanup
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
    }
    public class Masters_MoveToChartStatus
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }

    }
    public class CACCodes
    {
        [Key]
        public int id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public CheckBoxState state { get; set; }
        public bool selected { get; set; }
        public string li_attr { get; set; }
        public string a_attr { get; set; }
    }

    public class CheckBoxState
    {
        [Key]
        public bool selected { get; set; }
        public bool opened { get; set; }
    }
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
    }

    public class SearchConditionChartFilter : EntityBase
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public string ScanDate { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ResourceID { get; set; }
        public int SortColumnID { get; set; }
        public string SortColumnName { get; set; }
        public string SortDirection { get; set; }
    }

    public class SearchConditionFilter : EntityBase
    {

        public int Start { get; set; }
        public int Length { get; set; }
        public string FileName { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public string Gender { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ResourceID { get; set; }
        public string ScanDate { get; set; }
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string DOB { get; set; }
        public string ReviewerID { get; set; }
        public string ReviewerName { get; set; }
        public string ReviewedOn { get; set; }
        public string Auditor { get; set; }
        public string AuditedOn { get; set; }
        public string ChartStatus { get; set; }
        //public string Status { get; set; }

        public int SortColumnID { get; set; }
        public string SortColumnName { get; set; }
        public string SortDirection { get; set; }

        public string ReviewHoldedBy { get; set; }
        public string AuditHoldedBy { get; set; }
        public string ReviewHoldReason { get; set; }
        public string AuditHoldReason { get; set; }
        public string ReviewHoldComment { get; set; }
        public string AuditHoldComment { get; set; }
        public string ReviewHoldedOn { get; set; }
        public string AuditHoldedOn { get; set; }

        public string SuperAuditor { get; set; }
        public string SuperAuditedOn { get; set; }
    }
    public class ProjectDetails
    {
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int ProjectTypId { get; set; }
        public string ProjectDescription { get; set; }
        public byte Shore { get; set; }
        public byte PreloadData { get; set; }
    }
    public class ResourceDetails
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
    public class ProjectDropDown
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
    public class NotificationDetails
    {
        [Key]
        public int RID { get; set; }
        public int Id { get; set; }
        public string SentOn { get; set; }
        public string EffectiveDate { get; set; }
        public string SentBy { get; set; }
        public int ClientID { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ExpiryDate { get; set; }
        public string SentTo { get; set; }
        public string UserName { get; set; }

    }
    public class UserProjectRole
    {
        public int ProjectID { get; set; }
        public int Id { get; set; }
        public string RoleID { get; set; }
        public string ProjectName { get; set; }
        public int AuditSamplePercent { get; set; }
        public int NormsPerDay { get; set; }
        public string ProjectRoleID { get; set; }
    }
    public class EnrollmentDetails
    {
        public int Id { get; set; }
        public string EnrollmentStartDate { get; set; }
        public string EnrollmentEndDate { get; set; }
    }

    public class ChartProjectRules
    {
        [Key]
        public int ProjectId { get; set; }
        public int CaptureDiags { get; set; }
        public int CaptureFrequency { get; set; }
        public byte GenderEdit { get; set; }
        public byte AgeEdit { get; set; }
        public byte PregnancyEdit { get; set; }
        public byte NewBornEdit { get; set; }
        public string ReviewYear { get; set; }
        public bool AutoPopulate { get; set; }
        public bool SliderPopup { get; set; }
        public byte PreloadData { get; set; }
    }
    public class MasterPlugInDetails
    {
        [Key]
        public int Id { get; set; }
        public string PlugInName { get; set; }
        public bool Selected { get; set; }
    }
    public class ReportSearchFilter : EntityBase
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public int SortColumnID { get; set; }
        public string SortColumnName { get; set; }
        public string SortDirection { get; set; }

        public string CoderID { get; set; }
        public int ProjectID { get; set; }
        public int ResourceID { get; set; }
        public string ChartStatus { get; set; }

        public string ScanDate { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string FileName { get; set; }

        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string ReviewerID { get; set; }
        //public string ReviewerName { get; set; }
        //public string ReviewedOn { get; set; }
        public string Auditor { get; set; }
        //public string AuditedOn { get; set; }
        public string ReviewHoldedBy { get; set; }
        public string ReviewHoldedOn { get; set; }
        public string ReviewHoldReason { get; set; }
        public string FromRange { get; set; }
        public string ToRange { get; set; }
    }
    //public class ReportResult
    //{
    //    [Key]
    //    public int ROWID { get; set; }
    //    public string Coder { get; set; }
    //    public int CompletedCharts { get; set; }
    //    public int PendingCharts { get; set; }
    //    public int ErrorCharts { get; set; }
    //    public int NoOfCodes { get; set; }
    //    public int NoOfErrorCodes { get; set; }
    //    public int NoOfNewCodes { get; set; }
    //    public double Quality { get; set; }
    //    public double Efficiency { get; set; }
    //}
    public class CoderDetails
    {
        [Key]
        public string UserID { get; set; }
        public string CoderName { get; set; }

    }
    public class AuditorDetails
    {
        [Key]
        public string UserID { get; set; }
        public string AuditorName { get; set; }
    }
    public class GuidelineDetails
    {
        [Key]
        public int ProjectId { get; set; }
        public string FileName { get; set; }
    }
    public class MasterTableOptions : EntityBase
    {
        [Key]
        public int ChartID { get; set; }
        public int ProjectID { get; set; }
    }

    public class ProcedureCodes
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProcCode { get; set; }
    }

    public class Modifiers
    {
        [Key]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Modifier { get; set; }
    }

    public class CACDiagDetails
    {
        [Key]
        public int AnnotationId { get; set; }
        public string Diag { get; set; }
        public string PageNo { get; set; }
        public int DiagCommentsID { get; set; }
        public string DiagComment { get; set; }
        public int AnnotateActionId { get; set; }
        public string ColorCode { get; set; }
        public int HCCValue { get; set; }
    }
    public class ProjectReport
    {
        [Key]
        //public int ROWID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int TotalCharts { get; set; }
        public int ChartsAllocated { get; set; }
        public int NotTouched { get; set; }
        public int ChartsCompleted { get; set; }
        public int InProgress { get; set; }
        public int Pending { get; set; }
        public double Quality { get; set; }
        public double Efficiency { get; set; }
        public double ProductionAvg { get; set; }
        public int NoOfTeams { get; set; }
        public int TeamAvgPrd { get; set; }
        public int InActiveTeam { get; set; }
        public string ActualDueDate { get; set; }
        public string ExpCompletionDate { get; set; }
        public string CapacityPrediction { get; set; }
    }
    public class TeamReport
    {
        [Key]
        //public int ROWID { get; set; }
        public int ResourceID { get; set; }
        public string TeamName { get; set; }
        public string ProjectName { get; set; }
        public int ChartsAllocated { get; set; }
        public int NotTouched { get; set; }
        public int ChartsCompleted { get; set; }
        public int InProgress { get; set; }
        public int Pending { get; set; }
        public double Quality { get; set; }
        public int ProductionAvg { get; set; }
        public double Efficiency { get; set; }
        public int NoOfCoder { get; set; }
        public int ProductionAvgPerCoder { get; set; }
        public int InActiveCoder { get; set; }
    }

    public class ChartReport
    {
        [Key]
        //public int ROWID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ExpectedVolume { get; set; }
        public int TotalChartsReceived { get; set; }
        public int TotalOCRPending { get; set; }
        public int TotalCACPending { get; set; }
        public int NotStarted { get; set; }
        public int InprogressCoding { get; set; }
        public int AuditPending { get; set; }
        public int AuditInProgress { get; set; }
        public int AuditQA { get; set; }
        public int SuperAuditPending { get; set; }
        public int SuperAuditInprogress { get; set; }
        public int SuperAuditQA { get; set; }
        public int ChartsCompleted { get; set; }
        public int ErrorCharts { get; set; }
       
    }

    public class CoderReport
    {
        [Key]
        //public int ROWID { get; set; }
        public string CoderId { get; set; }
        public string CoderName { get; set; }
        public int CompletedCharts { get; set; }
        public int PendingCharts { get; set; }
        public int ErrorCharts { get; set; }
        public int NoOfCodes { get;set;}
        public int NoOfErrorCodes { get;set;}
        public int NoOfNewCodes { get; set; }
        public double Quality {get;set;}
        public double Efficiency {get;set;}
    }

    public class  ProjectAndTeamBasedUser
    {
        [Key]
        public string Id { get; set; }
        public string Value { get; set; }
    }

    
    public class DemographicsDetails : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public string ScanDate { get; set; }
        public string FileName { get; set; }
        public string MemberID { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string ProviderID { get; set; }
        public string ProviderFirstName { get; set; }
        public string ProviderLastName { get; set; }

    }
    public class DemographicsListdetails
    {
        [Key]
        public int Id { get; set; }
        public string ScanDate { get; set; }
        public string FileName { get; set; }
        public string MemberID { get; set; }
        public string MemberFirstName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string ProviderID { get; set; }
        public string ProviderFirstName { get; set; }
    }
    public class PluginDetails
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int PlugInNameId { get; set; }
        public string PlugInName { get; set; }
    }
    public class CoderProductionReport
    {
        [Key]
        //public int ROWID { get; set; }
        public string CoderId { get; set; }
        public string CoderName { get; set; }
        public string Team { get; set; }
        public string Gender { get; set; }
        public int NormsPerDay { get; set; }
        public int OverallDays { get; set; }
        public int OverallHours { get; set; }
        public int OverallCompletedCharts { get; set; }
        public int OverallPendingCharts { get; set; }
        public int OverallNoOfCodes { get; set; }
        public int TotalLastMonthDays { get; set; }
        public int TotalLastMonthHours { get; set; }
        public int CompletedChartsLastMonth { get; set; }
        public int PendingChartsLastMonth { get; set; }
        public int NoOfCodesLastMonth { get; set; }
        public int TotalCurrMonthDays { get; set; }
        public int TotalCurrMonthHours { get; set; }
        public int CompletedChartsCurrMonth { get; set; }
        public int PendingChartsCurrMonth { get; set; }
        public int NoOfCodesCurrMonth { get; set; }
    }

    public class MandatoryFields
    {
        [Key]
        public int Id { get; set; }
        public int SectionID { get; set; }
        public string FieldName { get; set; }
        public bool IsMandatory { get; set; }
        public int ProjectID { get; set; }
    }

    public class UserSessionDetails
    {
        [Key]
        public int Id { get; set; }
        public int Flag { get; set; }
        public string UserID { get; set; }
        public string SessionID { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
    }
}
