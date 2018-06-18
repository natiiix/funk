using System.Collections.Generic;
using System.Linq;

namespace Funk.Expressions
{
    public class BuiltInFunction : Function
    {
        private FunctionCallHandler handler;
        private bool evaluateArgs;

        public BuiltInFunction(FunctionCallHandler handler, bool evaluateArgs = true)
        {
            this.handler = handler;
            this.evaluateArgs = evaluateArgs;
        }

        public override IExpression Call(InterpreterEnvironment env, IEnumerable<IExpression> args) =>
            handler(env, evaluateArgs ? args.Select(x => x.Evaluate(env)) : args);
    }
}
