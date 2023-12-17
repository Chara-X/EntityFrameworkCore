using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Structure;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete;

public class Layer3BinarySerializer : IRightUnionExpressionSerializer
{
    public IExpressionSerializer Secondary { get; set; } = new Layer4BinarySerializer();

    public string Serialize(Expression exp) => Base(exp);

    private string Base(Expression exp) =>
        exp.NodeType switch
        {
            ExpressionType.AndAlso => AndAlso((BinaryExpression)exp),
            _ => Default(exp)
        };

    private string AndAlso(BinaryExpression exp) =>
        Base(exp.Left) + " AND " + Secondary.Serialize(exp.Right);

    private string Default(Expression exp) => Secondary.Serialize(exp);
}