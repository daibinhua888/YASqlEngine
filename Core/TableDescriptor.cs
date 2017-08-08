using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASqlEngine.Core
{
    public enum TableReadType
    { 
        NONE,
        NOLOCK,
        READPAST
    }

    public class TableDescriptor
    {
        public string TableName { get; set; }

        public TableReadType TableReadType { get; set; }
    }
}
