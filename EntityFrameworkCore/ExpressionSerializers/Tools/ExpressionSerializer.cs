using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Concrete;

namespace EntityFrameworkCore.ExpressionSerializers.Tools;

public static class ExpressionSerializer
{
    private static readonly Layer0StatementSerializer Serializer = new();

    public static string Serialize(Expression exp) => Serializer.Serialize(exp);
}