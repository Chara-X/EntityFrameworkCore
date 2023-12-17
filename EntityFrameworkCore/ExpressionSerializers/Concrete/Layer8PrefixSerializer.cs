using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Structure;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete;

public class Layer8PrefixSerializer:IRightUnionExpressionSerializer
{
    public IExpressionSerializer Secondary { get; set; } = new Layer9FactorSerializer();

    public string Serialize(Expression exp) => Base(exp);

    private string Base(Expression exp) =>
        exp.NodeType switch
        {
            ExpressionType.New => New(),
            _ => Secondary.Serialize(exp)
        };

    private static string New() => "this";
}