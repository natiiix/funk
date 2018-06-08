using System.Collections.Generic;
using Funk.Expressions;

namespace Funk
{
    public class AbstractSyntaxTree
    {
        public List<IExpression> TopLevelExpressions { get; private set; }

        public AbstractSyntaxTree(List<IExpression> topLevelExprs)
        {
            TopLevelExpressions = topLevelExprs;
        }
    }
}
