using System.Linq.Expressions;

namespace EntityFrameworkCore.Expressions.Structure
{
    public class PrefixExpression : UnaryExpression
    {
        public PrefixExpression(Expression sub) : base(sub) { }
    }
}
