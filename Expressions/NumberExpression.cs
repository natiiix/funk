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
    }
}
