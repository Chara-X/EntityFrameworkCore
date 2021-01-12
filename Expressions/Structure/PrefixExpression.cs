using System.Linq.Expressions;

namespace EntityFrameworkCore.Expressions.Structure
{
    public abstract class PrefixExpression : UnaryExpression
    {
        protected PrefixExpression(Expression sub) : base(sub) { }
    }
}
