using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace PED.ViewModels.Admin
{
    public class UserGroup : EntityBase
    {
        [Key]
        public string Id { get; set; }
        public UserGroupDet UserGroupDet;
        public List<UserGroupList> UserGroupList;

        public List<UserNameList> UserNameList;
    }
    public class UserGroupDet : EntityBase
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string UserId { get; set; }
        public bool Active { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
    public class UserGroupList
    {
        [Key]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Active { get; set; }
    }

    public class UserNameList
    {
        [Key]
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
