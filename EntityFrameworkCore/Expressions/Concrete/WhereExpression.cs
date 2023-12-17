using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Structure;

namespace EntityFrameworkCore.Expressions.Concrete;

public class WhereExpression : SuffixExpression
{
    public LambdaExpression Predicate { get; set; }

    public WhereExpression(Expression sub, LambdaExpression predicate) : base(sub) => Predicate = predicate;
}