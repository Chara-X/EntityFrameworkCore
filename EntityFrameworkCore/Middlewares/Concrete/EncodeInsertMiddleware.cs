using System.Linq;
using EntityFrameworkCore.ORMapping;

namespace EntityFrameworkCore.Middlewares.Concrete;

public class EncodeInsertMiddleware : IMiddleware<object, int>
{
    public ExecuteNonQueryMiddleware Next { get; set; }

    public EncodeInsertMiddleware(ExecuteNonQueryMiddleware next) => Next = next;

    public int Invoke(object request)
    {
        var type = PortableType.Create(request.GetType());
        var sql = string.Empty;
        sql += $"INSERT INTO [{type.Name}] VALUES (";
        sql = type.Properties.Where(i => !i.HasForeignKey && !i.IsIdentity).Aggregate(sql, (current, i) => current + (i.GetString(request) + ','));
        sql = sql[0..^1] + ")";
        return Next.Invoke(sql);
    }
}