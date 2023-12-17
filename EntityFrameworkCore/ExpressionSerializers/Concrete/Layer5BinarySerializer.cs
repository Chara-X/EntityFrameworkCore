using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Structure;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete;

public class Layer5BinarySerializer : IRightUnionExpressionSerializer
{
    public IExpressionSerializer Secondary { get; set; } = new Layer6BinarySerializer();

    public string Serialize(Expression exp) => Base(exp);

    private string Base(Expression exp) =>
        exp.NodeType switch
        {
            ExpressionType.Add => Add((BinaryExpression)exp),
            ExpressionType.Subtract => Subtract((BinaryExpression)exp),
            _ => Default(exp)
        };

    private string Add(BinaryExpression exp) =>
        Base(exp.Left) + " + " + Secondary.Serialize(exp.Right);

    private string Subtract(BinaryExpression exp) =>
        Base(exp.Left) + " - " + Secondary.Serialize(exp.Right);

    private string Default(Expression exp) => Secondary.Serialize(exp);
}