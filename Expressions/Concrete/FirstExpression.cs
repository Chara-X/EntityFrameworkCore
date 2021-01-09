using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete
{
    public class FirstExpression : PrefixExpression
    {
        public LambdaExpression Predicate { get; set; }

        public FirstExpression(Expression sub, LambdaExpression predicate) : base(sub) => Predicate = predicate;
    }
}
