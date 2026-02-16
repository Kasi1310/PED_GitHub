using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WiseX.Models;
using WiseX.ViewModels.Admin;
using WiseX.Helpers;

namespace WiseX.ViewModels.Home
{
    public class HomeViewModel
    {
        public ChartProperties ChartBoxes;
        public IList<ChartProperties> ChartPropertiesList;

        public ChartBoxProperties ChartBoxProperty;
        public IList<ChartBoxProperties> ChartBoxPropertiesList;
        public IList<ChartBoxPropertiesLoad> ChartBoxPropertiesLoad;

        public IList<PECredentialingLicenseBoxesList> PECredentialingLicenseBoxesList;
        public IList<PEContractBoxesList> PEContractBoxesList;

        public IList<Topcoders> Topcoders;
        public IList<ReceivedvsCompleted> ReceivedvsCompleted;
        public IList<ProgressTracker> ProgressTracker;
        public IList<ChartStatus> ChartStatus;
        public IList<ErrorCharts> ErrorCharts;
        public IList<ProductionLastMonth> ProductionLastMonth;
        public IList<OverdueLastMonth> OverdueLastMonth;
        public List<Daycount> Daycount { get; set; }
        public List<Daymonth> Daymonth { get; set; }
        public List<TopCoderstrend> TopCoderstrend { get; set; }
        public List<AttendanceTracker> OverallAttendanceTrackerList { get; set; }

        public List<ProductionHrsDetails> ProductionHrsList { get; set; }
        public List<CompletedChartsDetails> CompletedChartsList { get; set; }

        public List<CoderProductivityDetails> CoderProductivityDetailsList;
        public CoderProductivityDetails CoderProductivityDetails;

        public List<CoderQualityDetails> CoderQualityDetailsList;
        public CoderQualityDetails CoderQualityDetails;

        public List<CoderEfficiencyDetails> CoderEfficiencyDetailsList;
        public CoderEfficiencyDetails CoderEfficiencyDetails;

        public List<AttendanceTracker> AttendanceTrackerList;
        public AttendanceTracker AttendanceTrackerDetails;

        public List<CoderChartBoxReport> CoderChartBoxReportList;
        public CoderChartBoxReport CoderChartBoxReport;

        public List<PECheckListBoxes> PECheckListBoxes;
        public List<PECheckListBoxesFroAllocatedUser> PECheckListBoxesFroAllocatedUser;

        public string RoleName { get; set; }
        public int OverAllCount { get; set; }
        public int OverAllRowCount { get; set; }
        public HomeViewModel()
        {
            Daycount = new List<Daycount>();
            Daymonth = new List<Daymonth>();
            ProductionHrsList = new List<ProductionHrsDetails>();
            CompletedChartsList = new List<CompletedChartsDetails>();
            TopCoderstrend = new List<TopCoderstrend>();
        }

        public TeamAttendance TeamAttendance;
        public List<TeamAttendance> TeamAttendancesList;

        public TeamCompletedChart TeamCompletedChart;
        public List<TeamCompletedChart> TeamCompletedChartsList;

        public List<UserProjectRole> UserProjectRoleList;
    }
    public class ChartProperties
    {
        [Key]
        public int AgreementsExpire { get; set; }
        public int AgreementsOutstanding { get; set; }
        public int AgreementsReturned { get; set; }
        public int ContractsSenToAE { get; set; }
        public int ContractsReturnedbyAEtoManagement { get; set; }
        public int ContractSentToClient { get; set; }
        public int FeeChangedClients { get; set; }
        public int ActiveClientsWillNotSignNewContracts{ get; set; }
        public int AutoRenewalClients { get; set; }
        public int LicenseExpire { get; set; }
        public int LicenseOutstanding { get; set; }
        public int MedicareContractExpire { get; set; }
        public int MedicareOutstanding { get; set; }
        public int MedicaidContractExpire { get; set; }
        public int MedicaidOutstanding { get; set; }
        public int OpenTask { get; set; }
        public int ClosedTask { get; set; }
        public int PamelaRoysdonTask { get; set; }
        public int AmyKingTask { get; set; }
        public int BillyWhiteTask { get; set; }


    }

    public class ChartBoxProperties
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ContractSignedDate { get; set; }
        public int ClientId { get; set; }
        public int ContractId { get; set; }
        public string AggrementType { get; set; }
        public string Status { get; set; }
    }
    public class ChartBoxPropertiesLoad
    {
        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ContractExpiryDate { get; set; }
        public int ClientId { get; set; }
        public int ContractId { get; set; }
        public string AggrementType { get; set; }
        public string Status { get; set; }
        public int TotalCount { get; set; }
        public string ResidencyCode { get; set; }
        public string Notes { get; set; }
        public string Fees { get; set; }
    }

    public class Daycount
    {
        [Key]
        public long Day { get; set; }
        public int Count { get; set; }

    }
    public class Daymonth
    {
        [Key]
        public long Day { get; set; }
        public string Month { get; set; }
    }
    public class Topcoders
    {
        [Key]
        public string UserName { get; set; }
        public int Totalcharts { get; set; }
        public int Pending { get; set; }
        public int Hold { get; set; }
        public int Completed { get; set; }
    }


    public class ReceivedvsCompleted
    {
        [Key]
        public string Month { get; set; }
        public int Received { get; set; }
        public int Completed { get; set; }
        public int MonthNumber { get; set; }
        //public int Year { get; set; }
    }
    public class ProgressTracker
    {
        [Key]
        public string Month { get; set; }
        public int Execpted { get; set; }
        public int Actual { get; set; }
        public int MonthNumber { get; set; }
        //public int Year { get; set; }
    }
    public class ChartStatus
    {
        [Key]
        public string ProjectName { get; set; }
        public int New { get; set; }
        public int InProgress { get; set; }
        public int Hold { get; set; }
        //public int SaveDraft { get; set; }
        public int ReviewCompleted { get; set; }
        public int AuditCompleted { get; set; }
        public int SuperAuditCompleted { get; set; }
        public int Archival { get; set; }
    }

    public class ErrorCharts
    {
        [Key]
        public string Project { get; set; }
        public int Error { get; set; }
        public int Education { get; set; }
    }
    public class ProductionLastMonth
    {
        [Key]
        public string Project { get; set; }
        public int Production { get; set; }
    }
    public class OverdueLastMonth
    {
        [Key]
        public string Project { get; set; }
        public int Overdue { get; set; }
    }
    public class BroadcastRecevierDetails
    {
        [Key]
        public int RID { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        public string SentBy { get; set; }
        public string SentOn { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }
        public byte IsRead { get; set; }
        public string ReadOn { get; set; }
        public string SentTime { get; set; }
        public bool Flag { get; set; }
    }

    public class TopCoderstrend
    {
        [Key]
        public int Day { get; set; }
        public string UserName { get; set; }
        public int counts { get; set; }
    }

    public class AttendanceTracker
    {
        [Key]
        public string Date { get; set; }
        public int WorkingHours { get; set; }
    }

    public class CompletedChartsDetails
    {
        [Key]
        public string Date { get; set; }
        public int CompletedCharts { get; set; }

    }

    public class ProductionHrsDetails
    {
        [Key]
        public string Date { get; set; }
        public int ProductionHrs { get; set; }
    }

    public class CoderChartBoxReport
    {
        [Key]
        public int TotalCharts { get; set; }
        public int CompletedCharts { get; set; }
        public int PendingCharts { get; set; }
        public int InProgressCharts { get; set; }
        public int New { get; set; }
    }
    public class TeamAttendance
    {
        [Key]
        public string Name { get; set; }
        public int ActualDaysPerMonth { get; set; }
        public int PresentLastMonth { get; set; }
        public int PresentCurrentMonth { get; set; }
    }
    public class TeamCompletedChart
    {
        [Key]
        public string UserID { get; set; }
        public string Name { get; set; }
        public int CompletedCharts { get; set; }
    }
    public class NotificationCount
    {
        [Key]
        public int NCount { get; set; }
    }

    public class PECredentialingLicenseBoxesList
    {
        [Key]
        public int ClientId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string LicenseType { get; set; }
        public string CertificationLevel { get; set; }
        public string LicenseNo { get; set; }
        public string IssuedDate { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }
        public int TotalCount { get; set; }
    }

    public class PEContractBoxesList
    {
        [Key]
        public int ClientId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string InsuranceContract { get; set; }
        public string LastRevalidateDate { get; set; }
        public string EffectiveDate { get; set; }
        public string NextUpdateDue { get; set; }
        public string TermedDate { get; set; }
        public int TotalCount { get; set; }
    }

    public class PECheckListBoxes
    {
        [Key]
        public int ClientId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BillingType { get; set; }
        public string Task { get; set; }
        public string Status { get; set; }
        public int TotalCount { get; set; }
        public string AllocatedUser { get; set; }
    }

    public class PECheckListBoxesFroAllocatedUser
    {
        [Key]
        public int ClientId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string BillingType { get; set; }
        public string Task { get; set; }
        public string Status { get; set; }
        public int TotalCount { get; set; }
        public string AllocatedUser { get; set; }
    }
}
