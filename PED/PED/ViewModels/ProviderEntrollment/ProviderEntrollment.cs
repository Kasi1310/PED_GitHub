using Microsoft.AspNetCore.Http;
using PED.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;
using static System.Net.Mime.MediaTypeNames;

namespace PED.ViewModels.ProviderEntrollment
{
    public class ProviderEntrollment : EntityBase
    {
        [Key]
        public string Id { get; set; }

        public ProviderEntrollmentDetails ProviderEntrollmentDetails;
        public List<ProviderEntrollmentDetailsList> ProviderEntrollmentDetailsList;
        public List<CitiesList> CitiesList;
        public List<StatesList> StatesList;
        public List<ResidencyCodeList> ResidencyCodeList;
        public List<PEStatusList> PEStatusList;
        public List<AccountExecutiveList> AccountExecutiveList;
        public List<PEHoursofOperationList> PEHoursofOperationList;
        public List<PETaxClassificationList> PETaxClassificationList;
        public List<PEPaymentIndicatorList> PEPaymentIndicatorList;
        public List<PETaxonomyList> PETaxonomyList;
        public List<InsuranceContractList> InsuranceContractList;
        public List<PEAddressTypeList> PEAddressTypeList;
        public List<PEContactTypeList> PEContactTypeList;
        public List<PELicenseTypeList> PELicenseTypeList;
        public List<PECertificationLevelList> PECertificationLevelList;
        public List<PEDocumentTitleList> PEDocumentTitleList;
        public List<PESearchClient> SearchPEClientList;

        public List<PEUsersRoleMailID> PEUsersRoleMailIDList;

        public PECheckList PECheckList;

        public PEPaymentCategoryChangeCheckList PEPaymentCategoryChangeCheckList;
        public PESinglePayerUpdateCheckList PESinglePayerUpdateCheckList;
        public PEPracticeLocationChangeCheckList PEPracticeLocationChangeCheckList;
        public PEClosedClientCheckList PEClosedClientCheckList;
        public PEClientBankChangeCheckList PEClientBankChangeCheckList;
        public PECommericalEFTsCheckList PECommericalEFTsCheckList;
        public PEMedicareChangeOfInfoCheckList PEMedicareChangeOfInfoCheckList;
        public PEOutOfStateMCDCheckList PEOutOfStateMCDCheckList;

        public List<PESinglePayerUpdateCheckList> ListOfPESinglePayerUpdateCheckList;

        //public PEDetailsInsert PEDetailsInsert;
        public PEDetails PEDetails;
        public List<PEDetailsList> PEDetailsList;

        public PEInsuranceContracts PEInsuranceContracts;

        

        public List<PEOtherAddressList> PEOtherAddressList;
        public PEOtherAddress PEOtherAddress;


        public List<PEContactsList> PEContactsList;
        public PEContacts PEContacts;

        public List<PERefusedEnrollmentList> PERefusedEnrollmentList;
        public PERefusedEnrollment PERefusedEnrollment;

        public List<PEEFTList> PEEFTList;
        public PEEFT PEEFT;

        public List<PEDocumentsList> PEDocumentsList;
        public PEDocuments PEDocuments;

        public List<PECredentialingLicenseList> PECredentialingLicenseList;
        public PECredentialingLicense PECredentialingLicense;


        public List<PENotesList> PENotesList;
        public PENotes PENotes;

        public List<PEAuditLog> PEAuditLogList;

        public List<SearchClientNames> SearchClientNames;

        public InstructionsDocument InstructionsDocument;
        public List<InstructionsDocument> InstructionsDocumentList;

        public List<PEInsuranceContractFileDetails> PEInsuranceContractFileDetailsList;

        public List<PEEPCRList> PEEPCRList;
        public List<PEEnrollmentRepresentativeStatusList> PEEnrollmentRepresentativeStatusList;

        


    }

    public class ProviderEntrollmentDetails : EntityBase
    {
        [Key]
        public int ID { get; set; }
        public string CompanyId { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string NPINumber { get; set; }
        public string TaxID { get; set; }
        public string CollectionsCompanyName { get; set; }
        public string PayToName { get; set; }
        public string PayToAddressLine1 { get; set; }
        public string PayToAddressLine2 { get; set; }
        public int PayToCityId { get; set; }
        public int PayToStateId { get; set; }
        public string PayToZipCode { get; set; }
        public string BillingProviderName { get; set; }
        public string BillingProviderAddressLine1 { get; set; }
        public string BillingProviderAddressLine2 { get; set; }
        public int BillingProviderCityId { get; set; }
        public int BillingProviderStateId { get; set; }
        public string BillingProviderZipCode { get; set; }
        public string LastDOS { get; set; }
        public string ContractEndDate { get; set; }
        public string CreatedBy { get; set; }
    }
    public class ProviderEntrollmentDetailsList
    {
        [Key]
        public int ID { get; set; }
        public string CompanyId { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string NPINumber { get; set; }
        public string TaxID { get; set; }
        public string CollectionsCompanyName { get; set; }
        public string PayToName { get; set; }
        public string PayToAddressLine1 { get; set; }
        public string PayToAddressLine2 { get; set; }
        public int PayToCityId { get; set; }
        public int PayToStateId { get; set; }
        public string PayToZipCode { get; set; }
        public string BillingProviderName { get; set; }
        public string BillingProviderAddressLine1 { get; set; }
        public string BillingProviderAddressLine2 { get; set; }
        public int BillingProviderCityId { get; set; }
        public int BillingProviderStateId { get; set; }
        public string BillingProviderZipCode { get; set; }
        public string LastDOS { get; set; }
        public string ContractEndDate { get; set; }
        public string PayToCity { get; set; }
        public string PayToState { get; set; }
        public string BillingProviderCity { get; set; }
        public string BillingProviderState { get; set; }
    }

    public class DownloadProviderEntrollmentDetailsList
    {
        [Key]
        public string CompanyId { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string NPINumber { get; set; }
        public string TaxID { get; set; }
        public string CollectionsCompanyName { get; set; }
        public string PayToName { get; set; }
        public string PayToAddressLine1 { get; set; }
        public string PayToAddressLine2 { get; set; }
        public string PayToCity { get; set; }
        public string PayToState { get; set; }
        public string PayToZipCode { get; set; }
        public string BillingProviderName { get; set; }
        public string BillingProviderAddressLine1 { get; set; }
        public string BillingProviderAddressLine2 { get; set; }
        public string BillingProviderCity { get; set; }
        public string BillingProviderState { get; set; }
        public string BillingProviderZipCode { get; set; }
        public string LastDOS { get; set; }
        public string ContractEndDate { get; set; }
    }

    public class PEDetails
    {
        [Key]
        public int ID { get; set; }
        public int ClientDetailsID { get; set; }
        public string CompanyId { get; set; }
        public string ClientName { get; set; }
        public int StatusID { get; set; }
        public int ResidencyIndicatorID { get; set; }
        public int PaymentIndicatorID { get; set; }
        public string DoingBusinessAs { get; set; }
        public string OtherName { get; set; }
        public int HoursofOperationID { get; set; }
        public string AccountRepresentativeID { get; set; }
        public string Opened { get; set; }
        public string Closed { get; set; }
        public string PracticeLocation { get; set; }
        public string AddressLine2 { get; set; }
        public string County { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Zip { get; set; }
        public string Phone1 { get; set; }
        public string Ext1 { get; set; }
        public string Phone2 { get; set; }
        public string Ext2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }
        public string TaxID { get; set; }
        public string TaxIDIssued { get; set; }
        public int TaxIDClassificationID { get; set; }
        public int TaxonomyID { get; set; }
        public string GroupNPI { get; set; }
        public string GroupNPIIssued { get; set; }
        public string MedicaidNumber { get; set; }
        public string MedicaidNumberIssued { get; set; }
        public string MedicareNumber { get; set; }
        public string MedicareNumberIssued { get; set; }
        public string GroupPTAN { get; set; }
        public string GroupPTANIssued { get; set; }
        public string WPPolicyNumber { get; set; }
        public string InitialBillingSetup { get; set; }
        public string DateOf1stTreatedPatient { get; set; }
        public string PreviousBiller { get; set; }
        public string PreviousBillerContact { get; set; }
        public string GovBankNameEFT { get; set; }
        public string GovBankRoutingNumber { get; set; }
        public string GovBankAccountNumber { get; set; }
        public string GovBankLetterFileName { get; set; }
        public string GovBankLetterOrgFileName { get; set; }
        public string PrevBankFileName { get; set; }
        public string PrevBankOrgFileName { get; set; }
        public string SendToCollections { get; set; }
        public string CollectionAgency { get; set; }
        public bool IsNonEmergencies { get; set; }
        public bool IsNonTransports { get; set; }
        public string NonGovBankNameEFT { get; set; }
        public string NonGovBankRoutingNumber { get; set; }
        public string NonGovBankAccountNumber { get; set; }
        public string NonGovBankLetterFileName { get; set; }
        public string NonGovBankLetterOrgFileName { get; set; }
        public string FirstRunDate { get; set; }
        public string AccountExecutiveMailID { get; set; }
        public string SupervisorMailID { get; set; }
        public string WaystarAdminMailID { get; set; }
        public string ClientMailID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCheckList { get; set; }
        public string CommericalEFTsCheckListPayer { get; set; }
        public int EPCRStatus { get; set; }
        public int EnrollmentRepresentativeStatus  { get; set; }
    }

    public class PEDetailsList
    {
        [Key]
        public int ID { get; set; }
        public int ClientDetailsID { get; set; }
        public string ClientID { get; set; }
        public string ClientName { get; set; }
        public int StatusID { get; set; }
        public string Status { get; set; }
        public int ResidencyIndicatorID { get; set; }
        public string ResidencyIndicator { get; set; }
        public int PaymentIndicatorID { get; set; }
        public string PaymentIndicator { get; set; }
        public string DoingBusinessAs { get; set; }
        public string OtherName { get; set; }
        public int HoursofOperationID { get; set; }
        public string HoursofOperation { get; set; }
        public string AccountRepresentativeID { get; set; }
        public string AccountRepresentativeName { get; set; }
        public string Opened { get; set; }
        public string Closed { get; set; }
        public string PracticeLocation { get; set; }
        public string AddressLine2 { get; set; }
        public string County { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone1 { get; set; }
        public string Ext1 { get; set; }
        public string Phone2 { get; set; }
        public string Ext2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }
        public string TaxID { get; set; }
        public string TaxIDIssued { get; set; }
        public int TaxIDClassificationID { get; set; }
        public string TaxIDClassification { get; set; }
        public int TaxonomyID { get; set; }
        public string Taxonomy { get; set; }
        public string GroupNPI { get; set; }
        public string GroupNPIIssued { get; set; }
        public string MedicaidNumber { get; set; }
        public string MedicaidNumberIssued { get; set; }
        public string MedicareNumber { get; set; }
        public string MedicareNumberIssued { get; set; }
        public string GroupPTAN { get; set; }
        public string GroupPTANIssued { get; set; }
        public string WPPolicyNumber { get; set; }
        public string InitialBillingSetup { get; set; }
        public string DateOf1stTreatedPatient { get; set; }
        public string PreviousBiller { get; set; }
        public string PreviousBillerContact { get; set; }
        public string GovBankNameEFT { get; set; }
        public string GovBankRoutingNumber { get; set; }
        public string GovBankAccountNumber { get; set; }
        public string GovBankLetterFileName { get; set; }
        public string GovBankLetterOrgFileName { get; set; }
        public string PrevBankFileName { get; set; }
        public string PrevBankOrgFileName { get; set; }
        public string SendToCollections { get; set; }
        public string CollectionAgency { get; set; }
        public bool IsNonEmergencies { get; set; }
        public bool IsNonTransports { get; set; }
        public string NonGovBankNameEFT { get; set; }
        public string NonGovBankRoutingNumber { get; set; }
        public string NonGovBankAccountNumber { get; set; }
        public string NonGovBankLetterFileName { get; set; }
        public string NonGovBankLetterOrgFileName { get; set; }
        public string FirstRunDate { get; set; }
        public string ClientAddedToESOCompany { get; set; }
        public string MedicareSubmitApplicationThruPecos { get; set; }
        public string WaystarSubmitTicketToCreateChildAccount { get; set; }
        public string WaystarProviderSetup { get; set; }
        public string WaystarEnrollments { get; set; }
        public string MedicareEDISubmitApplication { get; set; }
        public string BCBSRequestToJoinNetwork { get; set; }
        public string MMORequestToJoinNetwork { get; set; }
        public string AetnaNPISubmission { get; set; }
        public string MedicaidSubmitApplication { get; set; }
        public string MedicaidSubmitERAAndOrEDI { get; set; }
        public string TricareEastSubmitApplication { get; set; }
        public string RailroadMedicareApplyForPTAN { get; set; }
        public string RailroadMedicareSubmitERAAndOrEDI { get; set; }
        public string OHBWCSubmitMedco13Application { get; set; }
        public string UHCRequestForNonParProvider { get; set; }
        public string VAVendorForm { get; set; }
       // public string EPCRStatus { get; set; }
        public string EnrollmentRepresentativeStatus { get; set; }

    }

    public class PEContracts
    {
        [Key]
        public int Id { get; set; }
        public string ClientID { get; set; }
    }
    public class PEStatusList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class PEEPCRList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class PEEnrollmentRepresentativeStatusList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class PEHoursofOperationList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class PETaxClassificationList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class PEPaymentIndicatorList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class PETaxonomyList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class PEAddressTypeList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class PEContactTypeList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
    public class PELicenseTypeList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class PECertificationLevelList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }    
    public class PEDocumentTitleList
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class PEInsuranceContracts
    {
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public int InsuranceContractId { get; set; }
        public string Info { get; set; }
        public string LastRevalidateDate { get; set; }
        public string EffectiveDate { get; set; }
        public string NextUpdateDue { get; set; }
        public string TermedDate { get; set; }
        public string ProviderNo { get; set; }
        public string GroupNo { get; set; }
        public int Active { get; set; }

    }

    

    public class PEOtherAddress
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public int AddressTypeID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class PEOtherAddressList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int AddressTypeID { get; set; }
        public string AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class PEContacts
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public int ContactTypeID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }
    public class PERefusedEnrollment
    {
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string RefusedEnrollmentInsuranceName { get; set; }
        public string RefusedEnrollmentStatus { get; set; }
        public string RefusedEnrollmentComments { get; set; }

    }

    public class PERefusedEnrollmentList
    {
        [Key]
        public int Id { get; set; }
       
        public string RefusedEnrollmentInsuranceName { get; set; }
        public string RefusedEnrollmentStatus { get; set; }
        public string RefusedEnrollmentComments { get; set; }
        
    }

    public class PEEFT
    {
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string EFTPayerName { get; set; }
        public string EFTBankType { get; set; }
        public string EFTComments { get; set; }

    }
    public class PEEFTList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string CompanyName { get; set; }
        public string EFTPayerName { get; set; }
        public string EFTBankType { get; set; }
        public string EFTComments { get; set; }

    }


    public class PEContactsList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public int ContactTypeID { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string ContactType { get; set; }
        public string Title { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }

    public class PEDocuments
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public int TitleID { get; set; }
        public string TitleName { get; set; }
        //public string Document { get; set; }
        //public string AddedOn { get; set; }
    }
    public class PEDocumentsList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string DocumentName { get; set; }
        public string AddedOn { get; set; }
        public string FileName { get; set; }
        public bool IsAdminOnly { get; set; }
    }

    public class PECredentialingLicenseList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int LicenseTypeID { get; set; }
        public string LicenseType { get; set; }
        public int CertificationLevelID { get; set; }
        public string CertificationLevel { get; set; }
        public string LicenseNo { get; set; }
        public string IssuedDate { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }
    }
    public class PECredentialingLicense
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public int LicenseTypeID { get; set; }
        public int CertificationLevelID { get; set; }
        public string LicenseNo { get; set; }
        public string IssuedDate { get; set; }
        public string EffectiveDate { get; set; }
        public string ExpiryDate { get; set; }
    }

    public class PESearchClient
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class PECheckList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string ClientAddedToESOCompanyStatus { get; set; }
        public string ClientAddedToESOCompanyStatusAllocatedTo { get; set; }
        public string MedicareSubmitApplicationThruPecosStatus { get; set; }
        public string MedicareSubmitApplicationThruPecosStatusAllocatedTo { get; set; }
        public string WaystarSubmitTicketToCreateChildAccountStatus { get; set; }
        public string WaystarSubmitTicketToCreateChildAccountStatusAllocatedTo { get; set; }
        public string WaystarProviderSetupStatus { get; set; }
        public string WaystarProviderSetupStatusAllocatedTo { get; set; }
        public string WaystarEnrollmentsStatus { get; set; }
        public string WaystarEnrollmentsStatusAllocatedTo { get; set; }
        public string MedicareEDISubmitApplicationStatus { get; set; }
        public string MedicareEDISubmitApplicationStatusAllocatedTo { get; set; }
        public string BCBSRequestToJoinNetworkStatus { get; set; }
        public string BCBSRequestToJoinNetworkStatusAllocatedTo { get; set; }
        public string MMORequestToJoinNetworkStatus { get; set; }
        public string MMORequestToJoinNetworkStatusAllocatedTo { get; set; }
        public string AetnaNPISubmissionStatus { get; set; }
        public string AetnaNPISubmissionStatusAllocatedTo { get; set; }
        public string MedicaidSubmitApplicationStatus { get; set; }
        public string MedicaidSubmitApplicationStatusAllocatedTo { get; set; }
        public string MedicaidSubmitERAAndOrEDIStatus { get; set; }
        public string MedicaidSubmitERAAndOrEDIStatusAllocatedTo { get; set; }
        public string TricareEastSubmitApplicationStatus { get; set; }
        public string TricareEastSubmitApplicationStatusAllocatedTo { get; set; }
        public string RailroadMedicareApplyForPTANStatus { get; set; }
        public string RailroadMedicareApplyForPTANStatusAllocatedTo { get; set; }
        public string RailroadMedicareSubmitERAAndOrEDIStatus { get; set; }
        public string RailroadMedicareSubmitERAAndOrEDIStatusAllocatedTo { get; set; }
        public string OHBWCSubmitMedco13ApplicationStatus { get; set; }
        public string OHBWCSubmitMedco13ApplicationStatusAllocatedTo { get; set; }
        public string UHCRequestForNonParProviderStatus { get; set; }
        public string UHCRequestForNonParProviderStatusAllocatedTo { get; set; }
        public string VAVendorFormStatus { get; set; }
        public string VAVendorFormStatusAllocatedTo { get; set; }
        public string NPPESStatus { get; set; }
        public string NPPESStatusAllocatedTo { get; set; }
        public string TricareEastSubmitApplicationIfNotEnrolledStatus { get; set; }
        public string RailroadMedicareApplyIfNotEnrolledOrUpdatePayToAddressStatus { get; set; }
        public string OHBWCOHClientsOnlyIfNotEnrolledSubmitMedco13ApplicationStatus { get; set; }
        public string BCBSParstatusAndRequestConfidentialityAgreementStatus { get; set; }
        public string MMOsendPIFtoMMStatus { get; set; }
        public string TricareEastSubmitApplicationIfNotEnrolledStatusAllocatedTo { get; set; }
        public string RailroadMedicareApplyIfNotEnrolledOrUpdatePayToAddressStatusAllocatedTo { get; set; }
        public string OHBWCOHClientsOnlyIfNotEnrolledSubmitMedco13ApplicationStatusAllocatedTo { get; set; }
        public string BCBSParstatusAndRequestConfidentialityAgreementStatusAllocatedTo { get; set; }
        public string MMOsendPIFtoMMStatusAllocatedTo { get; set; }
    }

    public class PEPaymentCategoryChangeCheckList
    {
        [Key]
        public int ID { get; set; }
        public int ClientDetailsID { get; set; }
        public string ESOAddressAndPaymentCategoryUpdate { get; set; }
        public string ESOAddressAndPaymentCategoryUpdateAllocatedTo { get; set; }
        public string PEDAddressUpdate { get; set; }
        public string PEDAddressUpdateAllocatedTo { get; set; }
        public string NotifyPaymentTeamToStartAddressChange { get; set; }
        public string NotifyPaymentTeamToStartAddressChangeAllocatedTo { get; set; }
        public string MedicareChangeOfInformationApplication { get; set; }
        public string MedicareChangeOfInformationApplicationAllocatedTo { get; set; }
        public string MedicaidMaintenanceApplication { get; set; }
        public string MedicaidMaintenanceApplicationAllocatedTo { get; set; }
        public string RailroadMedicareFaxRequestWithW9 { get; set; }
        public string RailroadMedicareFaxRequestWithW9AllocatedTo { get; set; }
        public string Availity { get; set; }
        public string AvailityAllocatedTo { get; set; }
        public string UpdateCommericalEFTs { get; set; }
        public string UpdateCommericalEFTsAllocatedTo { get; set; }
    }
    public class PESinglePayerUpdateCheckList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string TaskName { get; set; }
        public string TaskValue { get; set; }
        public string NotifyAR { get; set; }
        public string TaskValueAllocatedTo { get; set; }
    }
    public class PEPracticeLocationChangeCheckList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string ESOAddressUpdate { get; set; }

        public string ESOAddressUpdateAllocatedTo { get; set; }
        public string ZOHONotifyAEToUpdateZOHO { get; set; }

        public string ZOHONotifyAEToUpdateZOHOAllocatedTo { get; set; }
        public string PEDAddressUpdate { get; set; }

        public string PEDAddressUpdateAllocatedTo { get; set; }
        public string MedicareChangeOfInformationApplication { get; set; }

        public string MedicareChangeOfInformationApplicationAllocatedTo { get; set; }
        public string MedicaidMaintenanceApplication { get; set; }
        public string MedicaidMaintenanceApplicationAllocatedTo { get; set; }
        public string RailroadMedicareFaxRequestWithW9 { get; set; }
        public string RailroadMedicareFaxRequestWithW9AllocatedTo { get; set; }
        public string Availity { get; set; }
        public string AvailityAllocatedTo { get; set; }
        public string NPPESWebsite { get; set; }
        public string NPPESWebsiteAllocatedTo { get; set; }
        public string Waystar { get; set; }
        public string WaystarAllocatedTo { get; set; }
        public string ContractPayerBCBS { get; set; }
        public string ContractPayerBCBSAllocatedTo { get; set; }
        public string ContractPayerMMO { get; set; }
        public string ContractPayerMMOAllocatedTo { get; set; }
        public string ContractPayerOHBWC { get; set; }
        public string ContractPayerOHBWCAllocatedTo { get; set; }
        public string ContractPayerTricare { get; set; }
        public string ContractPayerTricareAllocatedTo { get; set; }
        public string ContractPayerOther { get; set; }
        public string ContractPayerOtherAllocatedTo { get; set; }
        public string MedicaidApplicationTask { get;set; }
        public string MedicaidApplicationTaskAllocatedTo { get;set; }        
        public string NPPES { get;set; }
        public string NPPESAllocatedTo { get; set; }

    }
    public class PEClosedClientCheckList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string NotifyTeam { get; set; }
        public string NotifyTeamAllocatedTo { get; set; }
    }
    public class PEClientBankChangeCheckList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string ReviewObtainBankLetter { get; set; }
        public string ReviewObtainBankLetterAllocatedTo { get; set; }
        public string PEDUpdateBankInfo { get; set; }
        public string PEDUpdateBankInfoAllocatedTo { get; set; }
        public string MedicareEFTApplication { get; set; }
        public string MedicareEFTApplicationAllocatedTo { get; set; }
        public string MedicaidMaintenanceApplication { get; set; }
        public string MedicaidMaintenanceApplicationAllocatedTo { get; set; }
        public string RailroadMedicareEFTNotificationEmail { get; set; }
        public string RailroadMedicareEFTNotificationEmailAllocatedTo { get; set; }
        public string UpdateCommercialEFTs { get; set; }
        public string UpdateCommercialEFTsAllocatedTo { get; set; }
    }

    public class PECommericalEFTsCheckList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string UpdateBCBS { get; set; }
        public string UpdateUHCOptum { get; set; }
        public string UpdateHumana { get; set; }
        public string UpdateAetnaTricareEastTricareForLife { get; set; }
        public string UpdateMMO { get; set; }
        public string UpdateECHO { get; set; }
        public string UpdatePayspan { get; set; }
        public string UpdateInstamed { get; set; }
        public string UpdateOther { get; set; }
        public string Payer { get; set; }
    }

    public class PEMedicareChangeOfInfoCheckList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string ZOHONotifyAEToUpdateZOHO { get; set; }
        public string ZOHONotifyAEToUpdateZOHOAllocatedTo { get; set; }
        public string PEDUpdateInfo { get; set; }
        public string PEDUpdateInfoAllocatedTo { get; set; }
        public string MedicareChangeOfInformationApplication { get; set; }
        public string MedicareChangeOfInformationApplicationAllocatedTo { get; set; }
        public string MedicaidMaintenanceApplication { get; set; }
        public string MedicaidMaintenanceApplicationAllocatedTo { get; set; }
        public string NPPESWebsite { get; set; }
        public string NPPESWebsiteAllocatedTo { get; set; }
    }
    public class PEOutOfStateMCDCheckList
    {
        [Key]
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string InquireWithClientAboutOOSEnrollment { get; set; }
        public string InquireWithClientAboutOOSEnrollmentAllocatedTo { get; set; }
        public string ObtainNecessaryDocumentsToStartEnrollment { get; set; }
        public string ObtainNecessaryDocumentsToStartEnrollmentAllocatedTo { get; set; }
        public string MedicaidSubmitApplication { get; set; }
        public string MedicaidSubmitApplicationAllocatedTo { get; set; }
        public string MedicaidSubmitERAAndOrEDI { get; set; }
        public string MedicaidSubmitERAAndOrEDIAllocatedTo { get; set; }
        public string NotifyAR { get; set; }
        public string NotifyARAllocatedTo { get; set; }
    }

    public class PEUsersRoleMailID
    {
        [Key]
        public string Id { get; set; }
        public string MailID { get; set; }
    }

    public class PEAuditLog
    {
        [Key]
        public int Id { get; set; }
        public string Changes { get; set; }
        public string ChangedBy { get; set; }
        public string ChangedDate { get; set; }
    }
    public class SearchClientNames
    {
        [Key]
        public int Id { get; set; }
        public string value { get; set; }
        public string Email { get; set; }
    }

    public class InstructionsDocument
    {
        [Key]
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public string OrgFileName { get; set; }
        public string UploadDate { get; set; }
    }

    #region Insurance Contract
    public class PEInsuranceContractFileDetails
    {
        public int Id { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string DocumentType { get; set; }
        public int InsuranceContractId { get; set; }
        public string InsuranceContractName { get; set; }
        public string LastRevalidateDate { get; set; }
        public string EffectiveDate { get; set; }
        public string NextUpdateDue { get; set; }
        public string TermedDate { get; set; }
        public string FileName { get; set; }
        public string ProviderNo { get; set; }
        public string GroupNo { get; set; }
        public string Active { get; set; }
        public string Info { get; set; }

    }
    #endregion


    public class PENotes
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Notes { get; set; }
    }
    public class PENotesList
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string CompanyName { get; set; }
        public string Notes { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }
    public class WebhookPayload
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }
    public class PEEFTFileUploadModel
    {
        public int Id { get; set; }
        public int ClientDetailsID { get; set; }
        public string EFTPayerName { get; set; }
        public int EFTBankType { get; set; }
        public string EFTComments { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
       

    }
}
