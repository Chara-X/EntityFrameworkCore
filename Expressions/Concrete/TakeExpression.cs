using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete
{
    public class TakeExpression : PrefixExpression
    {
        public int Count { get; set; }

        public TakeExpression(Expression sub, int count) : base(sub)
        {
            Count = count;
        }
    }
}
