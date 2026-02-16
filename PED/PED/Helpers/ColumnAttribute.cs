using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiseX.Helpers
{
    public class ColumnAttribute : Attribute
    {
        public bool Exclude { get; set; }
    }

    public class ColumnAlias : Attribute
    {
        public string Name { get; set; }
    }
}
