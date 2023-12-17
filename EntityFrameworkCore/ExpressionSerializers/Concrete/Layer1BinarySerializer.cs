using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Structure;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete;

public class Layer1BinarySerializer : IRightUnionExpressionSerializer
{
    public IExpressionSerializer Secondary { get; set; } = new Layer2BinarySerializer();

    public string Serialize(Expression exp) => Base(exp);

    private string Base(Expression exp) => Secondary.Serialize(exp);
}