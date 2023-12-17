using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using EntityFrameworkCore.ExpressionSerializers.Tools;
using EntityFrameworkCore.Tools;

namespace EntityFrameworkCore.Middlewares.Concrete;

public class EncodeReaderMiddleware<T> : IMiddleware<Expression, IEnumerable<T>>
{
    public ExecuteReaderMiddleware Next { get; set; }

    public EncodeReaderMiddleware(ExecuteReaderMiddleware next) => Next = next;

    public IEnumerable<T> Invoke(Expression request) => Next.Invoke(ExpressionSerializer.Serialize(request)).Select(ActivatorOfDictionary.CreateInstance<T>).ToList();
}