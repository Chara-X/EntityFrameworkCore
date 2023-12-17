using System.Linq.Expressions;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete;

public class Layer9FactorSerializer : IExpressionSerializer
{
    public string Serialize(Expression exp) => Base(exp);

    private static string Base(Expression exp) =>
        exp.NodeType switch
        {
            ExpressionType.MemberAccess => Value(exp),
            ExpressionType.Constant => Value(exp),
            ExpressionType.Call => Value(exp),
            ExpressionType.Parameter => "this",
            _ => null
        };

    private static string Value(Expression exp) => $"'{Expression.Lambda(exp).Compile().DynamicInvoke()}'";
}