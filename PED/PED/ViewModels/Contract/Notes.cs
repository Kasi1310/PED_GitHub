using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace PED.ViewModels.Contract
{
    public class Notes : EntityBase
    {
        public CommentsDetails CommentsDetails;
        public List<CommentsDetailsList> CommentsDetailsList;
    }
    public class CommentsDetails
    {
        [Key]
        public int ID { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Comments { get; set; }
        public string Active { get; set; }
        public int TotalCount { get; set; }
    }

    public class CommentsDetailsList
    {
        [Key]
        public int ID { get; set; }
        public string Comments { get; set; }
        public int TotalCount { get; set; }
    }
}
