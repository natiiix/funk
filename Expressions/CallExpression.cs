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

        public IExpression Evaluate(InterpreterEnvironment env) => throw new System.NotImplementedException();

        public static bool TryParse(List<Token> tokens, out CallExpression result)
        {
            // (FunctionName [Arguments])
            // ^^                       ^
            if (tokens.Count >= 3 &&
                tokens[0].Type == TokenType.OpenParenthesis &&
                tokens[1].Type == TokenType.Symbol &&
                tokens[tokens.Count - 1].Type == TokenType.CloseParenthesis)
            {
                // List of tokens to be converted into argument expressions
                List<Token> argsTokens = tokens.GetRange(2, tokens.Count - 3);

                // Try to parse a list of expressions to be passed
                // to the function as arguments from the tokens
                if (Parser.TryParseExpressions(argsTokens, out List<IExpression> args))
                {
                    result = new CallExpression(tokens[1].Value, args);
                    return true;
                }
            }

            result = null;
            return false;
        }

        public override string ToString() => $"CallExpression({FunctionName} ({string.Join(", ", Arguments)}))";
    }
}
