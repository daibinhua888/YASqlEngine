using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASqlEngine.Core
{
    public class Column
    {
        public ColumnExpression Expression { get; set; }
        public bool HasAlias { get; set; }
        public string Alias { get; set; }
    }
}
