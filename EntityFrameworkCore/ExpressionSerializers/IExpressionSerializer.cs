using System.Linq.Expressions;

namespace EntityFrameworkCore.ExpressionSerializers;

public interface IExpressionSerializer
{
    string Serialize(Expression exp);
}