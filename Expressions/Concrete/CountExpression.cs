using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete
{
    public class CountExpression : PrefixExpression
    {
        public CountExpression(Expression sub) : base(sub) { }
    }
}
