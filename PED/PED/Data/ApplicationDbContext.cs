using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PED.Models;
using PED.ViewModels.Admin;
using PED.ViewModels.Contract;
using PED.ViewModels.ProviderEntrollment;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WiseX.Models;
using WiseX.ViewModels.Account;
using WiseX.ViewModels.Admin;
using WiseX.ViewModels.Home;
namespace WiseX.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }

        //Masters
        public virtual DbSet<ProcedureCodes> ProcedureCodes { get; set; }
        public virtual DbSet<Modifiers> Modifiers { get; set; }
        //Admin-Users
        public virtual DbSet<UserListInfo> UserListInfo { get; set; }
        public virtual DbSet<ClientsCompanyDetails> ClientsDetailsInfo { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }
        public virtual DbSet<ProjectDetails> ProjectDetails { get; set; }
        public virtual DbSet<ProjectDetailsUsers> ProjectDetailsUsers { get; set; }
        public virtual DbSet<UserProjectRole> UserProjectRole { get; set; }
        public virtual DbSet<AllocatedList> AllocatedList { get; set; }
        public virtual DbSet<GetUserRoles> CheckUserRoles { get; set; }
        public virtual DbSet<UserDetailsTemp> UserDetailsTemp { get; set; }
        public virtual DbSet<SearchConditionFilterList> SearchConditionFilter { get; set; }
        public virtual DbSet<ChartHistoryList> ChartHistoryList { get; set; }
        public virtual DbSet<EmployeePositionList> EmployeePositionList { get; set; }

        //Common
        public virtual DbSet<ContractUSers> ContractUSersList { get; set; }
        public virtual DbSet<ContractAgreementType> ContractAgreementTypeList { get; set; }
        public virtual DbSet<CitiesList> CitiesList { get; set; }
        public virtual DbSet<StatesList> StatesList { get; set; }
        public virtual DbSet<UserOtherDetails> UserOtherDetails { get; set; }


        public virtual DbSet<Contractdetails> Contractdetails { get; set; }
        public virtual DbSet<SearchClient> SearchClient { get; set; }
        public virtual DbSet<SearchClientEmployee> SearchClientEmployee { get; set; }
        public virtual DbSet<AEDetailsList> AEDetailsList { get; set; }
        public virtual DbSet<ChartProperties> ChartProperties { get; set; }
        public virtual DbSet<ChartBoxProperties> ChartBoxProperties { get; set; }
        public virtual DbSet<ChartBoxPropertiesLoad> ChartBoxPropertiesLoad { get; set; }
        public virtual DbSet<PECredentialingLicenseBoxesList> PECredentialingLicenseBoxesList { get; set; }
        public virtual DbSet<PEContractBoxesList> PEContractBoxesList { get; set; }
        public virtual DbSet<UserSessionDetails> UserSessionDetails { get; set; }
        public virtual DbSet<PECheckListBoxes> PECheckListBoxes { get; set; }
        public virtual DbSet<PECheckListBoxesFroAllocatedUser> PECheckListBoxesFroAllocatedUser { get; set; }

        //Admin
        public virtual DbSet<Roles> Role { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }
        public virtual DbSet<NotificationCount> NotificationCount { get; set; }
        public virtual DbSet<AgreementTypeDetailsList> AgreementTypeList { get; set; }
        public virtual DbSet<PositionList> PositionList { get; set; }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<AccountExecutiveList> AccountExecutiveList { get; set; }
        public virtual DbSet<ResidencyCodeList> ResidencyCodeList { get; set; }

        public virtual DbSet<ClientsDetailsList> ClientsDetailsList { get; set; }
        public virtual DbSet<ClientsEmployeeList> ClientsEmployeeList { get; set; }
        public virtual DbSet<ClientContract> ClientContract { get; set; }
        public virtual DbSet<DownloadClientsDetailsList> DownloadClientsDetailsList { get; set; }
        public virtual DbSet<BulkClientsDetailsValidationList> BulkClientsDetailsValidationList { get; set; }
        public virtual DbSet<ClientsViewDetails> ClientsViewDetails { get; set; }



        //Contracts
        public virtual DbSet<ContractDetails> ContractDetails { get; set; }
        public virtual DbSet<ContractStatus> ContractStatus { get; set; }
        public virtual DbSet<CommentsDetailsList> CommentsDetailsList { get; set; }

        public virtual DbSet<ClientContractDetail> ClientContractDetail { get; set; }
        public virtual DbSet<ContractDetailApprovalList> ContractDetailApprovalList { get; set; }
        public virtual DbSet<ContractLogList> ContractLogList { get; set; }
        public virtual DbSet<ClientContractOtherDocumentDetails> ClientContractOtherDocumentDetails { get; set; }
        public virtual DbSet<ClientContractPassThruContractDetails> ClientContractPassThruContractDetails { get; set; }
        public virtual DbSet<ClientContractCrewLogDetails> ClientContractCrewLogDetails { get; set; }
        public virtual DbSet<ClientContractNotesDetails> ClientContractNotesDetails { get; set; }
        public virtual DbSet<NotificationDetails> NotificationDetailsList { get; set; }
        public virtual DbSet<SMTPServerDetails> SMTPServerDetails { get; set; }
        //MenuAccess 
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<RoleModules> RoleModules { get; set; }
        public virtual DbSet<MenuAccessRole> MenuAccessRole { get; set; }
        public virtual DbSet<ContractStatusList> ContractStatusList { get; set; }

        #region Insurance Contract
        public virtual DbSet<InsuranceContractList> InsuranceContractList { get; set; }
        public virtual DbSet<ClientsInsuranceContractList> ClientsInsuranceContractList { get; set; }
        public virtual DbSet<ClientInsuranceContractFileDetails> ClientInsuranceContractFileDetails { get; set; }

        #endregion

        public virtual DbSet<AddressTypeList> AddressTypeList { get; set; }
        public virtual DbSet<ContactTypeList> ContactTypeList { get; set; }
        public virtual DbSet<DocumentTitleList> DocumentTitleList { get; set; }
        public virtual DbSet<LicenseTypeList> LicenseTypeList { get; set; }
        public virtual DbSet<CertificationLevelList> CertificationLevelList { get; set; }
        public virtual DbSet<UserGroupList> UserGroupList { get; set; }
        public virtual DbSet<UserNameList> UserNameList { get; set; }


        #region Annual Charge List

        public virtual DbSet<AnnualChargeRateDet> AnnualChargeRateDet { get; set; }
        public virtual DbSet<AnnualChargeRateList> AnnualChargeRateList { get; set; }
        #endregion

        #region Provider Entrollment
        public virtual DbSet<ProviderEntrollmentDetailsList> ProviderEntrollmentDetailsList { get; set; }
        public virtual DbSet<DownloadProviderEntrollmentDetailsList> DownloadProviderEntrollmentDetailsList { get; set; }
        public virtual DbSet<PEStatusList> PEStatusList { get; set; }
        public virtual DbSet<PEHoursofOperationList> PEHoursofOperationList { get; set; }
        public virtual DbSet<PETaxClassificationList> PETaxClassificationList { get; set; }
        public virtual DbSet<PEPaymentIndicatorList> PEPaymentIndicatorList { get; set; }
        public virtual DbSet<PETaxonomyList> PETaxonomyList { get; set; }
        public virtual DbSet<PEAddressTypeList> PEAddressTypeList { get; set; }
        public virtual DbSet<PEContactTypeList> PEContactTypeList { get; set; }
        public virtual DbSet<PELicenseTypeList> PELicenseTypeList { get; set; }
        public virtual DbSet<PECertificationLevelList> PECertificationLevelList { get; set; }
        public virtual DbSet<PEDocumentTitleList> PEDocumentTitleList { get; set; }
        public virtual DbSet<PEDetails> PEDetails { get; set; }
        public virtual DbSet<PEDetailsList> PEDetailsList { get; set; }
        public virtual DbSet<PEInsuranceContracts> PEInsuranceContracts { get; set; }
        public virtual DbSet<PERefusedEnrollment> PERefusedEnrollment { get; set; }
        public virtual DbSet<PERefusedEnrollmentList> PERefusedEnrollmentList { get; set; }    
        public virtual DbSet<PEInsuranceContractFileDetails> PEInsuranceContractFileDetails { get; set; }
        public virtual DbSet<PEOtherAddress> PEOtherAddress { get; set; }
        public virtual DbSet<PEOtherAddressList> PEOtherAddressList { get; set; }
        public virtual DbSet<PEContacts> PEContacts { get; set; }
        public virtual DbSet<PEContactsList> PEContactsList { get; set; }
        public virtual DbSet<PEDocuments> PEDocuments { get; set; }
        public virtual DbSet<PEDocumentsList> PEDocumentsList { get; set; }
        public virtual DbSet<PECredentialingLicenseList> PECredentialingLicenseList { get; set; }
        public virtual DbSet<PECredentialingLicense> PECredentialingLicense { get; set; }
        public virtual DbSet<PESearchClient> PESearchClient { get; set; }
        public virtual DbSet<PECheckList> PECheckList { get; set; }
        public virtual DbSet<PEUsersRoleMailID> PEUsersRoleMailID { get; set; }
        public virtual DbSet<PEAuditLog> PEAuditLog { get; set; }
        public virtual DbSet<SearchClientNames> SearchClientNames { get; set; }       

        public virtual DbSet<PEEPCRList> PEEPCRList { get; set; }
        public virtual DbSet<PEEnrollmentRepresentativeStatusList> PEEnrollmentRepresentativeStatusList { get; set; }

        public virtual DbSet<PEEFT> PEEFT { get; set; }
        public virtual DbSet<PEEFTList> PEEFTList { get; set; }

        public virtual DbSet<PEClientsBankInfo> PEClientsBankInfo { get; set; }
        public virtual DbSet<PEClientsBankInfoList> PEClientsBankInfoList { get; set; }

        #endregion

        #region other check List
        public virtual DbSet<PEPaymentCategoryChangeCheckList> PEPaymentCategoryChangeCheckList { get; set; }
        public virtual DbSet<PESinglePayerUpdateCheckList> PESinglePayerUpdateCheckList { get; set; }
        public virtual DbSet<PEPracticeLocationChangeCheckList> PEPracticeLocationChangeCheckList { get; set; }
        public virtual DbSet<PEClosedClientCheckList> PEClosedClientCheckList { get; set; }
        public virtual DbSet<PEClientBankChangeCheckList> PEClientBankChangeCheckList { get; set; }
        public virtual DbSet<PECommericalEFTsCheckList> PECommericalEFTsCheckList { get; set; }
        public virtual DbSet<PEMedicareChangeOfInfoCheckList> PEMedicareChangeOfInfoCheckList { get; set; }
        public virtual DbSet<PEOutOfStateMCDCheckList> PEOutOfStateMCDCheckList { get; set; }

        #endregion
        public virtual DbSet<DownloadContractNotes> DownloadContractNotes { get; set; }

        public virtual DbSet<InstructionsDocument> InstructionsDocument { get; set; }

        public virtual DbSet<PENotes> PENotes { get; set; }
        public virtual DbSet<PENotesList> PENotesList { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<WebhookPayload> WebhookPayloads { get; set; }

        //public DbSet<PEEFTFileUploadModel> PEEFTTable { get; set; }

        


    }
}
