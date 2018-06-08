using System.Collections.Generic;

namespace Funk.Expressions
{
    public class NumberExpression : IExpression
    {
        public int Value { get; private set; }

        public NumberExpression(int value)
        {
            Value = value;
        }

        public IExpression Evaluate() => this;

        public static bool TryParse(List<Token> tokens, out NumberExpression result)
        {
            if (tokens.Count == 1 &&
                tokens[0].Type == TokenType.Number &&
                int.TryParse(tokens[0].Value, out int value))
            {
                result = new NumberExpression(value);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override string ToString() => $"NumberExpression({Value})";
    }
}
