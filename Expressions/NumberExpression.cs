using System.Collections.Generic;

namespace Funk.Expressions
{
    public class NumberExpression : IExpression
    {
        public int Value { get; private set; }

        public bool BooleanValue { get => Value != 0; private set => Value = value ? 1 : 0; }

        public NumberExpression(int value) => Value = value;

        public NumberExpression(bool booleanValue) => BooleanValue = booleanValue;

        public IExpression Evaluate(InterpreterEnvironment env) => this;

        public static bool TryParse(List<Token> tokens, out NumberExpression result)
        {
            // <Value>
            // ^
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
