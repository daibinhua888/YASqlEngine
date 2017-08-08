using Antlr4.Runtime;
using YASqlEngine.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace YASqlEngine.Core
{
    class ErrorListener : IAntlrErrorListener<IToken>
    {
        //public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        //{
        //    throw e;
        //}

        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            if (msg == "mismatched input 'FROM' expecting {A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, '*', NUMBER, FUNCTIONS_PREFIX, COLUMN_PREDICT}")
                throw new MissingColumnExpressionException();
            
            if(msg== "missing {A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z} at '<EOF>'")
                throw new MissingTableNameException();

            throw e;
        }
    }
}
