using System.Collections.Generic;
using System.Linq;
using Funk.Expressions;

using TokenEnum = Funk.BetterEnumerator<Funk.Token>;

namespace Funk
{
    public static class Parser
    {
        public static AbstractSyntaxTree ParseAST(IEnumerable<Token> tokens)
        {
            // Parse an abstract syntax tree
            if (TryParseExpressions(tokens, out List<IExpression> exprs))
            {
                return new AbstractSyntaxTree(exprs);
            }
            // Unable to parse the AST due to invalid syntax
            else
            {
                Program.ExitWithError("Invalid syntax");
                return null;
            }
        }

        public static bool TryParseExpressions(IEnumerable<Token> tokens, out List<IExpression> result)
        {
            // Empty token list
            // Return an empty expression list
            if (tokens.Count() == 0)
            {
                result = new List<IExpression>();
                return true;
            }

            List<IExpression> exprs = new List<IExpression>();
            TokenEnum tokenEnum = new TokenEnum(tokens);

            // Parse top-level expressions from tokens one by one
            do
            {
                // Expression parsed successfully
                if (TryParseNextExpression(tokenEnum, out IExpression expr))
                {
                    exprs.Add(expr);
                }
                // Unrecognized expression syntax
                else
                {
                    result = null;
                    return false;
                }
            }
            while (tokenEnum.MoveNext());

            result = exprs;
            return true;
        }

        public static bool TryParseNextExpression(TokenEnum tokenEnum, out IExpression result)
        {
            List<Token> tokenBuffer = new List<Token>();

            // Gradually add tokens to the token buffer
            // until they match an expression pattern
            // or until the parser runs out of tokens
            do
            {
                tokenBuffer.Add(tokenEnum.Current);

                // Try to parse an expression from the tokens in the token buffer
                if (TryParseExpression(tokenBuffer, out IExpression expr))
                {
                    result = expr;
                    return true;
                }
            }
            while (tokenEnum.MoveNext());

            // Unrecognized expression syntax
            result = null;
            return false;
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
