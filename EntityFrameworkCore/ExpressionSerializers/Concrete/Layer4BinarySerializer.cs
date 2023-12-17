using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Structure;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete;

public class Layer4BinarySerializer : IRightUnionExpressionSerializer
{
    public IExpressionSerializer Secondary { get; set; } = new Layer5BinarySerializer();

    public string Serialize(Expression exp) => Base(exp);

    private string Base(Expression exp) =>
        exp.NodeType switch
        {
            ExpressionType.Equal => Equal((BinaryExpression) exp),
            ExpressionType.NotEqual => NotEqual((BinaryExpression) exp),
            ExpressionType.GreaterThan => GreaterThan((BinaryExpression) exp),
            ExpressionType.LessThan => LessThan((BinaryExpression) exp),
            ExpressionType.GreaterThanOrEqual => GreaterThanOrEqual((BinaryExpression) exp),
            ExpressionType.LessThanOrEqual => LessThanOrEqual((BinaryExpression) exp),
            _ => Default(exp)
        };

    private string Equal(BinaryExpression exp) =>
        Base(exp.Left) + " = " + Secondary.Serialize(exp.Right);

    private string NotEqual(BinaryExpression exp) =>
        Base(exp.Left) + " <> " + Secondary.Serialize(exp.Right);

    private string GreaterThan(BinaryExpression exp) =>
        Base(exp.Left) + " > " + Secondary.Serialize(exp.Right);

    private string LessThan(BinaryExpression exp) =>
        Base(exp.Left) + " < " + Secondary.Serialize(exp.Right);

    private string GreaterThanOrEqual(BinaryExpression exp) =>
        Base(exp.Left) + " >= " + Secondary.Serialize(exp.Right);

    private string LessThanOrEqual(BinaryExpression exp) =>
        Base(exp.Left) + " <= " + Secondary.Serialize(exp.Right);

    private string Default(Expression exp) => Secondary.Serialize(exp);
}