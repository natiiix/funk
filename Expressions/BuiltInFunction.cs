using System.Collections.Generic;

namespace Funk.Expressions
{
    public class BuiltInFunction : Function
    {
        private FunctionCallHandler handler;

        public BuiltInFunction(FunctionCallHandler handler) => this.handler = handler;

        public override IExpression Evaluate(InterpreterEnvironment env) => this;

        public override IExpression Call(InterpreterEnvironment env, IEnumerable<IExpression> args) => handler(env, args);
    }
}
