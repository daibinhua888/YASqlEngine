using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASqlEngine.Core
{
    public class ColumnExpression
    {
        private ExpressionType expType;
        public ColumnExpression(ExpressionType expType)
        {
            this.expType = expType;
        }

        public ExpressionType ExpressionType
        {
            get
            {
                return this.expType;
            }
        }

        public FunctionDescription Function { get; set; }
        public string ColumnName { get; set; }
    }
}
