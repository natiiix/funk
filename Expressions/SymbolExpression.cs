namespace Funk.Expressions
{
    public class SymbolExpression : IExpression
    {
        public string Symbol { get; private set; }

        public SymbolExpression(string symbol)
        {
            Symbol = symbol;
        }

        public IExpression Evaluate() => throw new System.NotImplementedException();
    }
}
