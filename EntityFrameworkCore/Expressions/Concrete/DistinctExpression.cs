using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete;

public class DistinctExpression : SuffixExpression
{
    public DistinctExpression(Expression sub) : base(sub) { }
}