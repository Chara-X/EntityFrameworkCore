using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Structure;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete;

public class Layer6BinarySerializer : IRightUnionExpressionSerializer
{
    public IExpressionSerializer Secondary { get; set; } = new Layer7SuffixSerializer();

    public string Serialize(Expression exp) => Base(exp);

    private string Base(Expression exp) =>
        exp.NodeType switch
        {
            ExpressionType.Multiply => Multiply((BinaryExpression)exp),
            ExpressionType.Divide => Divide((BinaryExpression)exp),
            _ => Default(exp)
        };

    private string Multiply(BinaryExpression exp) =>
        Base(exp.Left) + "*" + Secondary.Serialize(exp.Right);

    private string Divide(BinaryExpression exp) =>
        Base(exp.Left) + "/" + Secondary.Serialize(exp.Right);

    private string Default(Expression exp) => Secondary.Serialize(exp);
}