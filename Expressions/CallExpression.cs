using System.Collections.Generic;

namespace Funk.Expressions
{
    public class CallExpression : IExpression
    {
        public string FunctionName { get; private set; }
        public IEnumerable<IExpression> Arguments { get; private set; }

        public CallExpression(string functionName, IEnumerable<IExpression> arguments)
        {
            FunctionName = functionName;
            Arguments = arguments;
        }

        public IExpression Evaluate() => throw new System.NotImplementedException();
    }
}
