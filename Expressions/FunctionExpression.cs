using System.Collections.Generic;
using System.Linq;

namespace Funk.Expressions
{
    public class FunctionExpression : Function
    {
        public string Name { get; private set; }
        public IEnumerable<string> Arguments { get; private set; }
        public IExpression Body { get; private set; }

        public FunctionExpression(string name, IEnumerable<string> arguments, IExpression body)
        {
            Name = name;
            Arguments = arguments;
            Body = body;
        }

        public static bool TryParse(List<Token> tokens, out FunctionExpression result)
        {
            // (function <FunctionName> ([Arguments]) <Expression>)
            // ^^        ^              ^           ^ ^           ^
            if (tokens.Count >= 7 &&
                tokens[0].Type == TokenType.OpenParenthesis && // "("
                tokens[1].Type == TokenType.Symbol && tokens[1].Value == Keywords.Function && // "function" keyword
                tokens[2].Type == TokenType.Symbol && // function name
                tokens[3].Type == TokenType.OpenParenthesis && // "("
                tokens[tokens.Count - 1].Type == TokenType.CloseParenthesis) // ")"
            {
                // List of argument names
                List<string> args = new List<string>();

                // Parse argument names
                int i = 4;
                while (i < tokens.Count - 1)
                {
                    Token t = tokens[i++];

                    // ")"
                    if (t.Type == TokenType.CloseParenthesis)
                    {
                        break;
                    }
                    // Symbol
                    else if (t.Type == TokenType.Symbol)
                    {
                        args.Add(t.Value);
                    }
                    // Unexpected token
                    else
                    {
                        result = null;
                        return false;
                    }
                }

                // Parse body expression
                if (Parser.TryParseExpression(tokens.GetRange(i, tokens.Count - i - 1), out IExpression expr))
                {
                    result = new FunctionExpression(tokens[2].Value, args, expr);
                    return true;
                }
            }

            result = null;
            return false;
        }

        public override string ToString() => $"FunctionExpression({Name} ({string.Join(", ", Arguments)}) {Body})";

        public override IExpression Call(InterpreterEnvironment env, IEnumerable<IExpression> args)
        {
            if (args.Count() != Arguments.Count())
            {
                throw new UnexpectedNumberOfArgumentsException(Name, Arguments.Count(), args.Count());
            }

            if (Body is CallExpression)
            {
                InterpreterEnvironment innerEnv = new InterpreterEnvironment(env);

                int argCount = args.Count();
                for (int i = 0; i < argCount; i++)
                {
                    innerEnv.Symbols[Arguments.ElementAt(i)] = args.ElementAt(i);
                }

                return Body.Evaluate(innerEnv);
            }
            else
            {
                return Body.Evaluate(env);
            }
        }

        public override IExpression Evaluate(InterpreterEnvironment env)
        {
            // Register the function as a symbol in the currect environment
            env.Symbols[Name] = this;

            // Perform the default behavior
            return base.Evaluate(env);
        }
    }
}
