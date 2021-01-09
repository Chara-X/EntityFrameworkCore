using System.Linq.Expressions;

namespace EntityFrameworkCore.Expressions.Structure
{
    public class UnaryExpression : Expression
    {
        public Expression Sub { get; set; }

        public UnaryExpression(Expression sub) => Sub = sub;
    }
}
