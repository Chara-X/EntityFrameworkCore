using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete
{
    public class DistinctExpression : PrefixExpression
    {
        public DistinctExpression(Expression sub) : base(sub) { }
    }
}
