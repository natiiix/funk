using System.Collections.Generic;
using Funk.Expressions;

namespace Funk
{
    public class InterpreterEnvironment
    {
        public readonly bool HasParent;
        public readonly InterpreterEnvironment Parent;
        public readonly Dictionary<string, IExpression> Symbols;

        public InterpreterEnvironment()
        {
            HasParent = false;
            Parent = null;
            Symbols = new Dictionary<string, IExpression>();
        }

        public InterpreterEnvironment(InterpreterEnvironment parent)
        {
            HasParent = true;
            Parent = parent;
            Symbols = new Dictionary<string, IExpression>();
        }

        public IExpression FindSymbol(string symbol)
        {
            if (Symbols.ContainsKey(symbol))
            {
                return Symbols[symbol];
            }
            else if (HasParent)
            {
                return Parent.FindSymbol(symbol);
            }
            else
            {
                throw new FatalException($"Undefined symbol: \"{symbol}\"");
            }
        }
    }
}
