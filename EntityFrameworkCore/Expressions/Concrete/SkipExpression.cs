using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete;

public class SkipExpression:SuffixExpression
{
    public LambdaExpression KeySelector { get; set; }

    public int Count { get; set; }

    public SkipExpression(Expression sub, LambdaExpression keySelector, int count) : base(sub)
    {
        KeySelector = keySelector;
        Count = count;
    }
}