using System.Linq.Expressions;

namespace EntityFrameworkCore.Expressions.Structure
{
    public abstract class UnaryExpression : CachedExpression
    {
        public Expression Sub { get; set; }

        protected UnaryExpression(Expression sub) => Sub = sub;
    }
}
