using System.Collections.Generic;

using TokenEnum = Funk.BetterEnumerator<Funk.Token>;

namespace Funk.Expressions
{
    public class CallExpression : IExpression
    {
        public IExpression Function { get; private set; }
        public List<IExpression> Arguments { get; private set; }

        public CallExpression(IExpression function, List<IExpression> arguments)
        {
            Function = function;
            Arguments = arguments;
        }

        public IExpression Evaluate(InterpreterEnvironment env)
        {
            Function evaluatedFunc = Function.Evaluate(env) as Function;

            if (evaluatedFunc == null)
            {
                throw new FatalException($"Unexpected function call syntax (\"{Function}\" does not evaluate to a function)");
            }

            return evaluatedFunc.Call(env, Arguments);
        }

        public static bool TryParse(List<Token> tokens, out CallExpression result)
        {
            // (<Function Expression> [Arguments])
            // ^^                                ^
            if (tokens.Count >= 3 &&
                tokens[0].Type == TokenType.OpenParenthesis &&
                tokens[tokens.Count - 1].Type == TokenType.CloseParenthesis)
            {
                List<Token> innerTokens = tokens.GetRange(1, tokens.Count - 2);
                TokenEnum tokenEnum = new TokenEnum(innerTokens);

                // Try to parse at least one valid expression from the inner tokens
                if (Parser.TryParseExpressions(innerTokens, out List<IExpression> innerExprs) && innerExprs.Count > 0)
                {
                    result = new CallExpression(innerExprs[0], innerExprs.GetRange(1, innerExprs.Count - 1));
                    return true;
                }
            }

            result = null;
            return false;
        }

        public override string ToString() => $"CallExpression({Function} ({string.Join(", ", Arguments)}))";
    }
}
