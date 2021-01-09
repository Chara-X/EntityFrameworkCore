using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete
{
    public class SumExpression : PrefixExpression
    {
        public LambdaExpression Selector { get; set; }

        public SumExpression(Expression sub, LambdaExpression selector) : base(sub)
        {
            Selector = selector;
        }
    }
}
