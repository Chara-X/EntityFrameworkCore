using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete
{
    public class SelectExpression : PrefixExpression
    {
        public LambdaExpression Selector { get; set; }

        public SelectExpression(Expression sub, LambdaExpression selector) : base(sub) => Selector = selector;
    }
}
