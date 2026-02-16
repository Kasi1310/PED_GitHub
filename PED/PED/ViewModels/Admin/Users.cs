using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WiseX.Helpers;
using WiseX.Models;

namespace WiseX.ViewModels.Admin
{

    public class Users: EntityBase
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } 
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public int newVlaue { get; set; }
        public bool IsValidUser { get; set; }
        public UserDetails UserDetails;
        public UserDetailsTemp info;
        public Roles Roles;
        public List<UserListInfo> UserList; 
        public List<UserDetailsTemp> userDetailsTemp;
        public List<Roles> RolesList;
        public List<Client> ClientList;
        public bool activeResource = true;
        public Users()
        {
            UserDetails = new UserDetails();
        }   
    } 
    
    public class UserDetails: EntityBase
    {
        
        [Key] 
        public string UserID { get; set; }
        public int UserDetailId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public List<ClientRoles> ClientRoles;
        public string RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Active { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public byte Permissions { get; set; }
        public string Client { get; set; }
    }
    //To Display the details in the list format
    public class UserListInfo
    { 
        [Key]
        public int Id { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }  
        public string Active { get; set; } 
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        
    }
    //For Filling the Edit Button Details in the _UserForm
    public class UserDetailsTemp
    {
        [Key]
        public string UserID { get; set; } 
        public int UserDetailId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string UserRole { get; set; }
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Active { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string RoleID { get; set; }
        public byte Permissions { get; set; }

    }
   
    public class ClientRoles
    {
        public string ClientId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
   
   
    public class ProjectDetailsUsers
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
        public string RoleID { get; set; }
        public bool Selected { get; set; }
        public int AuditSamplePercent { get; set; }
        public int NormsPerDay { get; set; }
      
    }
     
    public class GetUserRoles
    {
        [Key]
        public string ROWID { get; set; }
        public string UserId { get; set; }
        
    }
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }

    }
}
