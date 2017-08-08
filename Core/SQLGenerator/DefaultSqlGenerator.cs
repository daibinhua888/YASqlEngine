using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASqlEngine.Core.SQLGenerator
{
    public class DefaultSqlGenerator:IGenerator
    {
        public string Generate(SelectStmtInfo stmtInfo)
        {
            string predictWord = "";
            string columnSql = "";
            string tableName = "";
            string orderBySql = "";
            string whereSql = "";
            string finalSql = "";

            predictWord = Generate_PredictWord(stmtInfo);
            tableName = Generate_TableName(stmtInfo);
            columnSql = Generate_ColumnSql(stmtInfo);
            orderBySql = Generate_OrderBySql(stmtInfo);
            whereSql = Generate_WhereSql(stmtInfo);

            if(string.IsNullOrEmpty(predictWord))
                finalSql = string.Format("SELECT {1} FROM {2}{3}{4}", predictWord, columnSql, tableName, whereSql, orderBySql);
            else
                finalSql = string.Format("SELECT {0} {1} FROM {2}{3}{4}", predictWord, columnSql, tableName, whereSql, orderBySql);

            return finalSql;
        }

        private static string Generate_WhereSql(SelectStmtInfo stmtInfo)
        {
            if (stmtInfo.WhereCondition == null)
                return string.Empty;

            var conditions=_Generate_WhereSql(stmtInfo.WhereCondition);

            if(string.IsNullOrEmpty(conditions))
                return string.Empty;

            return string.Format(" WHERE {0}", conditions);
        }

        private static string _Generate_WhereSql(WhereCondition conditionNode)
        {
            StringBuilder sb = new StringBuilder();

            if (conditionNode.NodeType == WhereConditionNodeType.Condition)
            {
                string conditionSql = string.Format("{0}{1}{2}", conditionNode.Condition_LeftExpression,
                                                                    conditionNode.Condition_Operator,
                                                                    conditionNode.Condition_RightExpression);

                sb.Append(conditionSql);
            }
            else
            {
                string leftConditionSql = _Generate_WhereSql(conditionNode.Statement_LeftNode);
                string op = conditionNode.Statement_Operator;
                string rightConditionSql = _Generate_WhereSql(conditionNode.Statement_RightNode);

                sb.AppendFormat("({0}) {1} ({2})", leftConditionSql, op, rightConditionSql);
            }

            return sb.ToString();
        }

        private static string Generate_OrderBySql(SelectStmtInfo stmtInfo)
        {
            StringBuilder sb = new StringBuilder();

            if (stmtInfo.OrderBy != null && stmtInfo.OrderBy.Count > 0)
            {
                sb.Append(" ORDER BY ");

                foreach(var orderByTerm in stmtInfo.OrderBy)
                    sb.AppendFormat("{0} {1}, ", orderByTerm.Expression, orderByTerm.Direction.ToString());
            }

            return sb.ToString().TrimEnd(", ".ToCharArray());
        }

        private static string Generate_ColumnSql(SelectStmtInfo stmtInfo)
        {
            StringBuilder sb = new StringBuilder();

            foreach(var column in stmtInfo.Columns)
            {
                string cur_sql = string.Empty;

                cur_sql = column.Expression.ColumnName;

                if(column.HasAlias)
                    cur_sql+=string.Format(" AS {0}", column.Alias);

                sb.AppendFormat("{0}, ", cur_sql);
            }

            return sb.ToString().TrimEnd(", ".ToCharArray());
        }

        private static string Generate_TableName(SelectStmtInfo stmtInfo)
        {
            string sql=stmtInfo.TableDescriptor.TableName;

            switch (stmtInfo.TableDescriptor.TableReadType)
            { 
                case TableReadType.NONE:
                    //Nothing to do
                    break;
                case TableReadType.NOLOCK:
                    sql += "(NOLOCK)";
                    break;
                case TableReadType.READPAST:
                    sql += "(READPAST)";
                    break;
                default:
                    throw new NotSupportedException();
                    break;
            }

            return sql;
        }

        private static string Generate_PredictWord(SelectStmtInfo stmtInfo)
        {
            string predictWord = "";

            if (stmtInfo.Column_PredictExists)
                predictWord = stmtInfo.Column_PredictWord;

            return predictWord;
        }
    }
}
