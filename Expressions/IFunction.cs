using System.Collections.Generic;

namespace Funk.Expressions
{
    public interface IFunction
    {
        IExpression Call(InterpreterEnvironment env, IEnumerable<IExpression> args);
    }
}
