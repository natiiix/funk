using System.Collections.Generic;
using Funk.Expressions;

using TokenEnum = Funk.BetterEnumerator<Funk.Token>;

namespace Funk
{
    public static class Parser
    {
        public static AbstractSyntaxTree ParseAST(IEnumerable<Token> tokens)
        {
            try
            {
                // Parse an abstract syntax tree
                return new AbstractSyntaxTree(ParseExpressions(tokens));
            }
            catch (UnexpectedTokenException e)
            {
                Program.ExitWithError(e.Message);
                return null;
            }
        }

        public static List<IExpression> ParseExpressions(IEnumerable<Token> tokens)
        {
            List<IExpression> exprs = new List<IExpression>();
            TokenEnum tokenEnum = new TokenEnum(tokens);

            do
            {
                exprs.Add(ParseNextExpression(tokenEnum));
            }
            while (tokenEnum.MoveNext());

            return exprs;
        }

        public static IExpression ParseNextExpression(TokenEnum tokenEnum)
        {
            List<Token> tokenBuffer = new List<Token>();

            do
            {
                tokenBuffer.Add(tokenEnum.Current);

                if (TryParseExpression(tokenBuffer, out IExpression expr))
                {
                    return expr;
                }
            }
            while (tokenEnum.MoveNext());

            // Unrecognized expression syntax
            // Exit with an error
            Program.ExitWithError("Unexpected end of source code");
            return null;
        }

        public static bool TryParseExpression(List<Token> tokens, out IExpression result)
        {
            // Number
            if (NumberExpression.TryParse(tokens, out NumberExpression numExpr))
            {
                result = numExpr;
                return true;
            }
            // Symbol
            else if (SymbolExpression.TryParse(tokens, out SymbolExpression symExpr))
            {
                result = symExpr;
                return true;
            }
            // Function call
            else if (CallExpression.TryParse(tokens, out CallExpression callExpr))
            {
                result = callExpr;
                return true;
            }
            // Function definition
            else if (FunctionExpression.TryParse(tokens, out FunctionExpression funcExpr))
            {
                result = funcExpr;
                return true;
            }

            result = null;
            return false;
        }
    }
}
