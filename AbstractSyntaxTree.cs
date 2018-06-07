using System.Collections.Generic;
using Funk.Expressions;

namespace Funk
{
    public class AbstractSyntaxTree
    {
        public List<IExpression> TopLevelExpressions { get; private set; }

        public AbstractSyntaxTree()
        {
            TopLevelExpressions = new List<IExpression>();
        }
    }
}
