namespace Funk.Expressions
{
    public class VoidExpression : IExpression
    {
        public IExpression Evaluate(InterpreterEnvironment env) =>
            throw new FatalException("EmptyExpression type cannot be evaluated");
    }
}
