using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WiseX.Data;
using WiseX.Models;
using System;
using System.Data;
using PED.ViewModels.Contract;
using PED.ViewModels.Admin;
using WiseX.ViewModels.Admin;
using System.Linq;

namespace WiseX.Services
{
    public class CommonService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        string connectionString = string.Empty;

        public CommonService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public bool CheckMenuAccess(string ActionName, string ControllerName, string RoleID)
        {
            bool resultVal = false;
            //var resultVal = 0;
            try
            {
                var ParamActionName = new SqlParameter("@ActionName", ActionName);
                var ParamControllerName = new SqlParameter("@ControllerName", ControllerName);
                var ParamRoleID = new SqlParameter("@RoleID", RoleID);
                var paramOutput = new SqlParameter()
                {
                    ParameterName = "@Result",
                    Direction = ParameterDirection.Output,
                    SqlValue = SqlDbType.Bit
                };

                _applicationDbContext.Database.ExecuteSqlCommand("EXEC CheckMenuAccess @ActionName, @ControllerName, @RoleID, @Result OUTPUT",
                    ParamActionName, ParamControllerName, ParamRoleID, paramOutput);
                resultVal = Convert.ToBoolean(paramOutput.Value);
            }
            catch (Exception Ex)
            {

            }

            return resultVal;
        }

        public async Task<int> UpdateUserSession(UserSession userSessionDetails)
        {
            int ReturnID = 0;
            try
            {
                var paramUserSessionDetails = new SqlParameter("@UserSessionDetails", userSessionDetails.GetXml());
                SqlParameter paramOutput = new SqlParameter()
                {
                    ParameterName = "@ReturnID",
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 50
                };

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateSessionDetails @UserSessionDetails, @ReturnID OUTPUT", paramUserSessionDetails, paramOutput);
                ReturnID = Convert.ToInt32(paramOutput.Value);
            }
            catch (Exception Ex)
            {

            }
            return ReturnID;
        }
        public async Task<List<ContractStatus>> GetContractStatusList(int id)
        {
            List<ContractStatus> ListStatus = new List<ContractStatus>();

            try
            {
                var ParamId = new SqlParameter("@Id", id);
                ListStatus = await _applicationDbContext.ContractStatus.FromSql("EXEC ContractStatus @Id", ParamId).AsNoTracking().ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return ListStatus;

        }
        public async Task<List<ContractDetails>> GetContractDetailsList(int Start, int Length,string UserID)
        {
            List<ContractDetails> ListDetails = new List<ContractDetails>();

            try
            {
                var ParamStart = new SqlParameter("@Start", Start);
                var ParamLength = new SqlParameter("@Length", Length);
                var ParamUserID = new SqlParameter("@UserID", UserID);
                ListDetails = await _applicationDbContext.ContractDetails.FromSql("EXEC GetContractDetailsList @Start,@Length,@UserID", ParamStart, ParamLength, ParamUserID).AsNoTracking().ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return ListDetails;

        }
        public async Task<List<ClientContractDetail>> GetClientContractDetailsList(int Start, int Length, int ClientID)
        {
            List<ClientContractDetail> ListDetails = new List<ClientContractDetail>();
            try
            {
                var ParamStart = new SqlParameter("@Start", Start);
                var ParamLength = new SqlParameter("@Length", Length);
                var paramClientID = new SqlParameter("@ClientId", ClientID);
                ListDetails = await _applicationDbContext.ClientContractDetail.FromSql("EXEC GetClientContractDetailsList @Start,@Length,@ClientId", ParamStart, ParamLength, paramClientID).AsNoTracking().ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return ListDetails;

        }
        public async Task<List<ClientContractDetail>> GetClientContractDetails(int companyId)
        {

            List<ClientContractDetail> chkList = new List<ClientContractDetail>();
            var CId = new SqlParameter("@ClientId", companyId);
            try
            {
                chkList = await _applicationDbContext.ClientContractDetail.FromSql("EXEC GetClientContractDetails @ClientId", CId).AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return chkList;
        }
        public async Task<List<ContractAgreementType>> GetContractAgreementType()
        {
            List<ContractAgreementType> check = new List<ContractAgreementType>();
            try
            {
                var ParamID = new SqlParameter("@Id", 1);
                check = await _applicationDbContext.ContractAgreementTypeList.FromSql("EXEC GetMasterTableData @Id", ParamID).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<List<ContractUSers>> GetUsers(string UserId)
        {
            List<ContractUSers> check = new List<ContractUSers>();
            try
            {
                var ParamID = new SqlParameter("@Id", 8);
                var paramUserid = new SqlParameter("@UserID", UserId);
                check = await _applicationDbContext.ContractUSersList.FromSql("EXEC GetMasterTableData @Id,@UserID", ParamID, paramUserid).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<List<SMTPServerDetails>> GetSMTPServerDetails(string UserId)
        {
            List<SMTPServerDetails> SMTP = new List<SMTPServerDetails>();
            try
            {
                var paramUserId = new SqlParameter("@UserId", UserId);
               SMTP = await _applicationDbContext.SMTPServerDetails.FromSql("EXEC GetSMTPServerDetails @UserId", paramUserId).ToListAsync();
            }
            catch (Exception Ex) { }
            return SMTP;
        }
        public async Task<UserOtherDetails> GetUserOtherDetails(int UserId)
        {
            UserOtherDetails userOtherDetails = new UserOtherDetails();
            try
            {
                var paramUserId = new SqlParameter("@UserId", UserId);
                userOtherDetails = await _applicationDbContext.UserOtherDetails.FromSql("EXEC GetUserOtherDetails @UserId", paramUserId).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return userOtherDetails;
        }

        public async Task<int> UpdateContractDetails(ClientContractDetails contracts, string UserId)
        {
            int ContractId = 0;
            try
            {
                var paramcontracts = new SqlParameter("@Contracts", contracts.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);
                var paramOutput = new SqlParameter()
                {
                    ParameterName = "@ContractId",
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 50
                };
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateContractDetails @Contracts,@UId,@ContractId OUTPUT", paramcontracts, paramUserId, paramOutput);
                ContractId = Convert.ToInt32(paramOutput.Value);
            }
            catch (Exception Ex) { }

            return ContractId;
        }
        public async Task<int> UpdateEditContractDetails(ClientContractEditDetails contracts, string UserId)
        {
            int ContractId = 0;
            try
            {
                var paramcontracts = new SqlParameter("@Contracts", contracts.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);
                var paramOutput = new SqlParameter()
                {
                    ParameterName = "@ContractId",
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 50                    
                };
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateContractEditDetails @Contracts, @UId, @ContractId OUTPUT", paramcontracts, paramUserId, paramOutput);
                ContractId = Convert.ToInt32(paramOutput.Value);
            }
            catch (Exception Ex) { }
            return ContractId;
        }
        public async Task UpdateNotesDetails(NotesDetails notesDetails, string UserId)
        {
            try
            {
                var paramnotes = new SqlParameter("@Notes", notesDetails.GetXml());
                var paramUserId = new SqlParameter("@UId", UserId);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateNotesDetails @Notes,@UId", paramnotes, paramUserId);
            }
            catch (Exception Ex) { }

        }

        public async Task UpdateClientContarctNotesDetails(int Id,int ClientID, int ContractId,int InsuranceContractId, string Notes, string UserId)
        {
            try
            {
                if(Notes == null)
                {
                    Notes = "";
                }

                var paramId = new SqlParameter("@Id", Id);
                var paramClientID = new SqlParameter("@ClientID", ClientID);
                var paramContractId = new SqlParameter("@ContractId", ContractId);
                var paramInsuranceContractId = new SqlParameter("@InsuranceContractId", InsuranceContractId);
                var paramNotes = new SqlParameter("@Notes", Notes);
                var paramUserId = new SqlParameter("@UserId", UserId);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateClientContractNotesDetails @Id,@ClientID,@ContractId,@InsuranceContractId,@Notes,@UserId", paramId , paramClientID, paramContractId,paramInsuranceContractId, paramNotes, paramUserId);
            }
            catch (Exception Ex) { }

        }

        public async Task UpdateViewHistroyDetails(int companyId, string Version, int ContractId, string UserId,string Status)
        {
            try
            {
                var paramcompanyId = new SqlParameter("@companyId", companyId);
                var paramVersion = new SqlParameter("@Version", Version);
                var paramContractId = new SqlParameter("@ContractId", ContractId);
                var paramUserId = new SqlParameter("@UId", UserId);
                var paramStatus = new SqlParameter("@Status", Status);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateViewHistroyDetails @companyId,@Version,@ContractId,@UId,@Status", paramcompanyId, paramVersion, paramContractId, paramUserId, paramStatus);
            }
            catch (Exception Ex) { }

        }

       

        public async Task<int> SelectContractScreen(int ClientId)
        {
            int Dt = 0;
            try
            {
                var paramClientId = new SqlParameter("@ClientId", ClientId);
                var paramOutput = new SqlParameter()
                {
                    ParameterName = "@Version",
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 50
                };
               await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC GetContractVersion @ClientId,@Version OUTPUT", paramClientId, paramOutput);
                Dt = Convert.ToInt32(paramOutput.Value);
            }
            catch (Exception Ex)
            {
            }
            return Dt;
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
        public async Task UpdateContractFileDetails(ContractFileDetails model)
        {
            try
            {
                var details = new SqlParameter("@Data", model.GetXml());
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateContractFileDetails @Data", details);
                
            }
            catch (Exception Ex)
            {

            }
        }

        public async Task UpdateClientContractOtheFiles(ContractFileDetails model)
        {
            try
            {
                var details = new SqlParameter("@Data", model.GetXml());
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateContractOtherFileDetails @Data", details);

            }
            catch (Exception Ex)
            {

            }
        }
        public async Task UpdateClientContractPassThruFiles(ContractFileDetails model)
        {
            try
            {
                var details = new SqlParameter("@Data", model.GetXml());
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateContractPassThruFileDetails @Data", details);

            }
            catch (Exception Ex)
            {

            }
        }
        public async Task UpdateClientContractCrewLogFiles(ContractFileDetails model)
        {
            try
            {
                var details = new SqlParameter("@Data", model.GetXml());
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateContractCrewLogFileDetails @Data", details);

            }
            catch (Exception Ex)
            {

            }
        }

        public async Task<Contractdetails> GetContractDetails(int contractId)
        {
            Contractdetails check = new Contractdetails();
            try
            {
            
                var ParamID = new SqlParameter("@Id", contractId);
                check = await _applicationDbContext.Contractdetails.FromSql("EXEC GetContractDetails @Id", ParamID).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }


        public async Task<List<SearchClient>> GetSearchClient(string Prefix)
        {
            List<SearchClient> lst = new List<SearchClient>();
            try
            {
                var Param = new SqlParameter("@SearchTerm", Prefix);
                lst = await _applicationDbContext.SearchClient.FromSql("EXEC GetSearchClient @SearchTerm", Param).ToListAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }

        public async Task<List<SearchClientEmployee>> GetSearchClientEmployee(string Prefix,int clientId,string MailTo)
        {
            List<SearchClientEmployee> lst = new List<SearchClientEmployee>();
            try
            {
                var Param = new SqlParameter("@SearchTerm", Prefix);
                var ParamClientId = new SqlParameter("@clientId", clientId);
                var ParamMailTo = new SqlParameter("@MailTo", MailTo);

                lst = await _applicationDbContext.SearchClientEmployee.FromSql("EXEC GetSearchClientEmployee @SearchTerm,@clientId,@MailTo", Param, ParamClientId, ParamMailTo).ToListAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }

        public async Task<int> DeleteContract(int ContractId)
        {
            int ClientID = 0;
            try
            {
                var param = new SqlParameter("@ContractId", ContractId);
                SqlParameter paramOutput = new SqlParameter()
                {
                    ParameterName = "@ClientID",
                    Direction = System.Data.ParameterDirection.Output,
                    Size = 500
                };

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteContract @ContractId, @ClientID OUTPUT", param, paramOutput);
                ClientID = Convert.ToInt32(paramOutput.Value);
            }
            catch (Exception Ex)
            {

            }
            return ClientID;
        }

        public async Task<int> DeleteOtherDocument(int ID,string UserID)
        {
            int ClientID = 0;
            try
            {
                var param = new SqlParameter("@ID", ID);
                var paramUserID = new SqlParameter("@UserID", UserID);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteOtherDocument @ID,@UserID", param,paramUserID);
                
            }
            catch (Exception Ex)
            {

            }
            return ClientID;
        }

        public async Task<int> DeletePassThruContract(int ID, string UserID)
        {
            int ClientID = 0;
            try
            {
                var param = new SqlParameter("@ID", ID);
                var paramUserID = new SqlParameter("@UserID", UserID);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeletePassThruContract @ID,@UserID", param, paramUserID);

            }
            catch (Exception Ex)
            {

            }
            return ClientID;
        }
        public async Task<int> DeleteCrewLog(int ID, string UserID)
        {
            int ClientID = 0;
            try
            {
                var param = new SqlParameter("@ID", ID);
                var paramUserID = new SqlParameter("@UserID", UserID);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteCrewLog @ID,@UserID", param, paramUserID);

            }
            catch (Exception Ex)
            {

            }
            return ClientID;
        }

        public async Task<int> DeleteClientContarctNotes(int ID, string UserID)
        {
            int ClientID = 0;
            try
            {
                var param = new SqlParameter("@ID", ID);
                var paramUserID = new SqlParameter("@UserID", UserID);

                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC DeleteClientContarctNotes @ID,@UserID", param, paramUserID);

            }
            catch (Exception Ex)
            {

            }
            return ClientID;
        }
        public async Task <AEDetailsList> GetAEDetails(int ContractId)
        {
            AEDetailsList lst = new AEDetailsList();
            try
            {
                var Param = new SqlParameter("@ContractId", ContractId);
                lst = await _applicationDbContext.AEDetailsList.FromSql("EXEC GetAEDetails @ContractId", Param).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return lst;
        }
        public async Task UpdateContracMailDetails(EmailContractDetails emailContractDetails)
        {
            try
            {
                var details = new SqlParameter("@EmailContractDetails", emailContractDetails.GetXml());
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateContractMailDetails @EmailContractDetails", details);

            }
            catch (Exception Ex)
            {

            }
        }
        public async Task<List<ContractDetailApprovalList>> GetContractDetailApprovalList(int companyId)
        {

            List<ContractDetailApprovalList> chkList = new List<ContractDetailApprovalList>();
            var CId = new SqlParameter("@ClientId", companyId);
            try
            {
                chkList = await _applicationDbContext.ContractDetailApprovalList.FromSql("EXEC GetContractDetailApprovalList @ClientId", CId).AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return chkList;
        }
        public async Task<List<ContractLogList>> GetContractLogList(int ContractId)
        {

            List<ContractLogList> chkList = new List<ContractLogList>();
            var CId = new SqlParameter("@ContractId", ContractId);
            try
            {
                chkList = await _applicationDbContext.ContractLogList.FromSql("EXEC GetContractLog @ContractId", CId).AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return chkList;
        }

        public async Task<List<ClientContractOtherDocumentDetails>> GetClientContractOtherDocument(int ContractId)
        {

            List<ClientContractOtherDocumentDetails> chkList = new List<ClientContractOtherDocumentDetails>();
            var CId = new SqlParameter("@ContractId", ContractId);
            try
            {
                chkList = await _applicationDbContext.ClientContractOtherDocumentDetails.FromSql("EXEC GetClientContractOtherDocumentList @ContractId", CId).AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return chkList;
        }

        public async Task<List<ClientContractPassThruContractDetails>> GetClientContractPassThruContract(int ContractId)
        {

            List<ClientContractPassThruContractDetails> chkList = new List<ClientContractPassThruContractDetails>();
            var CId = new SqlParameter("@ContractId", ContractId);
            try
            {
                chkList = await _applicationDbContext.ClientContractPassThruContractDetails.FromSql("EXEC GetClientContractPassThruContractList @ContractId", CId).AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return chkList;
        }
        public async Task<List<ClientContractCrewLogDetails>> GetClientContractCrewLogList(int ContractId)
        {

            List<ClientContractCrewLogDetails> chkList = new List<ClientContractCrewLogDetails>();
            var CId = new SqlParameter("@ContractId", ContractId);
            try
            {
                chkList = await _applicationDbContext.ClientContractCrewLogDetails.FromSql("EXEC GetClientContractCrewLogList @ContractId", CId).AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return chkList;
        }


        public async Task<List<ClientContractNotesDetails>> GetClientContractNotesDetails(int ClientId)
        {

            List<ClientContractNotesDetails> chkList = new List<ClientContractNotesDetails>();
            var CId = new SqlParameter("@ClientId", ClientId);
            try
            {
                chkList = await _applicationDbContext.ClientContractNotesDetails.FromSql("EXEC GetClientContractOtherNotes @ClientId", CId).AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return chkList;
        }

        public async Task<List<DownloadContractNotes>> ExportContractNotes(int ClientId)
        {
            List<DownloadContractNotes> check = new List<DownloadContractNotes>();
            var CId = new SqlParameter("@ClientId", ClientId);
            try
            {
                check = await _applicationDbContext.DownloadContractNotes.FromSql("EXEC usp_ExportClientContractNotes @ClientId", CId).ToListAsync();
            }
            catch (Exception Ex) { }
            return check;
        }

        public async Task<List<ClientContractNotesDetails>> GetClientContractNote(int Id)
        {

            List<ClientContractNotesDetails> chkList = new List<ClientContractNotesDetails>();
            var CId = new SqlParameter("@Id", Id);
            try
            {
                chkList = await _applicationDbContext.ClientContractNotesDetails.FromSql("EXEC GetEditClientContractNote @Id", CId).AsNoTracking().ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return chkList;
        }

        public async Task<RolePermissions> GetRoleAccessControls(string UserId)
        {
            RolePermissions check = new RolePermissions();
            try
            {
                var UID = new SqlParameter("@UserId", UserId);
                check = await _applicationDbContext.RolePermissions.FromSql("EXEC GetRoleAccessControls @UserId", UID).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return check;
        }
        public async Task<int> GetNotification(string UserId)
        {
            int NotificationCount = 0;
            try
            {

                var ParamNotificationCount = new SqlParameter("@UserId", UserId);
                NotificationCount = await _applicationDbContext.NotificationCount.FromSql("EXEC GetNotification @UserId",ParamNotificationCount).Select(ff=>ff.NCount).SingleAsync();
               
            }
            catch (Exception Ex)
            {

            }
            return NotificationCount;
        }
        public async Task<List<NotificationDetails>> GetNotificationDetailsList(string UserId)
        {

            List<NotificationDetails> chkList = new List<NotificationDetails>();
            var paramUserId = new SqlParameter("@UserId", UserId);
            try
            {
                chkList = await _applicationDbContext.NotificationDetailsList.FromSql("EXEC GetNotificationDetails @UserId", paramUserId).AsNoTracking()
                .ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return chkList;
        }

        public async Task<List<CommentsDetailsList>> GetCommentsList(int Start, int Length)
        {
            List<CommentsDetailsList> ListDetails = new List<CommentsDetailsList>();
            try
            {
                var ParamStart = new SqlParameter("@Start", Start);
                var ParamLength = new SqlParameter("@Length", Length);
                ListDetails = await _applicationDbContext.CommentsDetailsList.FromSql("EXEC GetCommentsList @Start,@Length", ParamStart, ParamLength).AsNoTracking().ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return ListDetails;
        }

        public async Task<UserSessionDetails> GetUserSessionDetails(string UserId)
        {
            UserSessionDetails userSession = new UserSessionDetails();
            try
            {
                var UID = new SqlParameter("@UserId", UserId);
                userSession = await _applicationDbContext.UserSessionDetails.FromSql("EXEC GetUserSessionDetails @UserId", UID).FirstOrDefaultAsync();
            }
            catch (Exception Ex) { }
            return userSession;
        }
        public async Task UpdateContractStatus(int Id, int CompanyID, string Notes)
        {
            try
            {
                var ID = new SqlParameter("@ID", Id);
                var CompanyId = new SqlParameter("@CompanyId", CompanyID);
                var NotesData = new SqlParameter("@NotesData", Notes);
                await _applicationDbContext.Database.ExecuteSqlCommandAsync("EXEC UpdateContractStatusDetails @ID,@CompanyId,@NotesData", ID, CompanyId, NotesData);
            }
            catch (Exception Ex)
            {

            }
        }

        #region Insurance Contract
        public async Task<List<ClientInsuranceContractFileDetails>> GetClientInsuranceContractFileDetailsList(int ClientID)
        {
            List<ClientInsuranceContractFileDetails> ListDetails = new List<ClientInsuranceContractFileDetails>();
            try
            {
                var paramClientID = new SqlParameter("@ClientId", ClientID);
                ListDetails = await _applicationDbContext.ClientInsuranceContractFileDetails.FromSql("EXEC GetClientInsuranceContractFileDetailsList @ClientId", paramClientID).AsNoTracking().ToListAsync();
            }
            catch (Exception Ex)
            {

            }
            return ListDetails;

        }
        #endregion 
    }
}
