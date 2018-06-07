using System.Collections.Generic;

namespace Funk.Expressions
{
    public class FunctionExpression : IExpression
    {
        public string FunctionName { get; private set; }
        public IEnumerable<string> Arguments { get; private set; }
        public IExpression Body { get; private set; }

        public FunctionExpression(string functionName, IEnumerable<string> arguments, IExpression body)
        {
            FunctionName = functionName;
            Arguments = arguments;
            Body = body;
        }

        public IExpression Evaluate() => throw new System.NotImplementedException();
    }
}
