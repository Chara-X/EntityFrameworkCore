using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete
{
    public class MinExpression : PrefixExpression
    {
        public LambdaExpression Selector { get; set; }

        public MinExpression(Expression sub, LambdaExpression selector) : base(sub)
        {
            Selector = selector;
        }
    }
}
