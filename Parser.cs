using System.Collections.Generic;
using Funk.Expressions;

using TokenEnum = Funk.BetterEnumerator<Funk.Token>;

namespace Funk
{
    public static class Parser
    {
        public static AbstractSyntaxTree ParseAST(IEnumerable<Token> tokens)
        {
            AbstractSyntaxTree ast = new AbstractSyntaxTree();
            TokenEnum tokenEnum = new TokenEnum(tokens);

            // Parse all the top level expressions
            do
            {
                // Top level expressions must always begin with an open parenthesis
                if (tokenEnum.Current.Type == TokenType.OpenParenthesis)
                {
                    ast.TopLevelExpressions.Add(ParseExpression(tokenEnum));
                }
                else
                {
                    ExitUnexpectedToken(tokenEnum.Current);
                }
            }
            while (tokenEnum.MoveNext());

            return ast;
        }

        private static void ExitUnexpectedToken(Token token) => Program.ExitWithError($"Unexpected token: \"{token}\"");

        private static IExpression ParseExpression(TokenEnum tokenEnum)
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

        private static bool TryParseExpression(List<Token> tokens, out IExpression result)
        {
            // Number
            if (NumberExpression.TryParse(tokens, out NumberExpression expr))
            {
                result = expr;
                return true;
            }
            // TODO: Other expression types

            result = null;
            return false;
        }
    }
}
