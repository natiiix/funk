using System.Collections.Generic;

namespace Funk.Expressions
{
    public class SymbolExpression : IExpression
    {
        public string Symbol { get; private set; }

        public SymbolExpression(string symbol)
        {
            Symbol = symbol;
        }

        public IExpression Evaluate(InterpreterEnvironment env) => env.FindSymbol(Symbol);

        public static bool TryParse(List<Token> tokens, out SymbolExpression result)
        {
            // <Symbol>
            // ^
            if (tokens.Count == 1 &&
                tokens[0].Type == TokenType.Symbol)
            {
                result = new SymbolExpression(tokens[0].Value);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override string ToString() => $"SymbolExpression({Symbol})";
    }
}
