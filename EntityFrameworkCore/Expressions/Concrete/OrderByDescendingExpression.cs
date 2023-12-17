using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete;

public class OrderByDescendingExpression : SuffixExpression
{
    public LambdaExpression KeySelector { get; set; }

    public OrderByDescendingExpression(Expression sub, LambdaExpression keySelector) : base(sub) => KeySelector = keySelector;
}