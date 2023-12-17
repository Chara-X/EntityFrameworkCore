using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete;

public class TakeExpression : SuffixExpression
{
    public LambdaExpression Predicate { get; set; }

    public int Count { get; set; }

    public TakeExpression(Expression sub, LambdaExpression predicate, int count) : base(sub)
    {
        Predicate = predicate;
        Count = count;
    }
}