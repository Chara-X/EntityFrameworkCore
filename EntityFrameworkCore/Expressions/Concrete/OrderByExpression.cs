using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete;

public class OrderByExpression : SuffixExpression
{
    public LambdaExpression KeySelector { get; set; }

    public OrderByExpression(Expression sub, LambdaExpression keySelector) : base(sub)
    {
        KeySelector = keySelector;
    }
}