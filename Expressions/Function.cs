using System.Collections.Generic;

namespace Funk.Expressions
{
    public delegate IExpression FunctionCallHandler(InterpreterEnvironment env, IEnumerable<IExpression> args);

    public abstract class Function : IExpression
    {
        public abstract IExpression Call(InterpreterEnvironment env, IEnumerable<IExpression> args);

        public virtual IExpression Evaluate(InterpreterEnvironment env) => this;
    }
}
