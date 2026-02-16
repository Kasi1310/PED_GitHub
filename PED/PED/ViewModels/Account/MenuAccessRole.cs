using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WiseX.ViewModels.Account
{
    public class MenuAccessRole
    {
        public int Id { get; set; }
        public string MenuItem { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int OrderNo { get; set; }
        public int parent { get; set; }
        public int MenuId { get; set; }
        public string UrlImage { get; set; }
        //public int IsNotification { get; set; }
        public int NotificationCount { get; set; }
    }
    public class BroadcastMessageDetails
    {
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
    }

    public class MenuList
    {
       public List<MenuAccessRole> MenuAccessRole;
    }
    
}
