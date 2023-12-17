using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete;

public class MaxExpression : SuffixExpression
{
    public LambdaExpression Selector { get; set; }

    public MaxExpression(Expression sub, LambdaExpression selector) : base(sub)
    {
        Selector = selector;
    }
}