using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASqlEngine.Core
{
    public enum WhereConditionNodeType
    {
        Statement,
        Condition
    }

    public class WhereCondition
    {
        public WhereCondition(WhereConditionNodeType nodeType)
        {
            this.NodeType = nodeType;
        }

        public WhereConditionNodeType NodeType { get; set; }

        public WhereCondition Statement_LeftNode { get; set; }
        public string Statement_Operator { get; set; }
        public WhereCondition Statement_RightNode { get; set; }

        public string Condition_LeftExpression { get; set; }
        public string Condition_Operator { get; set; }
        public string Condition_RightExpression { get; set; }

        public int TotalCount
        {
            get
            {
                if (this.NodeType == WhereConditionNodeType.Condition)
                    return 1;

                int count = 0;

                if (this.Statement_LeftNode != null)
                    count += Statement_LeftNode.TotalCount;

                if (this.Statement_RightNode != null)
                    count += Statement_RightNode.TotalCount;

                return 1 + count;
            }
        }
    }
}
