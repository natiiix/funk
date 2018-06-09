using System;
using Funk.Expressions;

namespace Funk
{
    public class Interpreter
    {
        private readonly AbstractSyntaxTree ast;
        private readonly InterpreterEnvironment rootEnv;

        public Interpreter(AbstractSyntaxTree ast)
        {
            this.ast = ast;
            rootEnv = new InterpreterEnvironment();
        }

        public void Run()
        {
            foreach (IExpression expr in ast.TopLevelExpressions)
            {
                IExpression value = expr.Evaluate(rootEnv);
                Console.WriteLine(value);
            }
        }
    }
}
