using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WiseX.Helpers;

namespace PED.ViewModels.Admin
{
    public class Position : EntityBase
    {
        [Key]
        public string Id { get; set; }
        public PositionDetails PositionDetails;
        public List<PositionList> PositionList;
    }
    public class PositionDetails : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public byte Active { get; set; }
    }
    public class PositionList
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Active { get; set; }

    }
}
