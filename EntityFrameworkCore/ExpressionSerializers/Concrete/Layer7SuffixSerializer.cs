using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Structure;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete;

public class Layer7SuffixSerializer : IRightUnionExpressionSerializer
{
    public IExpressionSerializer Secondary { get; set; } = new Layer8PrefixSerializer();

    public string Serialize(Expression exp) => Base(exp);

    private string Base(Expression exp) =>
        exp.NodeType switch
        {
            ExpressionType.MemberAccess => MemberAccess((MemberExpression)exp),
            _ => Default(exp)
        };

    private string MemberAccess(MemberExpression exp) =>
        exp.Expression!.NodeType switch
        {
            ExpressionType.Parameter => Base(exp.Expression) + "." + exp.Member.Name,
            ExpressionType.MemberAccess=> Base(exp.Expression) + "_" + exp.Member.Name,
            _ => Default(exp)
        };

    private string Default(Expression exp) => Secondary.Serialize(exp);
}