using System.Linq;
using EntityFrameworkCore.ORMapping;

namespace EntityFrameworkCore.Middlewares.Concrete;

public class EncodeUpdateMiddleware : IMiddleware<object, int>
{
    public ExecuteNonQueryMiddleware Next { get; set; }

    public EncodeUpdateMiddleware(ExecuteNonQueryMiddleware next) => Next = next;

    public int Invoke(object request)
    {
        var type = PortableType.Create(request.GetType());
        var sql = string.Empty;
        sql += $"UPDATE [{type.Name}] SET ";
        sql = type.Properties.Where(i => !i.HasPrimaryKey && !i.HasForeignKey).Aggregate(sql, (current, i) => current + $"[{i.Name}] = {i.GetString(request)},");
        sql = $"{sql[0..^1]} WHERE [{type.PrimaryKey}] = '{type.GetPrimaryKeyValue(request)}'";
        return Next.Invoke(sql);
    }
}