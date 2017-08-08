using YASqlEngine.Core.Exceptions;
using YASqlEngine.Grammars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YASqlEngine.Core
{
    public class SelectSQLTreeListener : SelectSQLBaseListener
    {
        public SelectStmtInfo SelectStmt { get; set; }

        public SelectSQLTreeListener()
        {
            this.SelectStmt = new SelectStmtInfo();
        }


        public override void EnterFrom(SelectSQLParser.FromContext context)
        {
            var table = context.table();
            var tableLockType = context.tableLockType();

            if (table == null || table.IsEmpty)
                throw new MissingTableNameException();

            var tableName = table.GetText();

            if (string.IsNullOrEmpty(tableName))
                throw new MissingTableNameException();

            TableReadType readType = TableReadType.NONE;
            if (tableLockType != null && !tableLockType.IsEmpty)
            {
                var nolockOption = tableLockType.NOLOCK();
                var readpastOption = tableLockType.READPAST();

                if (nolockOption != null)
                    readType = TableReadType.NOLOCK;
                else if (readpastOption != null)
                    readType = TableReadType.READPAST;
            }

            this.SelectStmt.TableDescriptor = new TableDescriptor() { TableName = tableName, TableReadType = readType };
        }

        public override void EnterTable(SelectSQLParser.TableContext context)
        {
            var tableName = context.identity().GetText();

            if (string.IsNullOrEmpty(tableName))
                throw new MissingTableNameException();
        }

        public override void EnterColumn(SelectSQLParser.ColumnContext context)
        {
            Column column = new Column();

            if (context.columnExpression().IsEmpty)     //不存在列表达式
                throw new MissingColumnExpressionException();

            var functionalColumn = context.columnExpression().functionableColumn();
            var columnName = context.columnExpression().columnName();

            if (functionalColumn!=null&&!functionalColumn.IsEmpty)
            {
                column.Expression = new ColumnExpression(ExpressionType.Function);
                column.Expression.Function = new FunctionDescription();
            }
            else if (columnName!=null&&!columnName.IsEmpty)
            {
                column.Expression = new ColumnExpression(ExpressionType.ColumnName);
                column.Expression.ColumnName = columnName.GetText();
            }
            else
            {
                throw new MissingColumnExpressionException();
            }

            if (context.AS() != null)           //存在别名
            {
                columnName = context.columnName();

                if (columnName.IsEmpty)         //语法错误
                    throw new MissingColumnAliasException();

                column.HasAlias = true;
                column.Alias = columnName.GetText();
            }

            this.SelectStmt.Columns.Add(column);
        }

        public override void EnterSelect(SelectSQLParser.SelectContext context)
        {
            if (context.COLUMN_PREDICT() != null)
            { 
                this.SelectStmt.Column_PredictExists=true;
                this.SelectStmt.Column_PredictWord = context.COLUMN_PREDICT().GetText();
            }
        }

        public override void EnterWhereStmts(SelectSQLParser.WhereStmtsContext context)
        {
            var whereStmt = context.whereStmt();

            if (whereStmt == null || whereStmt.IsEmpty)
                throw new MissingWhereConditionException();


            this.SelectStmt.WhereCondition = GenerateStatementNode(whereStmt);
        }

        public override void EnterOrderByStmt(SelectSQLParser.OrderByStmtContext context)
        {
            var orderBy = new OrderByCondition();

            var expression = context.columnExpression();
            if (expression == null || expression.IsEmpty)
                throw new MissingOrderByException();

            orderBy.Expression = expression.GetText();

            if (string.IsNullOrEmpty(orderBy.Expression))
                throw new MissingOrderByException();


            orderBy.Direction = OrderByDirection.DESC;

            var direction = context.orderByDirection();
            if (direction == null || direction.IsEmpty)
                orderBy.Direction = OrderByDirection.ASC;
            else if(direction.GetText().Equals("asc", StringComparison.OrdinalIgnoreCase))
                orderBy.Direction = OrderByDirection.ASC;

            this.SelectStmt.OrderBy.Add(orderBy);
        }





        private static WhereCondition GenerateStatementNode(SelectSQLParser.WhereStmtContext whereStmt)
        {
            var andOr = whereStmt.AND_OR();
            var subWhereStmts = whereStmt.whereStmt();
            var whereCondition = whereStmt.whereCondition();

            if (whereCondition != null && !whereCondition.IsEmpty)   //普通单项where条件
                return GenerateConditionNode(whereCondition);

            if (subWhereStmts == null || subWhereStmts.Length<2)
                throw new MissingWhereConditionException();

            //父节点，非单项where条件
            var stmtNode = new WhereCondition(WhereConditionNodeType.Statement);

            var leftWhereStmt = subWhereStmts.First();
            var rightWhereStmt = subWhereStmts.Last();

            stmtNode.Statement_LeftNode = GenerateStatementNode(leftWhereStmt);
            stmtNode.Statement_Operator = andOr.GetText();
            stmtNode.Statement_RightNode = GenerateStatementNode(rightWhereStmt);

            return stmtNode;
        }

        private static WhereCondition GenerateConditionNode(SelectSQLParser.WhereConditionContext whereCondition)
        {
            var wcInfo = new WhereCondition(WhereConditionNodeType.Condition);

            wcInfo.Condition_LeftExpression = whereCondition.comparableValue().First().GetText();
            wcInfo.Condition_Operator = whereCondition.operators().GetText();
            wcInfo.Condition_RightExpression = whereCondition.comparableValue().Last().GetText();

            return wcInfo;
        }
    }
}
