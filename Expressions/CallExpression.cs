using System.Collections.Generic;

namespace Funk.Expressions
{
    public class CallExpression : IExpression
    {
        public string FunctionName { get; private set; }
        public List<IExpression> Arguments { get; private set; }

        public CallExpression(string functionName, List<IExpression> arguments)
        {
            FunctionName = functionName;
            Arguments = arguments;
        }

        public IExpression Evaluate() => throw new System.NotImplementedException();

        public static bool TryParse(List<Token> tokens, out CallExpression result)
        {
            if (tokens.Count >= 3 &&
                tokens[0].Type == TokenType.OpenParenthesis &&
                tokens[1].Type == TokenType.Symbol &&
                tokens[tokens.Count - 1].Type == TokenType.CloseParenthesis)
            {
                // List of expressions that are passed to the function as arguments
                List<IExpression> args = Parser.ParseExpressions(tokens.GetRange(2, tokens.Count - 3));

                // // List of indices of tokens beginning nested calls
                // List<int> nestedCalls = new List<int>();

                // // Iterate through the tokens inside of the expression
                // // to check if all nested calls are properly closed off
                // // Top-level argument expressions are parsed in the process
                // for (int i = 2; i < tokens.Count - 1; i++)
                // {
                //     if (tokens[i].Type == TokenType.OpenParenthesis)
                //     {
                //         nestedCalls.Add(i);
                //     }
                //     else if (tokens[i].Type == TokenType.CloseParenthesis)
                //     {
                //         // Unexpected token ")"
                //         // There are too many closing parentheses
                //         if (nestedCalls.Count == 0)
                //         {
                //             result = null;
                //             return false;
                //         }
                //         // End of a nested call
                //         else if (nestedCalls.Count == 1)
                //         {
                //             int start = nestedCalls[0];
                //             CallExpression nestedCallExpr = CallExpression.TryParse(tokens.GetRange(start, i - start + 1));
                //         }

                //         nestedCallTokens.RemoveLast();
                //     }
                // }

                // // At least one nested call is not properly closed off
                // // There are not enough closing parentheses
                // // The depth value cannot be negative
                // if (nestedCalls.Count > 0)
                // {
                //     result = null;
                //     return false;
                // }

                result = new CallExpression(tokens[1].Value, args);
                return true;
            }

            result = null;
            return false;
        }
    }
}
