using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASqlEngine.Core
{
    public enum OrderByDirection
    { 
        ASC,
        DESC
    }

    public class OrderByCondition
    {
        public string Expression { get; set; }
        public OrderByDirection Direction { get; set; }
    }
}
