namespace Funk.Expressions
{
    public interface IExpression
    {
        IExpression Evaluate(InterpreterEnvironment env);
    }
}
