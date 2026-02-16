using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;
using WiseX.Models;
using WiseX.ViewModels.Home;
namespace WiseX.ViewModels.Admin
{
    public class Reports
    {
        // public ReportResult ReportResult { get; set; }
        public ReportSearchFilter ReportSearchFilter { get; set; }
        public CoderDetails CodeDetails { get; set; }
        public CoderNameDetails CoderNameDetails { get; set; }
        public string CoderId;
        public string CoderName;
        public List<CoderDetails> CoderDetailsList { get; set; }
        public List<AuditorDetails> AuditorDetailsList { get; set; }
        // public List<ReportResult> ReportResultList { get; set; }
        public List<ProjectReport> ProjectReportList { get; set; }
        public List<TeamReport> TeamReportList { get; set; }
        public List<ChartReport> ChartReportList { get; set; }
        public List<CoderReport> CoderReportList { get; set; }
        public List<CoderProductionReport> CoderProductionReportList { get; set; }
        public List<ProjectDetailsUsers> ProjectDetailsUsersList;
        public List<CoderNameDetails> CoderNameList { get; set; }
       

        public List<ResourceDetails> ResourceDetailsList;
        public List<Masters_ChartStatus> ChartStatusList;
        public IList<Masters_X_HoldComments> HoldCommentList;
        public List<ProjectAndTeamBasedUser> GetProjectAndTeamBasedUserList;
        public IList<CoderProduction> CoderProductionList;
        public CoderProduction CoderProduction;
        public List<CoderProductivityDetails> CoderProductivityDetailsList;
        public CoderProductivityDetails CoderProductivityDetails;
        public List<CoderQualityDetails> CoderQualityDetailsList;
        public CoderQualityDetails CoderQualityDetails;
        public List<CoderEfficiencyDetails> CoderEfficiencyDetailsList;
        public CoderEfficiencyDetails CoderEfficiencyDetails;
        public List<CoderLast10DaysProduction> CoderLast10DaysProductionList;
        public List<CoderLast10DaysQtyAndEff> CoderLast10DaysQtyAndEffList;
        //Attendance Vs Production
        public ProductionHrsDetails ProductionHrsDetails { get; set; }
        public List<ProductionHrsDetails> ProductionHrsList { get; set; }

        public CompletedChartsDetails CompletedChartsDetails { get; set; }
        public List<CompletedChartsDetails> CompletedChartsList { get; set; }
        public int OverAllRowCount { get; set; }

        public ChartProperties ChartBoxes;
        public CoderChartBoxReport CoderChartBoxes;

        public ProductionOutput ProductionOutput { get; set; }
        public List<ProductionOutput> ProductionOutputList { get; set; }
        public ProjectDropDown ProjectDropDown { get; set; }
        public List<ProjectDropDown> ProjectDropDownList { get; set; }
        public string IsCoder = "false";
        public int ProjectID = 0;

        public List<GetProjectTeams> GetProjectTeamsList { get; set; }
    }

    public class CoderProduction
    {
        [Key]
        public int ProductionAverage { get; set; }
        public double Quality { get; set; }
        public double Efficiency { get; set; }
    }
    public class  CoderProductivityDetails
    {
        [Key]
        public int OverAll { get; set; }
        public string Last30Days { get; set; }
        public int Last30Production { get; set; }
        public string Last7Days { get; set; }
        public int Last7DaysProduction { get; set; }
        public int Yesterday { get; set; }
        public int YesterdayProduction { get; set; }
        public string CoderGrowthPercent { get; set; }


    }
    public class CoderQualityDetails
    {
        [Key]
        public int OverAll { get; set; }
        public string Last30Days { get; set; }
        public int Last30Production { get; set; }
        public string Last7Days { get; set; }
        public int Last7DaysProduction { get; set; }
        public int Yesterday { get; set; }
        public int YesterdayProduction { get; set; }
        public string CoderGrowthPercent { get; set; }

    }
    public class CoderEfficiencyDetails
    {
        [Key]
        public int OverAll { get; set; }
        public string Last30Days { get; set; }
        public int Last30Production { get; set; }
        public string Last7Days { get; set; }
        public int Last7DaysProduction { get; set; }
        public int Yesterday { get; set; }
        public int YesterdayProduction { get; set; }
        public string CoderGrowthPercent { get; set; }

    }
    public class CoderLast10DaysProduction
    {
        [Key]
        public string Last10Days { get; set; }
        public int Completedcharts { get; set; }
        public int NormsPerDay { get; set; }
        public string Flag { get; set; }
    }
    public class CoderLast10DaysQtyAndEff
    {
        [Key]
        public string Last10Days { get; set; }
        public int OverAllCodes { get; set; }
        public int OverAllErrorCodes { get; set; }
        public int NoOfMissedCodes { get; set; }
        public int Quality { get; set; }
        public int Efficiency { get; set; }
        public string Flag { get; set; }
    }

    public class CoderNameDetails
    {
        [Key]
        public string CoderID { get; set; }
        public string CoderName { get; set; }
    }

    public class ProductionOutput
    {   
        [Key]
        public int ROWID { get; set; }
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int ResourceID { get; set; }
        public string ResourceName { get; set; }
        public string ScanDate { get; set; }
        public string CoderID { get; set; }
        public string CoderName { get; set; }
        public int ChartID { get; set; }
        public int DosID { get; set; }
        public int DiagID { get; set; }
        public string Mode { get; set; }
        public string FileName { get; set; }
        public string MemberName { get; set; }
        public string FromDOS { get; set; }
        public string ToDOS { get; set; }
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string Diag { get; set; }
        public string PageNo { get; set; }
        public string POS { get; set; }
        public string PrimaryComment { get; set; }
        public string SecondaryComment { get; set; }
        
    }
}
