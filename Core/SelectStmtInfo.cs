using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASqlEngine.Core
{
    public class SelectStmtInfo
    {
        public TableDescriptor TableDescriptor { get; set; }
        public List<Column> Columns { get; set; }
        public WhereCondition WhereCondition { get; set; }
        public List<OrderByCondition> OrderBy { get; set; }

        public bool Column_PredictExists { get; set; }
        public string Column_PredictWord { get; set; }

        public SelectStmtInfo()
        {
            this.Columns = new List<Column>();
            this.OrderBy = new List<OrderByCondition>();
        }
    }
}
