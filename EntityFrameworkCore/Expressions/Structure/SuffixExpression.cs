using System.Linq.Expressions;

namespace EntityFrameworkCore.Expressions.Structure;

public abstract class SuffixExpression : UnaryExpression
{
    protected SuffixExpression(Expression sub) : base(sub) { }
}