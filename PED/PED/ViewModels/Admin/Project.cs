using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WiseX.Models;
using WiseX.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WiseX.ViewModels.Admin
{
    public class Project
    {
        public List<ProjectTypes> projectTypeList;
        public List<ProjectProtocol> projectProtocolList;
        public List<EncryptionDetails> encryptionList;
        public List<CaptureHCCDetails> captureHCCDetailsList;
        public List<ProjectRules> projectRulesList;
        public List<ProjectListDetails> ProjectDetailsList;
        public List<FileTypes> FileTypeList;
        public List<MasterPlugInDetails> MasterPlugInDetailsList;
        public List<ProjectPlugInDetails> ProjectPlugInList;
        public List<CustomFieldSections> CustomFieldSectionsList;
        public List<CustomFieldTypes> CustomFieldTypesList;
        public List<CustomFieldslist> customFieldsList;
        public List<CustomDropdownOptionList> customDropdownOptionList;

        public ProjectDetailsTemp ProjectDetails { get; set; }
        public ProjectInputDetails projectInputDetails { get; set; }
        public ProjectProtocol projectProtocolDetails { get; set; }
        public EncryptionDetails encryptionDetails { get; set; }
        public ProjectDetailsTemp ProjectDetailsTemp { get; set; }
        public ProjectChartVolumes projectChartVolumes { get; set; }
        public ProjectRules projectRules { get; set; }
        public ProjectListDetails projectListDetails { get; set; }
        public MasterPlugInDetails mastersPlugInDetails { get; set; }
        public ProjectPlugInDetails projectPlugInDetails { get; set; }
        public CustomFields  customFields { get; set; }
        public CustomDropdownOption CustomDropdownOption { get; set; }

        public FileTypes FileTypeDetails { get; set; }
        public ProjectUpload ProjectUpload;
        public string DemograhicsTemplateFileURL { get; set; }
        public string ProvidersTemplateFileURL { get; set; }
        public int CustomFieldId { get; set; }
    }

    public class ProjectTypes
    {
        [Key]
        public int Id { get; set; }
        public string ProjectType { get; set; }
    }

    public class FileTypes
    {
        [Key]
        public int Id { get; set; }
        public string InputFileType { get; set; }
        public string OutputFileType { get; set; }
    }

    public class ProjectDetailsTemp : EntityBase
    {
        [Key]
        public int Id { get; set; }
        [Remote("IsProjectNameExsit", "Admin",ErrorMessage ="Project Name is left blank or Already Exist!")]
        public string ProjectName { get; set; }
        public int ProjectType { get; set; }
        public string ProjectDescription { get; set; }
        public int Shore { get; set; }
        //public int NLPIntegrated { get; set; }
        public int Active { get; set; }
        public int PreloadData { get; set; }
    }
    public class ProjectInputDetails : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public int ProtocolId { get; set; }
        public int EncryptionId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DemographicPath { get; set; }
        public string ChartPath { get; set; }
        public string ClaimsPath { get; set; }
        public string EnrollmentDetailsPath { get; set; } 
        public string ProvidersPath { get; set; }
    }

    public class ProjectChartVolumes : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ExpectedVolume { get; set; }
        public string StartDate { get; set; }
        public string ExpectedEndDate { get; set; }
        public string ExpectedQuality { get; set; }
        public string ExpectedEfficiency { get; set; }
        public string ExpectedUsers { get; set; }
        public string InputFileType { get; set; } 
        public string OutputFormat { get; set; }
    }

    public class ProjectRules : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int CaptureDiags { get; set; }
        public int CaptureFrequency { get; set; }
        public int GenderEdit { get; set; }
        public int AgeEdit { get; set; }
        public int PregnancyEdit { get; set; }
        public int NewBornEdit { get; set;}
        public string[] ReviewYear { get; set; }
        public bool Selected { get; set; } = false;
    }
    public class ReviewYear
    {
        public int Year { get; set; }
    }
    public class ProjectProtocol
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
    }
   
    public class ProjectPlugInDetails : EntityBase
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string[] PlugInNameId { get; set; }
        public bool Selected { get; set; } = false;
    }
    public class EncryptionDetails
    { 
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
       // public int ProtocolId { get; set; }
    }

    public class CaptureHCCDetails
    {
        [Key]
        public int Id { get; set; }
        public string CaptureHCC { get; set; }
    }
    public class ProjectListDetails
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectType { get; set; }
        public string ProjectDescription { get; set; }
        public string ExpectedVolume { get; set; }
        public string StartDate { get; set; }
        public string ExpectedEndDate { get; set; }
        public string ExpectedQuality { get; set; }
        public string ReviewYear { get; set; }
        public string PlugIns { get; set; }
        public string Active { get; set; }
        public bool Flag { get; set; }

    }
    public class ProjectUpload :EntityBase
    {
        [Key]
        public int ProjectId { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }
        public string UploadedBy { get; set; }
        public string RefFileName { get; set; }
    }
   public class CustomFields:EntityBase
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string FieldName { get; set; }
        public int FieldType { get; set; }
        public int Section { get; set; }
        public int IsMandatory { get; set; }
        public int Active { get; set; }
        public string UserId { get; set; }
    }
    public class CustomFieldslist 
    {
        [Key]
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public int CustomFieldTypeId { get; set; }
        public string Section { get; set; }
        public int CustomFieldSectionId { get; set; }
        public string IsMandatory { get; set; }
        public int IsMandatoryId { get; set; }
        public string Active { get; set; }
        public int Status { get; set; }
    }
    public class CustomFieldSections
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class CustomFieldTypes
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class CustomDropdownOption:EntityBase
    {
        [Key]
        public int Id { get; set; }
        public int FieldId { get; set; }
        public string Option { get; set; }
        public int Active { get; set; }
        public string UserId { get; set; }

    }
    public class CustomDropdownOptionList
    {
        [Key]
        public int Id { get; set; }
        public string FieldOption { get; set; }
        public int Active { get; set; }
        public string Status { get; set; }

    }
}
