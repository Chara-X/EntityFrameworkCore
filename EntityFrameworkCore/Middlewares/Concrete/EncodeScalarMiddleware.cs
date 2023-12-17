using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Tools;

namespace EntityFrameworkCore.Middlewares.Concrete;

public class EncodeScalarMiddleware : IMiddleware<Expression, object>
{
    public ExecuteScalarMiddleware Next { get; set; }

    public EncodeScalarMiddleware(ExecuteScalarMiddleware next) => Next = next;

    public object Invoke(Expression request) => Next.Invoke(ExpressionSerializer.Serialize(request));
}