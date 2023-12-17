using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Structure;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete;

public class Layer2BinarySerializer : IRightUnionExpressionSerializer
{
    public IExpressionSerializer Secondary { get; set; } = new Layer3BinarySerializer();

    public string Serialize(Expression exp) => Base(exp);

    private string Base(Expression exp) =>
        exp.NodeType switch
        {
            ExpressionType.OrElse => OrElse((BinaryExpression)exp),
            _ => Default(exp)
        };

    private string OrElse(BinaryExpression exp) =>
        Base(exp.Left) + " OR " + Secondary.Serialize(exp.Right);

    private string Default(Expression exp) => Secondary.Serialize(exp);
}