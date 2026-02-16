using PED.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Data;
using WiseX.Models;
using WiseX.ViewModels.Account;
using WiseX.ViewModels.Admin;
using PED.ViewModels.ProviderEntrollment;
using Microsoft.Azure.KeyVault.Models;

namespace WiseX.Services
{
    public class AdminService : DbContext
    {
        private readonly ApplicationDbContext _applicationDbContext;
        string connectionString = string.Empty;

        public AdminService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            //connectionString = _applicationDbContext.Database.GetDbConnection().ConnectionString;
            connectionString = DBConnection.ConnectionString;
        }

        public async Task<IList<UserDetailsTemp>> GetUserDetails(string UserId)
        {
            var paramUserId = new SqlParameter("@UserID", UserId);
            IList<UserDetailsTemp> check = new List<UserDetailsTemp>();
            try
            {
                check = await _applicationDbContext.UserDetailsTemp
                .FromSql("EXEC GetUserListDetails @UserID", paramUserId)
                .AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex) { }
            return check;

        }
        public async Task<IList<GetUserRoles>> GetUserRole(string RoleId)
        {
            var paramUserId = new SqlParameter("@RoleId", RoleId);
            IList<GetUserRoles> check = new List<GetUserRoles>();
            try
            {
                check = await _applicationDbContext.CheckUserRoles.FromSql("EXEC GetUserRoles @RoleId", paramUserId).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdateRoleDetails(string RoleId, string Status, int Permissions)
        {
            var RID = new SqlParameter("@RoleId", RoleId);
            var status = new SqlParameter("@Status", Status);
            var ParamPermissions = new SqlParameter("@Permissions", Permissions);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateRolesDetails @RoleId,@Status,@Permissions", RID, status, ParamPermissions);
            }
            catch (Exception Ex) { }
        }
        public async Task<List<Roles>> GetRolesDescStatus(string RoleId, int Active)
        {
            List<Roles> check = new List<Roles>();
            try
            {
                var RID = new SqlParameter("@RoleId", RoleId);
                var paramActive = new SqlParameter("@Active", Active);
                check = await _applicationDbContext.Role.FromSql("EXEC GetRolesDetails @RoleId, @Active", RID, paramActive).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task DeleteRoleDescDetails(string RoleId)
        {
            var RID = new SqlParameter("@RoleId", RoleId);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteRoleDescStatus @RoleId", RID);
            }
            catch (Exception Ex) { }
        }
        public async Task<IList<GetUserRoles>> GetAgreementType(string AggrementId)
        {
            var paramAggrementId = new SqlParameter("@AggrementId", AggrementId);
            IList<GetUserRoles> check = new List<GetUserRoles>();
            try
            {
                check = await _applicationDbContext.CheckUserRoles.FromSql("EXEC GetAggrementType @AggrementId", paramAggrementId).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdateAgreementTypeDetails(AgreementType agreementType, string UserId)
        {
            try
            {
                var paramAgreementType = new SqlParameter("@AgreementType", agreementType.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateAgreementTypeDetails @AgreementType,@UId", paramAgreementType, paramUserId);
            }
            catch (Exception Ex) { }
        }
        public async Task<List<AgreementTypeDetailsList>> GetAgreementTypeDescStatus(int AggrementId, int Active)
        {
            List<AgreementTypeDetailsList> check = new List<AgreementTypeDetailsList>();
            try
            {
                var paramAggrementId = new SqlParameter("@AggrementId", AggrementId);
                var paramActive = new SqlParameter("@Active", Active);
                check = await _applicationDbContext.AgreementTypeList.FromSql("EXEC GetAggrementType @AggrementId, @Active", paramAggrementId, paramActive).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task DeleteAgreementTypeDetails(int AgreementId)
        {

            try
            {
                var paramAgreementId = new SqlParameter("@AgreementId", AgreementId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteAgreementTypeDetails @AgreementId", paramAgreementId);

            }
            catch (Exception Ex) { }
        }



        public async Task<List<Client>> GetClients()
        {
            var paramId = new SqlParameter("@Id", 2);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.Client.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }

        public async Task<List<Client>> GetEditClients(string UserId)
        {
            var paramId = new SqlParameter("@Id", 2);
            var paramSearchTerm = new SqlParameter("@UserID", UserId);
            return await _applicationDbContext.Client.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }

        public async Task<List<Client>> GetClientUserdetails(string UserId)
        {

            var paramSearchTerm = new SqlParameter("@UserID", UserId);
            return await _applicationDbContext.Client.FromSql("EXEC GetClientUserdetails @UserID", paramSearchTerm).ToListAsync();
        }


        public async Task<List<UserListInfo>> GetUserList(string UserID)
        {
            List<UserListInfo> check = new List<UserListInfo>();
            try
            {
                var paramUserid = new SqlParameter("@UserID", UserID);
                check = await _applicationDbContext.UserListInfo
                .FromSql("EXEC GetUserDetails @UserID", paramUserid).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }

        public async Task UpdateUserDetails(Users users, string UId)
        {
            try
            {
                var paramUsers = new SqlParameter("@Users", users.GetXml());
                var UID = new SqlParameter("@UId", UId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateUserDetails @Users, @UId", paramUsers, UID);
            }
            catch (Exception Ex) { }
        }
        public async Task<IList<MenuItem>> GetMenuList(string RoleId = "")
        {
            List<MenuItem> menulist = new List<MenuItem>();
            try
            {
                var Menus = new SqlParameter("@RoleId", string.IsNullOrWhiteSpace(RoleId) ? "" : RoleId);
                //return await _applicationDbContext.MenuItem.FromSql("EXEC GetMenuAccessList @RoleId", Menus).AsNoTracking().ToListAsync();
                menulist = await _applicationDbContext.MenuItem.FromSql("EXEC GetMenuAccessList @RoleId", Menus).ToListAsync();
            }
            catch (Exception Ex) { }
            return menulist;
        }
        public async Task InsertMenuAccess(RoleModules roleModules)
        {
            try
            {
                var MenuAccess = new SqlParameter("@RoleModules", roleModules.GetXml());
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC InsertMenuAccessDetails @RoleModules", MenuAccess);
            }
            catch (Exception Ex) { }
        }
        public async Task DeleteUsers(string users)
        {

            try
            {
                var paramUsers = new SqlParameter("@Users", string.IsNullOrWhiteSpace(users) ? "" : users);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteUserDetails @Users", paramUsers);

            }
            catch (Exception Ex) { }

        }
        public async Task<List<MenuAccessRole>> MenuAccessRole(string RoleID, string UserID)
        {
            List<MenuAccessRole> MenuList = new List<MenuAccessRole>();
            try
            {
                var Menus = new SqlParameter("@RoleId", string.IsNullOrWhiteSpace(RoleID) ? "" : RoleID);
                var User = new SqlParameter("@UserID", string.IsNullOrWhiteSpace(UserID) ? "" : UserID);

                MenuList = await _applicationDbContext.MenuAccessRole.FromSql("EXEC GetMenuAccessRole @RoleId, @UserID ", Menus, User).ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return MenuList;
        }

        #region Clients
        public async Task<List<EmployeePositionList>> GetEmployeePositionList()
        {
            List<EmployeePositionList> check = new List<EmployeePositionList>();
            try
            {
                var paramId = new SqlParameter("@Id", 3);
                check = await _applicationDbContext.EmployeePositionList.FromSql("EXEC GetMasterTableData @Id", paramId).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<ClientContract> UpdateClientsDetails(Clients clients, string userid)
        {

            ClientContract clientContract = new ClientContract();
            try
            {
                var paramclients = new SqlParameter("@Clients", clients.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                clientContract = await _applicationDbContext.ClientContract.FromSql("EXEC UpdateClients @Clients,@userid", paramclients, paramuserid).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return clientContract;
        }
        public async Task<List<ClientsEmployeeList>> AddClientEmployeeDetails(ClientsEmployeeDetails employeeDetails, string UserId)
        {
            List<ClientsEmployeeList> check = new List<ClientsEmployeeList>();
            try
            {
                var paramEmployee = new SqlParameter("@Employee", employeeDetails.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);
                check = await _applicationDbContext.ClientsEmployeeList.FromSql("EXEC UpdateClientEmployee @Employee,@UId", paramEmployee, paramUserId).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<List<ClientsEmployeeList>> GetClientEmployeeList(int RefID)
        {
            List<ClientsEmployeeList> check = new List<ClientsEmployeeList>();
            try
            {
                var paramRefID = new SqlParameter("@RefID", RefID);
                check = await _applicationDbContext.ClientsEmployeeList.FromSql("EXEC GetClientEmployeeList @RefID", paramRefID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<List<ClientsDetailsList>> GetClients(int ClientId, int Active)
        {
            List<ClientsDetailsList> check = new List<ClientsDetailsList>();
            try
            {
                var paramClientId = new SqlParameter("@ClientId", ClientId);
                var paramActive = new SqlParameter("@Active", Active);
                check = await _applicationDbContext.ClientsDetailsList.FromSql("EXEC GetClients @ClientId, @Active", paramClientId, paramActive).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<ClientsViewDetails> ViewClients(int ClientId, int Active)
        {
            ClientsViewDetails check = new ClientsViewDetails();
            try
            {
                var paramClientId = new SqlParameter("@ClientId", ClientId);
                var paramActive = new SqlParameter("@Active", Active);
                check = await _applicationDbContext.ClientsViewDetails.FromSql("EXEC GetViewClients @ClientId, @Active", paramClientId, paramActive).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task DeleteClients(int ClientId, string userId)
        {

            try
            {
                var paramClientId = new SqlParameter("@ClientId", ClientId);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteClients @ClientId,@UserID", paramClientId, paramUserId);

            }
            catch (Exception Ex) { }
        }
        public async Task DeleteClientEmployee(int EmployeeID, string Userid)
        {

            try
            {
                var paramEmployeeID = new SqlParameter("@EmployeeID", EmployeeID);
                var paramuserID = new SqlParameter("@UId", Userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteClientEmployee @EmployeeID,@UId", paramEmployeeID, paramuserID);

            }
            catch (Exception Ex) { }
        }
        public async Task<List<DownloadClientsDetailsList>> ExportClients(int ClientStatusID, int ERID)
        {
            List<DownloadClientsDetailsList> check = new List<DownloadClientsDetailsList>();
            try
            {
                var paramClientStatusId = new SqlParameter("@ClientStatus", ClientStatusID);
                var paramERStatus = new SqlParameter("@PEERStatus", ERID);
                check = await _applicationDbContext.DownloadClientsDetailsList.FromSql("EXEC ExportClients @ClientStatus, @PEERStatus", paramClientStatusId, paramERStatus).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<CitiesList>> GetCitiesList()
        {
            List<CitiesList> check = new List<CitiesList>();
            try
            {
                var ParamID = new SqlParameter("@Id", 4);
                check = await _applicationDbContext.CitiesList.FromSql("EXEC GetMasterTableData @Id", ParamID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<StatesList>> GetStatesList()
        {
            List<StatesList> check = new List<StatesList>();
            try
            {
                var ParamID = new SqlParameter("@Id", 6);
                check = await _applicationDbContext.StatesList.FromSql("EXEC GetMasterTableData @Id", ParamID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<StatesList>> GetStateList(int CityId)
        {
            List<StatesList> check = new List<StatesList>();
            try
            {
                var ParamID = new SqlParameter("@CityId", CityId);
                check = await _applicationDbContext.StatesList.FromSql("EXEC GetStates @CityId", ParamID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<CitiesList>> GetCityList(int StateId)
        {
            List<CitiesList> check = new List<CitiesList>();
            try
            {
                var ParamID = new SqlParameter("@StateId", StateId);
                check = await _applicationDbContext.CitiesList.FromSql("EXEC GetCities @StateId", ParamID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<AccountExecutiveList>> GetAccountExecutiveList()
        {
            var paramId = new SqlParameter("@Id", 5);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.AccountExecutiveList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }

        public async Task<List<ResidencyCodeList>> GetResidencyCodeList()
        {
            var paramId = new SqlParameter("@Id", 7);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.ResidencyCodeList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PEStatusList>> GetPEStatusList()
        {
            var paramId = new SqlParameter("@Id", 9);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PEStatusList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PEHoursofOperationList>> GetHoursofOperationList()
        {
            var paramId = new SqlParameter("@Id", 10);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PEHoursofOperationList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PETaxClassificationList>> GetTaxClassificationList()
        {
            var paramId = new SqlParameter("@Id", 11);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PETaxClassificationList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PEPaymentIndicatorList>> GetPaymentIndicatorList()
        {
            var paramId = new SqlParameter("@Id", 12);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PEPaymentIndicatorList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PETaxonomyList>> GetPETaxonomyList()
        {
            var paramId = new SqlParameter("@Id", 13);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PETaxonomyList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PEAddressTypeList>> GetPEAddressTypeList()
        {
            var paramId = new SqlParameter("@Id", 14);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PEAddressTypeList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PEContactTypeList>> GetPEContactTypeList()
        {
            var paramId = new SqlParameter("@Id", 15);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PEContactTypeList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PELicenseTypeList>> GetPELicenseTypeList()
        {
            var paramId = new SqlParameter("@Id", 16);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PELicenseTypeList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PECertificationLevelList>> GetPECertificationLevelList()
        {
            var paramId = new SqlParameter("@Id", 17);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PECertificationLevelList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PEDocumentTitleList>> GetPEDocumentTitleList()
        {
            var paramId = new SqlParameter("@Id", 18);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PEDocumentTitleList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }

        public async Task UpdateBulkDetails(Clients bulkClientsDetails, string UId)
        {
            string retMessage = string.Empty;
            try
            {
                var paramclients = new SqlParameter("@BulkClientsDetails", bulkClientsDetails.GetXml());
                var UID = new SqlParameter("@UId", UId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateBulkClients @BulkClientsDetails, @UId", paramclients, UID);

            }
            catch (Exception Ex)
            {

            }
        }

        public async Task<List<BulkClientsDetailsValidationList>> UpdateBulkClientsValidation(Clients bulkClientsDetails, string UId)
        {
            var paramclients = new SqlParameter("@BulkClientsDetails", bulkClientsDetails.GetXml());
            var UID = new SqlParameter("@UId", UId);
            //await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateBulkClientsValidation @BulkClientsDetails, @UId", paramclients, UID);
            var list = await _applicationDbContext.BulkClientsDetailsValidationList.FromSql("EXEC UpdateBulkClientsValidation @BulkClientsDetails, @UId", paramclients, UID).ToListAsync();
            return list;
        }
        #endregion


        public async Task<List<PositionList>> GetPositionListStatus(int PositionID)
        {
            List<PositionList> check = new List<PositionList>();
            try
            {
                var paramPositionID = new SqlParameter("@PositionID", PositionID);
                check = await _applicationDbContext.PositionList.FromSql("EXEC GetEmployeePosition @PositionID", paramPositionID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task UpdateEmployeePositionDetails(Position position, string UserId)
        {
            try
            {
                var paramEmployeePosition = new SqlParameter("@EmployeePosition", position.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateEmployeePositionDetails @EmployeePosition,@UId", paramEmployeePosition, paramUserId);
            }
            catch (Exception Ex) { }
        }

        public async Task DeleteEmployeePositionDetails(int PositionID)
        {
            try
            {
                var paramPositionID = new SqlParameter("@PositionID", PositionID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteEmployeePositionDetails @PositionID", paramPositionID);

            }
            catch (Exception Ex) { }
        }
        public async Task InsertContractType(ContractStatusDet contractStatus, int id)
        {
            var Contract = new SqlParameter("@InsertContractType", contractStatus.GetXml());
            var GetId = new SqlParameter("@Id", id);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [InsertContractStatus] @InsertContractType, @Id", Contract, GetId);
            }
            catch (Exception ex)
            {

            }
        }
        public async Task<List<ContractStatusList>> GetContractTypeDetails(int id)
        {
            List<ContractStatusList> ContractStatusList = new List<ContractStatusList>();
            var GetId = new SqlParameter("@Id", id);
            try
            {
                ContractStatusList = await _applicationDbContext.ContractStatusList.FromSql("EXEC [GetContractType] @Id", GetId).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return ContractStatusList;
        }
        public async Task DeleteContractDetails(int ContractID)
        {
            try
            {
                var paramContractID = new SqlParameter("@Id", ContractID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteContractDetails @Id", paramContractID);

            }
            catch (Exception Ex) { }
        }
        public async Task<List<ClientsCompanyDetails>> GetCompanyList(int CId)
        {
            List<ClientsCompanyDetails> check = new List<ClientsCompanyDetails>();
            try
            {
                var paramUserid = new SqlParameter("@CompanyID", CId);
                check = await _applicationDbContext.ClientsDetailsInfo.FromSql("EXEC [dbo].[GetCompanyIdDetails] @CompanyID", paramUserid).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }
        //GetContractList
        public async Task<List<ContractStatusList>> GetContractList(String CStatus)
        {
            List<ContractStatusList> check = new List<ContractStatusList>();
            try
            {
                var paramUserid = new SqlParameter("@ContractStatus", CStatus);
                check = await _applicationDbContext.ContractStatusList.FromSql("EXEC [dbo].[GetValidContractStatus] @ContractStatus", paramUserid).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }
        public async Task<List<PEEPCRList>> GetPEEPCRStatusList()
        {
            var paramId = new SqlParameter("@Id", 19);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PEEPCRList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        public async Task<List<PEEnrollmentRepresentativeStatusList>> GetPEEnrollmentRepresentativeStatusList()
        {
            var paramId = new SqlParameter("@Id", 20);
            var paramSearchTerm = new SqlParameter("@UserID", DBNull.Value);
            return await _applicationDbContext.PEEnrollmentRepresentativeStatusList.FromSql("EXEC GetMasterTableData @Id, @UserID", paramId, paramSearchTerm).ToListAsync();
        }
        /*
         
         public virtual DbSet<PEEPCRList> PEEPCRList { get; set; }
        public virtual DbSet<PEEnrollmentRepresentativeStatusList> PEEnrollmentRepresentativeStatusList { get; set; }
         */

        #region Insurance Contract
        public async Task<List<InsuranceContractList>> GetInsuranceContract(int id)
        {
            List<InsuranceContractList> lstInsuranceContractList = new List<InsuranceContractList>();
            var GetId = new SqlParameter("@Id", id);
            try
            {
                lstInsuranceContractList = await _applicationDbContext.InsuranceContractList.FromSql("EXEC [USP_InsuranceContract_Select] @Id", GetId).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return lstInsuranceContractList;
        }

        public async Task InsertInsuranceContract(int id, string Name, bool Active, string CreatedBy)
        {
            var GetId = new SqlParameter("@Id", id);
            var GetName = new SqlParameter("@Name", Name);
            var GetActive = new SqlParameter("@Active", Active);
            var GetCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_InsuranceContract_Insert] @Id,@Name,@Active,@CreatedBy", GetId, GetName, GetActive, GetCreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<InsuranceContractList>> GetInsuranceContractListByName(String Name)
        {
            List<InsuranceContractList> check = new List<InsuranceContractList>();
            try
            {
                var paramName = new SqlParameter("@Name", Name);
                check = await _applicationDbContext.InsuranceContractList.FromSql("EXEC [dbo].[GetValidInsuranceContract] @Name", paramName).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }

        public async Task DeleteInsuranceContractDetails(int ID)
        {
            try
            {
                var paramContractID = new SqlParameter("@Id", ID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteInsuranceContract @Id", paramContractID);

            }
            catch (Exception Ex) { }
        }

        public async Task<List<ClientsInsuranceContractList>> GetClientInsuranceContractList(int RefID)
        {
            List<ClientsInsuranceContractList> check = new List<ClientsInsuranceContractList>();
            try
            {
                var paramRefID = new SqlParameter("@RefID", RefID);
                check = await _applicationDbContext.ClientsInsuranceContractList.FromSql("EXEC USP_ClientInsuranceContract_Select @RefID", paramRefID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task DeleteClientInsuranceContract(int ID, string Userid)
        {

            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramuserID = new SqlParameter("@UId", Userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteClientInsuranceContract @ID,@UId", paramID, paramuserID);

            }
            catch (Exception Ex) { }
        }

        public async Task<List<ClientsInsuranceContractList>> AddClientsInsuranceContract(ClientsInsuranceContractDetails insuranceContractDetails, string UserId)
        {
            List<ClientsInsuranceContractList> check = new List<ClientsInsuranceContractList>();
            try
            {
                var paramInsuranceContract = new SqlParameter("@InsuranceContract", insuranceContractDetails.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);
                check = await _applicationDbContext.ClientsInsuranceContractList.FromSql("EXEC UpdateClientInsuranceContract @InsuranceContract,@UId", paramInsuranceContract, paramUserId).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task UpdateClientInsuranceContractFileDetails(int ID, string FileName, string FilePath, string OrgFileName, string UserId)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramFileName = new SqlParameter("@FileName", FileName);
                var paramFilePath = new SqlParameter("@FilePath", FilePath);
                var paramOrgFileName = new SqlParameter("@OrgFileName", OrgFileName);
                var paramUserId = new SqlParameter("@UserId", UserId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateClientInsuranceContractFileDetails @ID,@FileName,@FilePath,@OrgFileName,@UserId", paramID, paramFileName, paramFilePath, paramOrgFileName, paramUserId);

            }
            catch (Exception Ex)
            {

            }
        }

        #endregion

        #region Annual charge Rate
        public async Task InsertAnnualChargeRate(string FilePath, string CompanyID, string CreatedBy)
        {
            var GetFilePath = new SqlParameter("@FilePath", FilePath);
            var GetCompanyID = new SqlParameter("@CompanyID", CompanyID);
            var GetCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_AnnualChargeRate_Insert] @FilePath,@CompanyID,@CreatedBy", GetFilePath, GetCompanyID, GetCreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<AnnualChargeRateList>> GetAnnualChargeRateList(string CompanyID)
        {
            List<AnnualChargeRateList> check = new List<AnnualChargeRateList>();
            try
            {
                var paramCompanyID = new SqlParameter("@CompanyID", CompanyID);
                check = await _applicationDbContext.AnnualChargeRateList.FromSql("EXEC USP_AnnualChargeRate_Select @CompanyID", paramCompanyID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task DeleteAnnualChargeRate(int ID, string Userid)
        {

            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramuserID = new SqlParameter("@UId", Userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_AnnualChargeRate_Delete @ID,@UId", paramID, paramuserID);

            }
            catch (Exception Ex) { }
        }

        #endregion

        #region Provider Entrollment
        public async Task<List<ProviderEntrollmentDetailsList>> GetProviderEntrollment(int PEId)
        {
            List<ProviderEntrollmentDetailsList> check = new List<ProviderEntrollmentDetailsList>();
            try
            {
                var paramProviderEntrollmentID = new SqlParameter("@PEId", PEId);
                check = await _applicationDbContext.ProviderEntrollmentDetailsList.FromSql("EXEC GetProviderEntrollment @PEId", paramProviderEntrollmentID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<ProviderEntrollmentDetailsList>> UpdateProviderEntrollmentDetails(ProviderEntrollment providerEntrollment, string userid)
        {
            List<ProviderEntrollmentDetailsList> check = new List<ProviderEntrollmentDetailsList>();
            try
            {
                var paramProviderEntrollment = new SqlParameter("@ProviderEntrollment", providerEntrollment.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                check = await _applicationDbContext.ProviderEntrollmentDetailsList.FromSql("EXEC UpdateProviderEntrollment @ProviderEntrollment,@userid", paramProviderEntrollment, paramuserid).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task DeleteProviderEntrollment(int PEId, string userId)
        {

            try
            {
                var paramPEId = new SqlParameter("@PEId", PEId);
                var paramUserId = new SqlParameter("@UserID", userId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteProviderEntrollment @PEId,@UserID", paramPEId, paramUserId);

            }
            catch (Exception Ex) { }
        }

        public async Task<List<DownloadProviderEntrollmentDetailsList>> ExportProviderEntrollmentDetails()
        {
            List<DownloadProviderEntrollmentDetailsList> check = new List<DownloadProviderEntrollmentDetailsList>();
            try
            {
                check = await _applicationDbContext.DownloadProviderEntrollmentDetailsList.FromSql("EXEC ExportProviderEntrollment").ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<PEDetails> GetPEDetails(int ClientID, string UserId)
        {
            PEDetails check = new PEDetails();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            var paramUserId = new SqlParameter("@UserId", UserId);
            try
            {
                check = await _applicationDbContext.PEDetails.FromSql("EXEC USP_tblPEDetails_Select @ClientID,@UserId", paramClientID, paramUserId).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<PEDetailsList>> GetPEDetailsList(int ClientID)
        {
            List<PEDetailsList> check = new List<PEDetailsList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PEDetailsList.FromSql("EXEC USP_tblPEDetails_SelectList @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task InsertPEDetails(ProviderEntrollment providerEntrollment, string userid)
        {
            try
            {
                var paramPEDetailsInsert = new SqlParameter("@PEDetails", providerEntrollment.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEDetails_Insert @PEDetails,@userid", paramPEDetailsInsert, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task DeletePEDetails(int ID, string userid)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEDetails_Delete @ID,@userid", paramID, paramuserid);
            }
            catch (Exception Ex) { }
        }


        #region Insurance Contract
        public async Task<List<PEInsuranceContractFileDetails>> GetPEInsuranceContractFileDetailsList(int ClientID)
        {
            List<PEInsuranceContractFileDetails> ListDetails = new List<PEInsuranceContractFileDetails>();
            try
            {
                var paramClientID = new SqlParameter("@ClientId", ClientID);
                ListDetails = await _applicationDbContext.PEInsuranceContractFileDetails.FromSql("EXEC GetClientInsuranceContractFileDetailsList @ClientId", paramClientID).AsNoTracking().ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return ListDetails;

        }
        #endregion

        public async Task<PEInsuranceContracts> GetPEInsuranceContractsByID(int ID)
        {
            PEInsuranceContracts check = new PEInsuranceContracts();
            var paramID = new SqlParameter("@ID", ID);
            try
            {
                check = await _applicationDbContext.PEInsuranceContracts.FromSql("EXEC USP_PE_tblInsuranceContractDetails_SelectByID @ID", paramID).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task InsertPEInsuranceContracts(ProviderEntrollment providerEntrollment, string userid)
        {
            try
            {
                var paramPEInsuranceContracts = new SqlParameter("@PEInsuranceContracts", providerEntrollment.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_PE_tblInsuranceContractDetails_Insert @PEInsuranceContracts,@userid", paramPEInsuranceContracts, paramuserid);
            }
            catch (Exception Ex) { }
        }
        public async Task InsertPERefusedEnrollment(ProviderEntrollment providerEntrollment, string userid)
        {
            try
            {
                var paramPERefusedEnrollment = new SqlParameter("@PERefusedEnrollment", providerEntrollment.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_PE_tblRefusedEnrollmentDetails_Insert @PERefusedEnrollment,@userid", paramPERefusedEnrollment, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task InsertPEEFT(ProviderEntrollment providerEntrollment, string userid)
        {
            try
            {
                var paramPEEFT = new SqlParameter("@PEEFT", providerEntrollment.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_PE_tblPEEFTDetails_Insert @PEEFT,@userid", paramPEEFT, paramuserid);
            }
            catch (Exception Ex) { }
        }
        public async Task<List<PEEFTList>> GetPEEFTList(int ClientID)
        {
            List<PEEFTList> check = new List<PEEFTList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PEEFTList.FromSql("EXEC USP_tblPEEFT_Select @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<PEEFT> GetPEEFTByID(int ID)
        {
            PEEFT check = new PEEFT();
            var paramID = new SqlParameter("@ID", ID);
            try
            {
                check = await _applicationDbContext.PEEFT.FromSql("EXEC USP_tblPEEFT_SelectByID @ID", paramID).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task DeletePEEFT(int ID, string userid)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEEFT_Delete @ID,@userid", paramID, paramuserid);
            }
            catch (Exception Ex) { }
        }
        public async Task<List<PERefusedEnrollmentList>> GetPERefusedEnrollmentList(int ClientID)
        {
            List<PERefusedEnrollmentList> check = new List<PERefusedEnrollmentList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PERefusedEnrollmentList.FromSql("EXEC USP_tblPERefusedEnrollment_Select @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<PERefusedEnrollment> GetPERefusedEnrollmentByID(int ID)
        {
            PERefusedEnrollment check = new PERefusedEnrollment();
            var paramID = new SqlParameter("@ID", ID);
            try
            {
                check = await _applicationDbContext.PERefusedEnrollment.FromSql("EXEC USP_tblPERefusedEnrollment_SelectByID @ID", paramID).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task DeletePERefusedEnrollment(int ID, string userid)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPERefusedEnrollment_Delete @ID,@userid", paramID, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task<List<PEOtherAddressList>> GetPEOtherAddresses(int ClientID)
        {
            List<PEOtherAddressList> check = new List<PEOtherAddressList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PEOtherAddressList.FromSql("EXEC USP_tblPEOtherAddress_Select @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<PEOtherAddress> GetPEOtherAddressesByID(int ID)
        {
            PEOtherAddress check = new PEOtherAddress();
            var paramID = new SqlParameter("@ID", ID);
            try
            {
                check = await _applicationDbContext.PEOtherAddress.FromSql("EXEC USP_tblPEOtherAddress_SelectByID @ID", paramID).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task DeletePEOtherAddresses(int ID, string userid)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEOtherAddress_Delete @ID,@userid", paramID, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task InsertPEOtherAddress(ProviderEntrollment providerEntrollment, string userid)
        {
            try
            {
                var paramPEOtherAddress = new SqlParameter("@PEOtherAddress", providerEntrollment.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEOtherAddress_Insert @PEOtherAddress,@userid", paramPEOtherAddress, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task<List<PEContactsList>> GetPEContactsList(int ClientID)
        {
            List<PEContactsList> check = new List<PEContactsList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PEContactsList.FromSql("EXEC USP_tblPEContacts_Select @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<PEContacts> GetPEContactsByID(int ID)
        {
            PEContacts check = new PEContacts();
            var paramID = new SqlParameter("@ID", ID);
            try
            {
                check = await _applicationDbContext.PEContacts.FromSql("EXEC USP_tblPEContacts_SelectByID @ID", paramID).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task DeletePEContacts(int ID, string userid)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEContacts_Delete @ID,@userid", paramID, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task InsertPEContacts(ProviderEntrollment providerEntrollment, string userid)
        {
            try
            {
                var paramPEContacts = new SqlParameter("@PEContacts", providerEntrollment.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEContacts_Insert @PEContacts,@userid", paramPEContacts, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task UpdatePEFileDetails(int ID, string GovBankLetterFileName, string GovBankLetterOrgFileName, string PrevBankFileName, string PrevBankOrgFileName, string NonGovBankLetterFileName, string NonGovBankLetterOrgFileName, string UserId)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramGovBankLetterFileName = new SqlParameter("@GovBankLetterFileName", GovBankLetterFileName);
                var ParamGovBankLetterOrgFileName = new SqlParameter("@GovBankLetterOrgFileName", GovBankLetterOrgFileName);
                var paramPrevBankFileName = new SqlParameter("@PrevBankFileName", PrevBankFileName);
                var paramPrevBankOrgFileName = new SqlParameter("@PrevBankOrgFileName", PrevBankOrgFileName);
                var paramNonGovBankLetterFileName = new SqlParameter("@NonGovBankLetterFileName", NonGovBankLetterFileName);
                var paramNonGovBankLetterOrgFileName = new SqlParameter("@NonGovBankLetterOrgFileName", NonGovBankLetterOrgFileName);
                var paramUserId = new SqlParameter("@UserId", UserId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEDetails_UpdateFileDetails @ID,@GovBankLetterFileName,@GovBankLetterOrgFileName,@PrevBankFileName,@PrevBankOrgFileName,@NonGovBankLetterFileName,@NonGovBankLetterOrgFileName,@UserId", paramID, paramGovBankLetterFileName, ParamGovBankLetterOrgFileName, paramPrevBankFileName, paramPrevBankOrgFileName, paramNonGovBankLetterFileName, paramNonGovBankLetterOrgFileName, paramUserId);

            }
            catch (Exception Ex)
            {

            }
        }



        public async Task<List<PEDocumentsList>> GetPEDocumentsList(int ClientID)
        {
            List<PEDocumentsList> check = new List<PEDocumentsList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PEDocumentsList.FromSql("EXEC USP_tblPEDocuments_Select @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        //public async Task<PEDocuments> GetPEDocumentsByID(int ID)
        //{
        //    PEDocuments check = new PEDocuments();
        //    var paramID = new SqlParameter("@ID", ID);
        //    try
        //    {
        //        check = await _applicationDbContext.PEDocuments.FromSql("EXEC USP_tblPEDocuments_SelectByID @ID", paramID).FirstOrDefaultAsync();
        //    }
        //    catch (Exception Ex) { }
        //    return check;
        //}

        public async Task DeletePEDocuments(int ID, string userid)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEDocuments_Delete @ID,@userid", paramID, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task<PEDocuments> InsertPEDocuments(ProviderEntrollment providerEntrollment, string userid)
        {
            PEDocuments check = new PEDocuments();
            try
            {
                var paramPEDocuments = new SqlParameter("@PEDocuments", providerEntrollment.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                check = await _applicationDbContext.PEDocuments.FromSql("EXEC USP_tblPEDocuments_Insert @PEDocuments,@userid", paramPEDocuments, paramuserid).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }

            return check;
        }

        public async Task UpdatePEDocumentsFileDetails(int ID, int ClientDetailsID, string TitleName, string FileName, string FilePath, string OrgFileName, string UserId)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramClientDetailsID = new SqlParameter("@ClientDetailsID", ClientDetailsID);
                var paramTitleName = new SqlParameter("@TitleName", TitleName);
                var paramFileName = new SqlParameter("@FileName", FileName);
                var paramFilePath = new SqlParameter("@FilePath", FilePath);
                var paramOrgFileName = new SqlParameter("@OrgFileName", OrgFileName);
                var paramUserId = new SqlParameter("@UserId", UserId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEDocuments_UpdateFileDetails @ID,@ClientDetailsID,@TitleName" +
                    ",@FileName,@FilePath,@OrgFileName,@UserId", paramID, paramClientDetailsID, paramTitleName, paramFileName, paramFilePath
                    , paramOrgFileName, paramUserId);

            }
            catch (Exception Ex)
            {

            }
        }


        public async Task<List<PECredentialingLicenseList>> GetPECredentialingLicenseList(int ClientID)
        {
            List<PECredentialingLicenseList> check = new List<PECredentialingLicenseList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PECredentialingLicenseList.FromSql("EXEC USP_tblPECredentialingLicense_Select @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<PECredentialingLicense> GetPECredentialingLicenseByID(int ID)
        {
            PECredentialingLicense check = new PECredentialingLicense();
            var paramID = new SqlParameter("@ID", ID);
            try
            {
                check = await _applicationDbContext.PECredentialingLicense.FromSql("EXEC USP_tblPECredentialingLicense_SelectByID @ID", paramID).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task DeletePECredentialingLicense(int ID, string userid)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPECredentialingLicense_Delete @ID,@userid", paramID, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task InsertPECredentialingLicense(ProviderEntrollment providerEntrollment, string userid)
        {
            try
            {
                var paramPECredentialingLicense = new SqlParameter("@PECredentialingLicense", providerEntrollment.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPECredentialingLicense_Insert @PECredentialingLicense,@userid", paramPECredentialingLicense, paramuserid);
            }
            catch (Exception Ex) { }
        }

        #endregion

        #region AddressType
        public async Task<List<AddressTypeList>> GetAddressType(int id)
        {
            List<AddressTypeList> lstAddressTypeList = new List<AddressTypeList>();
            var GetId = new SqlParameter("@Id", id);
            try
            {
                lstAddressTypeList = await _applicationDbContext.AddressTypeList.FromSql("EXEC [USP_PEAddressType_Select] @Id", GetId).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return lstAddressTypeList;
        }

        public async Task InsertAddressType(int id, string Name, bool Active, string CreatedBy)
        {
            var GetId = new SqlParameter("@Id", id);
            var GetName = new SqlParameter("@Name", Name);
            var GetActive = new SqlParameter("@Active", Active);
            var GetCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_PEAddressType_Insert] @Id,@Name,@Active,@CreatedBy", GetId, GetName, GetActive, GetCreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<AddressTypeList>> GetAddressTypeListByName(String Name)
        {
            List<AddressTypeList> check = new List<AddressTypeList>();
            try
            {
                var paramName = new SqlParameter("@Name", Name);
                check = await _applicationDbContext.AddressTypeList.FromSql("EXEC [dbo].[USP_PEAddressType_CheckByName] @Name", paramName).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }

        public async Task DeleteAddressTypeDetails(int ID)
        {
            try
            {
                var paramContractID = new SqlParameter("@Id", ID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_PEAddressType_Delete @Id", paramContractID);

            }
            catch (Exception Ex) { }
        }

        #endregion

        #region ContactType
        public async Task<List<ContactTypeList>> GetContactType(int id)
        {
            List<ContactTypeList> lstContactTypeList = new List<ContactTypeList>();
            var GetId = new SqlParameter("@Id", id);
            try
            {
                lstContactTypeList = await _applicationDbContext.ContactTypeList.FromSql("EXEC [USP_PEContactType_Select] @Id", GetId).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return lstContactTypeList;
        }

        public async Task InsertContactType(int id, string Name, bool Active, string CreatedBy)
        {
            var GetId = new SqlParameter("@Id", id);
            var GetName = new SqlParameter("@Name", Name);
            var GetActive = new SqlParameter("@Active", Active);
            var GetCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_PEContactType_Insert] @Id,@Name,@Active,@CreatedBy", GetId, GetName, GetActive, GetCreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<ContactTypeList>> GetContactTypeListByName(String Name)
        {
            List<ContactTypeList> check = new List<ContactTypeList>();
            try
            {
                var paramName = new SqlParameter("@Name", Name);
                check = await _applicationDbContext.ContactTypeList.FromSql("EXEC [dbo].[USP_PEContactType_CheckByName] @Name", paramName).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }

        public async Task DeleteContactTypeDetails(int ID)
        {
            try
            {
                var paramContractID = new SqlParameter("@Id", ID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_PEContactType_Delete @Id", paramContractID);

            }
            catch (Exception Ex) { }
        }

        #endregion

        #region DocumentTitle
        public async Task<List<DocumentTitleList>> GetDocumentTitle(int id)
        {
            List<DocumentTitleList> lstDocumentTitleList = new List<DocumentTitleList>();
            var GetId = new SqlParameter("@Id", id);
            try
            {
                lstDocumentTitleList = await _applicationDbContext.DocumentTitleList.FromSql("EXEC [USP_PEDocumentTitle_Select] @Id", GetId).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return lstDocumentTitleList;
        }

        public async Task InsertDocumentTitle(int id, string Name, bool Active, string CreatedBy)
        {
            var GetId = new SqlParameter("@Id", id);
            var GetName = new SqlParameter("@Name", Name);
            var GetActive = new SqlParameter("@Active", Active);
            var GetCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_PEDocumentTitle_Insert] @Id,@Name,@Active,@CreatedBy", GetId, GetName, GetActive, GetCreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<DocumentTitleList>> GetDocumentTitleListByName(String Name)
        {
            List<DocumentTitleList> check = new List<DocumentTitleList>();
            try
            {
                var paramName = new SqlParameter("@Name", Name);
                check = await _applicationDbContext.DocumentTitleList.FromSql("EXEC [dbo].[USP_PEDocumentTitle_CheckByName] @Name", paramName).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }

        public async Task DeleteDocumentTitleDetails(int ID)
        {
            try
            {
                var paramContractID = new SqlParameter("@Id", ID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_PEDocumentTitle_Delete @Id", paramContractID);

            }
            catch (Exception Ex) { }
        }

        #endregion

        #region LicenseType
        public async Task<List<LicenseTypeList>> GetLicenseType(int id)
        {
            List<LicenseTypeList> lstLicenseTypeList = new List<LicenseTypeList>();
            var GetId = new SqlParameter("@Id", id);
            try
            {
                lstLicenseTypeList = await _applicationDbContext.LicenseTypeList.FromSql("EXEC [USP_PELicenseType_Select] @Id", GetId).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return lstLicenseTypeList;
        }

        public async Task InsertLicenseType(int id, string Name, bool Active, string CreatedBy)
        {
            var GetId = new SqlParameter("@Id", id);
            var GetName = new SqlParameter("@Name", Name);
            var GetActive = new SqlParameter("@Active", Active);
            var GetCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_PELicenseType_Insert] @Id,@Name,@Active,@CreatedBy", GetId, GetName, GetActive, GetCreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<LicenseTypeList>> GetLicenseTypeListByName(String Name)
        {
            List<LicenseTypeList> check = new List<LicenseTypeList>();
            try
            {
                var paramName = new SqlParameter("@Name", Name);
                check = await _applicationDbContext.LicenseTypeList.FromSql("EXEC [dbo].[USP_PELicenseType_CheckByName] @Name", paramName).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }

        public async Task DeleteLicenseTypeDetails(int ID)
        {
            try
            {
                var paramContractID = new SqlParameter("@Id", ID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_PELicenseType_Delete @Id", paramContractID);

            }
            catch (Exception Ex) { }
        }

        #endregion

        #region CertificationLevel
        public async Task<List<CertificationLevelList>> GetCertificationLevel(int id)
        {
            List<CertificationLevelList> lstCertificationLevelList = new List<CertificationLevelList>();
            var GetId = new SqlParameter("@Id", id);
            try
            {
                lstCertificationLevelList = await _applicationDbContext.CertificationLevelList.FromSql("EXEC [USP_PECertificationLevel_Select] @Id", GetId).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return lstCertificationLevelList;
        }

        public async Task InsertCertificationLevel(int id, string Name, bool Active, string CreatedBy)
        {
            var GetId = new SqlParameter("@Id", id);
            var GetName = new SqlParameter("@Name", Name);
            var GetActive = new SqlParameter("@Active", Active);
            var GetCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_PECertificationLevel_Insert] @Id,@Name,@Active,@CreatedBy", GetId, GetName, GetActive, GetCreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<CertificationLevelList>> GetCertificationLevelListByName(String Name)
        {
            List<CertificationLevelList> check = new List<CertificationLevelList>();
            try
            {
                var paramName = new SqlParameter("@Name", Name);
                check = await _applicationDbContext.CertificationLevelList.FromSql("EXEC [dbo].[USP_PECertificationLevel_CheckByName] @Name", paramName).ToListAsync();
            }
            catch (Exception Ex)
            { }
            return check;
        }

        public async Task DeleteCertificationLevelDetails(int ID)
        {
            try
            {
                var paramContractID = new SqlParameter("@Id", ID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_PECertificationLevel_Delete @Id", paramContractID);

            }
            catch (Exception Ex) { }
        }

        public async Task<List<PESearchClient>> GetPESearchClient(string Prefix)
        {
            List<PESearchClient> lst = new List<PESearchClient>();
            try
            {
                var Param = new SqlParameter("@SearchTerm", Prefix);
                lst = await _applicationDbContext.PESearchClient.FromSql("EXEC USP_Search_Client @SearchTerm", Param).ToListAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }

        public async Task<PECheckList> GetPECheckList(int ClientID, string UserId)
        {
            PECheckList check = new PECheckList();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            var paramUserId = new SqlParameter("@UserId", UserId);
            try
            {
                check = await _applicationDbContext.PECheckList.FromSql("EXEC USP_tblPECheckList_Select @ClientID,@UserId", paramClientID, paramUserId).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<PEUsersRoleMailID>> GetAccountExecutiveMailID(int ClientID)
        {
            List<PEUsersRoleMailID> lst = new List<PEUsersRoleMailID>();
            try
            {
                var Param = new SqlParameter("@ClientID", ClientID);
                lst = await _applicationDbContext.PEUsersRoleMailID.FromSql("EXEC USP_AccountExecutive_Select @ClientID", Param).ToListAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }

        public async Task<List<PEUsersRoleMailID>> GetSupervisorMailID()
        {
            List<PEUsersRoleMailID> lst = new List<PEUsersRoleMailID>();
            try
            {
                lst = await _applicationDbContext.PEUsersRoleMailID.FromSql("EXEC USP_SupervisorMailID_Select").ToListAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }

        public async Task<List<PEUsersRoleMailID>> GetWaystartMailID()
        {
            List<PEUsersRoleMailID> lst = new List<PEUsersRoleMailID>();
            try
            {
                lst = await _applicationDbContext.PEUsersRoleMailID.FromSql("EXEC USP_WaystarAdminMailID_Select").ToListAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }

        public async Task UpdatePECheckListStatus(int ClientID, string Mode, string Status, string UserId, string To, string CC, string Subject, string Body, string Attachements)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramTo = new SqlParameter("@To", To);
                var paramCC = new SqlParameter("@CC", CC);
                var paramSubject = new SqlParameter("@Subject", Subject);
                var paramBody = new SqlParameter("@Body", Body);
                var paramAttachements = new SqlParameter("@Attachements", Attachements);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_PECheckList_Update @ClientID,@Mode,@Status,@UserId,@To,@CC,@Subject,@Body,@Attachements"
                    , paramClientID, paramMode, paramStatus, paramUserId, paramTo, paramCC, paramSubject, paramBody, paramAttachements);

            }
            catch (Exception Ex) { }
        }

        public async Task InsertMailHistory(int ClientID, string UserId, string To, string CC, string Subject, string Body, string Attachements)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramTo = new SqlParameter("@To", To);
                var paramCC = new SqlParameter("@CC", CC);
                var paramSubject = new SqlParameter("@Subject", Subject);
                var paramBody = new SqlParameter("@Body", Body);
                var paramAttachements = new SqlParameter("@Attachements", Attachements);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblMailHistory_Insert @ClientID,@UserId,@To,@CC,@Subject,@Body,@Attachements"
                    , paramClientID, paramUserId, paramTo, paramCC, paramSubject, paramBody, paramAttachements);

            }
            catch (Exception Ex) { }
        }

        public async Task<List<PEAuditLog>> GetAuditLog(int ClientID)
        {
            List<PEAuditLog> lst = new List<PEAuditLog>();
            try
            {

                var paramClientID = new SqlParameter("@ClientID", ClientID);
                lst = await _applicationDbContext.PEAuditLog.FromSql("EXEC USP_tblPEHistory_Select @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }


        public async Task<List<SearchClientNames>> GetSearchPEClientNames(string Prefix, int clientId)
        {
            List<SearchClientNames> lst = new List<SearchClientNames>();
            try
            {
                var Param = new SqlParameter("@SearchTerm", Prefix);
                var ParamClientId = new SqlParameter("@clientId", clientId);

                lst = await _applicationDbContext.SearchClientNames.FromSql("EXEC GetSearchPEClientName @SearchTerm,@clientId", Param, ParamClientId).ToListAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }
        #endregion

        #region other Check list

        public async Task<PEPaymentCategoryChangeCheckList> GetPEPaymentCategoryChangeCheckList(int ClientID, string UserId)
        {
            PEPaymentCategoryChangeCheckList check = new PEPaymentCategoryChangeCheckList();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            var paramUserId = new SqlParameter("@UserId", UserId);
            try
            {
                check = await _applicationDbContext.PEPaymentCategoryChangeCheckList.FromSql("EXEC USP_tblPEPaymentCategoryChangeCheckList_Select @ClientID,@UserId", paramClientID, paramUserId).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task UpdatePEPaymentCategoryChangeCheckList(int ClientID, string Mode, string Status, string UserId, string To, string CC, string Subject, string Body, string Attachements)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramTo = new SqlParameter("@To", To);
                var paramCC = new SqlParameter("@CC", CC);
                var paramSubject = new SqlParameter("@Subject", Subject);
                var paramBody = new SqlParameter("@Body", Body);
                var paramAttachements = new SqlParameter("@Attachements", Attachements);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEPaymentCategoryChangeCheckList_Update @ClientID,@Mode,@Status,@UserId,@To,@CC,@Subject,@Body,@Attachements"
                    , paramClientID, paramMode, paramStatus, paramUserId, paramTo, paramCC, paramSubject, paramBody, paramAttachements);

            }
            catch (Exception Ex) { }
        }

        public async Task<PESinglePayerUpdateCheckList> GetPESinglePayerUpdateCheckList(int ID)
        {
            PESinglePayerUpdateCheckList check = new PESinglePayerUpdateCheckList();
            var paramID = new SqlParameter("@ID", ID);
            try
            {
                check = await _applicationDbContext.PESinglePayerUpdateCheckList.FromSql("EXEC USP_tblPESinglePayerUpdateCheckList_Select @ID,@UserId", paramID).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<List<PESinglePayerUpdateCheckList>> GetListOfPESinglePayerUpdateCheckList(int ClientID)
        {
            List<PESinglePayerUpdateCheckList> check = new List<PESinglePayerUpdateCheckList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PESinglePayerUpdateCheckList.FromSql("EXEC USP_tblPESinglePayerUpdateCheckList_SelectList @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdatePESinglePayerUpdateCheckList(int ClientID, string Mode, string Status
            , string UserId, string To, string CC, string Subject, string Body, string Attachements, string TaskValueAllocatedTo, string ID)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramTo = new SqlParameter("@To", To);
                var paramCC = new SqlParameter("@CC", CC);
                var paramSubject = new SqlParameter("@Subject", Subject);
                var paramBody = new SqlParameter("@Body", Body);
                var paramAttachements = new SqlParameter("@Attachements", Attachements);
                var paramAllocatedName = new SqlParameter("@TaskAllocatedName", TaskValueAllocatedTo);
                var paramID = new SqlParameter("@ID", ID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPESinglePayerUpdateCheckList_Update @ClientID,@Mode,@Status,@UserId,@To,@CC,@Subject,@Body,@Attachements,@TaskAllocatedName,@ID"
                    , paramClientID, paramMode, paramStatus, paramUserId, paramTo, paramCC, paramSubject, paramBody, paramAttachements, paramAllocatedName,paramID);

            }
            catch (Exception Ex) { }
        }
        public async Task DeletePESinglePayerUpdateCheckList(int ID, int ClientID, string UserId)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramUserId = new SqlParameter("@UserId", UserId);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPESinglePayerUpdateCheckList_Delete @ID,@ClientID,@UserId", paramID, paramClientID, paramUserId);
            }
            catch (Exception Ex) { }
        }

        public async Task<PEPracticeLocationChangeCheckList> GetPEPracticeLocationChangeCheckList(int ClientID, string UserId)
        {
            PEPracticeLocationChangeCheckList check = new PEPracticeLocationChangeCheckList();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            var paramUserId = new SqlParameter("@UserId", UserId);
            try
            {
                check = await _applicationDbContext.PEPracticeLocationChangeCheckList.FromSql("EXEC USP_tblPEPracticeLocationChangeCheckList_Select @ClientID,@UserId", paramClientID, paramUserId).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdatePEPracticeLocationChangeCheckList(int ClientID, string Mode, string Status, string UserId, string To, string CC, string Subject, string Body, string Attachements)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramTo = new SqlParameter("@To", To);
                var paramCC = new SqlParameter("@CC", CC);
                var paramSubject = new SqlParameter("@Subject", Subject);
                var paramBody = new SqlParameter("@Body", Body);
                var paramAttachements = new SqlParameter("@Attachements", Attachements);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEPracticeLocationChangeCheckList_Update @ClientID,@Mode,@Status,@UserId,@To,@CC,@Subject,@Body,@Attachements"
                    , paramClientID, paramMode, paramStatus, paramUserId, paramTo, paramCC, paramSubject, paramBody, paramAttachements);

            }
            catch (Exception Ex) { }
        }
        

        public async Task<PEClosedClientCheckList> GetPEClosedClientCheckList(int ClientID, string UserId)
        {
            PEClosedClientCheckList check = new PEClosedClientCheckList();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            var paramUserId = new SqlParameter("@UserId", UserId);
            try
            {
                check = await _applicationDbContext.PEClosedClientCheckList.FromSql("EXEC USP_tblPEClosedClientCheckList_Select @ClientID,@UserId", paramClientID, paramUserId).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdatePEClosedClientCheckList(int ClientID, string Mode, string Status, string UserId, string To, string CC, string Subject, string Body, string Attachements)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramTo = new SqlParameter("@To", To);
                var paramCC = new SqlParameter("@CC", CC);
                var paramSubject = new SqlParameter("@Subject", Subject);
                var paramBody = new SqlParameter("@Body", Body);
                var paramAttachements = new SqlParameter("@Attachements", Attachements);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEClosedClientCheckList_Update @ClientID,@Mode,@Status,@UserId,@To,@CC,@Subject,@Body,@Attachements"
                    , paramClientID, paramMode, paramStatus, paramUserId, paramTo, paramCC, paramSubject, paramBody, paramAttachements);

            }
            catch (Exception Ex) { }
        }

        public async Task<PEClientBankChangeCheckList> GetPEClientBankChangeCheckList(int ClientID, string UserId)
        {
            PEClientBankChangeCheckList check = new PEClientBankChangeCheckList();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            var paramUserId = new SqlParameter("@UserId", UserId);
            try
            {
                check = await _applicationDbContext.PEClientBankChangeCheckList.FromSql("EXEC USP_tblPEClientBankChangeCheckList_Select @ClientID,@UserId", paramClientID, paramUserId).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdatePEClientBankChangeCheckList(int ClientID, string Mode, string Status, string UserId, string To, string CC, string Subject, string Body, string Attachements)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramTo = new SqlParameter("@To", To);
                var paramCC = new SqlParameter("@CC", CC);
                var paramSubject = new SqlParameter("@Subject", Subject);
                var paramBody = new SqlParameter("@Body", Body);
                var paramAttachements = new SqlParameter("@Attachements", Attachements);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEClientBankChangeCheckList_Update @ClientID,@Mode,@Status,@UserId,@To,@CC,@Subject,@Body,@Attachements"
                    , paramClientID, paramMode, paramStatus, paramUserId, paramTo, paramCC, paramSubject, paramBody, paramAttachements);

            }
            catch (Exception Ex) { }
        }

        public async Task<PECommericalEFTsCheckList> GetPECommericalEFTsCheckList(int ClientID, string UserId)
        {
            PECommericalEFTsCheckList check = new PECommericalEFTsCheckList();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            var paramUserId = new SqlParameter("@UserId", UserId);
            try
            {
                check = await _applicationDbContext.PECommericalEFTsCheckList.FromSql("EXEC USP_tblPECommericalEFTsCheckList_Select @ClientID,@UserId", paramClientID, paramUserId).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdatePECommericalEFTsCheckList(int ClientID, string Mode, string Status, string UserId, string To, string CC, string Subject, string Body, string Attachements)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramTo = new SqlParameter("@To", To);
                var paramCC = new SqlParameter("@CC", CC);
                var paramSubject = new SqlParameter("@Subject", Subject);
                var paramBody = new SqlParameter("@Body", Body);
                var paramAttachements = new SqlParameter("@Attachements", Attachements);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPECommericalEFTsCheckList_Update @ClientID,@Mode,@Status,@UserId,@To,@CC,@Subject,@Body,@Attachements"
                    , paramClientID, paramMode, paramStatus, paramUserId, paramTo, paramCC, paramSubject, paramBody, paramAttachements);

            }
            catch (Exception Ex) { }
        }

        public async Task<PEMedicareChangeOfInfoCheckList> GetPEMedicareChangeOfInfoCheckList(int ClientID, string UserId)
        {
            PEMedicareChangeOfInfoCheckList check = new PEMedicareChangeOfInfoCheckList();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            var paramUserId = new SqlParameter("@UserId", UserId);
            try
            {
                check = await _applicationDbContext.PEMedicareChangeOfInfoCheckList.FromSql("EXEC USP_tblPEMedicareChangeOfInfoCheckList_Select @ClientID,@UserId", paramClientID, paramUserId).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdatePEMedicareChangeOfInfoCheckList(int ClientID, string Mode, string Status, string UserId, string To, string CC, string Subject, string Body, string Attachements)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramTo = new SqlParameter("@To", To);
                var paramCC = new SqlParameter("@CC", CC);
                var paramSubject = new SqlParameter("@Subject", Subject);
                var paramBody = new SqlParameter("@Body", Body);
                var paramAttachements = new SqlParameter("@Attachements", Attachements);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEMedicareChangeOfInfoCheckList_Update @ClientID,@Mode,@Status,@UserId,@To,@CC,@Subject,@Body,@Attachements"
                    , paramClientID, paramMode, paramStatus, paramUserId, paramTo, paramCC, paramSubject, paramBody, paramAttachements);

            }
            catch (Exception Ex) { }
        }

        public async Task<PEOutOfStateMCDCheckList> GetPEOutOfStateMCDCheckList(int ClientID, string UserId)
        {
            PEOutOfStateMCDCheckList check = new PEOutOfStateMCDCheckList();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            var paramUserId = new SqlParameter("@UserId", UserId);
            try
            {
                check = await _applicationDbContext.PEOutOfStateMCDCheckList.FromSql("EXEC USP_tblPEOutOfStateMCDCheckList_Select @ClientID,@UserId", paramClientID, paramUserId).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task UpdatePEOutOfStateMCDCheckList(int ClientID, string Mode, string Status, string UserId, string To, string CC, string Subject, string Body, string Attachements)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramTo = new SqlParameter("@To", To);
                var paramCC = new SqlParameter("@CC", CC);
                var paramSubject = new SqlParameter("@Subject", Subject);
                var paramBody = new SqlParameter("@Body", Body);
                var paramAttachements = new SqlParameter("@Attachements", Attachements);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEOutOfStateMCDCheckList_Update @ClientID,@Mode,@Status,@UserId,@To,@CC,@Subject,@Body,@Attachements"
                    , paramClientID, paramMode, paramStatus, paramUserId, paramTo, paramCC, paramSubject, paramBody, paramAttachements);

            }
            catch (Exception Ex) { }
        }


        #endregion

        #region UserGroup
        public async Task<List<UserGroupList>> GetUserGroup(int id)
        {
            List<UserGroupList> lstUserGroupList = new List<UserGroupList>();
            var GetId = new SqlParameter("@Id", id);
            try
            {
                lstUserGroupList = await _applicationDbContext.UserGroupList.FromSql("EXEC [USP_tblUsersGroup_Select] @Id", GetId).ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return lstUserGroupList;
        }

        public async Task InsertUserGroup(int id, string GroupName, string UserId, bool Active, string CreatedBy)
        {
            var GetId = new SqlParameter("@Id", id);
            var GetGroupName = new SqlParameter("@GroupName", GroupName);
            var GetUserId = new SqlParameter("@UserId", UserId);
            var GetActive = new SqlParameter("@Active", Active);
            var GetCreatedBy = new SqlParameter("@CreatedBy", CreatedBy);
            try
            {
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC [USP_tblUsersGroup_Insert] @Id,@GroupName,@UserId,@Active,@CreatedBy", GetId, GetGroupName, GetUserId, GetActive, GetCreatedBy);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task DeleteUserGroupDetails(int ID)
        {
            try
            {
                var paramContractID = new SqlParameter("@Id", ID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblUsersGroup_Delete @Id", paramContractID);

            }
            catch (Exception Ex) { }
        }

        public async Task<List<UserNameList>> GetUserNameList()
        {
            List<UserNameList> lstUserNameList = new List<UserNameList>();
            try
            {
                lstUserNameList = await _applicationDbContext.UserNameList.FromSql("EXEC [USP_UserNameList_Select]").ToListAsync();
            }
            catch (Exception ex)
            {

            }
            return lstUserNameList;
        }

        #endregion

        #region Instructions Document
        public async Task<List<InstructionsDocument>> GetInstructionsDocumentList()
        {
            List<InstructionsDocument> check = new List<InstructionsDocument>();
            try
            {
                check = await _applicationDbContext.InstructionsDocument.FromSql("EXEC USP_tblInstructionDocuments_Select").ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task DeleteInstructionsDocument(int ID, string userid)
        {
            try
            {
                var paramID = new SqlParameter("@ID", ID);
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblInstructionDocuments_Delete @ID,@userid", paramID, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task InsertInstructionsDocument(ProviderEntrollment providerEntrollment, string userid)
        {
            InstructionsDocument check = new InstructionsDocument();
            try
            {
                var paramInstructionsDocument = new SqlParameter("@InstructionsDocument", providerEntrollment.GetXml());
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblInstructionDocuments_Insert @InstructionsDocument,@userid", paramInstructionsDocument, paramuserid);
            }
            catch (Exception Ex) { }
        }

        #endregion

        #region Notes

        public async Task<List<PENotesList>> GetPENotesList(int ClientID)
        {
            List<PENotesList> check = new List<PENotesList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PENotesList.FromSql("EXEC USP_tblPENotes_Select @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task InsertPENotes(int Id, int ClientID, string Notes, string userid)
        {
            try
            {
                var paramID = new SqlParameter("@ID", Id);
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramNotes = new SqlParameter("@Notes", Notes);
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPENotes_Insert @Id,@ClientID,@Notes,@userid", paramID, paramClientID, paramNotes, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task DeletePENotes(int Id, string userid)
        {
            try
            {
                var paramID = new SqlParameter("@ID", Id);
                var paramuserid = new SqlParameter("@userid", userid);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPENotes_Delete @Id,@userid", paramID, paramuserid);
            }
            catch (Exception Ex) { }
        }

        public async Task<List<PENotesList>> GetPENotesListForExcel(int ClientID)
        {
            List<PENotesList> check = new List<PENotesList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PENotesList.FromSql("EXEC USP_tblPENotes_SelectForExcel @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        
         public async Task<List<PEEFTList>> GetPEEFTListForExcel(int ClientID)
        {
            List<PEEFTList> check = new List<PEEFTList>();
            var paramClientID = new SqlParameter("@ClientID", ClientID);
            try
            {
                check = await _applicationDbContext.PEEFTList.FromSql("EXEC USP_tblPEEFT_SelectForExcel @ClientID", paramClientID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        #endregion


        public async Task<List<ClientsDetailsList>> GetFilterClients(int clientId, int active, int clientStatusId, int erId)
        {
            try
            {
                var result = await _applicationDbContext.ClientsDetailsList.FromSql("EXEC GetFilterClients @ClientId, @Active, @ClientStatusId, @ERId",
                        new SqlParameter("@ClientId", clientId),
                        new SqlParameter("@Active", active),
                        new SqlParameter("@ClientStatusId", clientStatusId),
                        new SqlParameter("@ERId", erId))
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                // Optionally log the exception here
                return new List<ClientsDetailsList>();
            }
        }

        public async Task UpdatePECheckListStatusAllocatedTo(int ClientID, string Mode, string Status, string UserId)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_PECheckList_Update_AllocatedTo @ClientID,@Mode,@Status,@UserId"
                    , paramClientID, paramMode, paramStatus, paramUserId);

            }
            catch (Exception Ex) { }
        }
        public async Task UpdatePEPaymentCategoryChangeCheckListAllocatedTo(int ClientID, string Mode, string Status, string UserId)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEPaymentCategoryChangeCheckList_Update_AllocatedTo @ClientID,@Mode,@Status,@UserId"
                    , paramClientID, paramMode, paramStatus, paramUserId);

            }
            catch (Exception Ex) { }
        }

        public async Task UpdatePESinglePayerUpdateCheckListAllocatedTo(int ClientID, string Mode, string Status, string ChecklistStatus
            , string UserId,  string ID, string TaskValueAllocatedTo)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramChecklistStatus = new SqlParameter("@ChecklistStatus", ChecklistStatus);
                var paramUserId = new SqlParameter("@UserId", UserId);
                var paramAllocatedName = new SqlParameter("@TaskAllocatedName", TaskValueAllocatedTo);
                
                var paramID = new SqlParameter("@ID", ID);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPESinglePayerUpdateCheckList_Update_AllocatedTo @ClientID,@Mode,@Status,@ChecklistStatus,@UserId,@TaskAllocatedName,@ID"
                    , paramClientID, paramMode, paramStatus, paramChecklistStatus, paramUserId, paramAllocatedName,paramID);

            }
            catch (Exception Ex) { }
        }

        public async Task UpdatePEPracticeLocationChangeCheckListAllocatedTo(int ClientID, string Mode, string AllocatedTo, string UserId)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", AllocatedTo);
                var paramUserId = new SqlParameter("@UserId", UserId);
                //var paramAllcoatedTo = new SqlParameter("@AllocatedTo", AllocatedTo);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEPracticeLocationChangeCheckList_UpdateForAllocatedTo @ClientID,@Mode,@Status,@UserId"
                    , paramClientID, paramMode, paramStatus, paramUserId);

            }
            catch (Exception Ex) { }
        }

        public async Task UpdatePEClosedClientCheckListAllocatedTo(int ClientID, string Mode, string Status, string UserId)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEClosedClientCheckList_Update_AllocatedTo @ClientID,@Mode,@Status,@UserId"
                    , paramClientID, paramMode, paramStatus, paramUserId);

            }
            catch (Exception Ex) { }
        }
        public async Task UpdatePEClientBankChangeCheckListAllocated(int ClientID, string Mode, string Status, string UserId)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
               
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEClientBankChangeCheckList_Update_AllocatedTo @ClientID,@Mode,@Status,@UserId"
                    , paramClientID, paramMode, paramStatus, paramUserId);

            }
            catch (Exception Ex) { }
        }

        public async Task UpdatePEMedicareChangeOfInfoCheckListAllocatedTo(int ClientID, string Mode, string Status, string UserId)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
               
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEMedicareChangeOfInfoCheckList_Update_AllocatedTo @ClientID,@Mode,@Status,@UserId"
                    , paramClientID, paramMode, paramStatus, paramUserId);

            }
            catch (Exception Ex) { }
        }

        public async Task UpdatePEOutOfStateMCDCheckListAllocatedTo(int ClientID, string Mode, string Status, string UserId)
        {
            try
            {
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramMode = new SqlParameter("@Mode", Mode);
                var paramStatus = new SqlParameter("@Status", Status);
                var paramUserId = new SqlParameter("@UserId", UserId);
                
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC USP_tblPEOutOfStateMCDCheckList_Update_AllocatedTo @ClientID,@Mode,@Status,@UserId"
                    , paramClientID, paramMode, paramStatus, paramUserId);

            }
            catch (Exception Ex) { }
        }


        public async Task PEEFTFileUpload(string xmlData)
        {
            var xmlParam = new SqlParameter("@XmlData", SqlDbType.Xml)
            {
                Value = xmlData
            };

            await _applicationDbContext.Database.ExecuteSqlCommandAsync(
                "EXEC USP_PEEFTFileUpload_Insert @XmlData", xmlParam
            );
        }
    }
}

