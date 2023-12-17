using EntityFrameworkCore.ORMapping;

namespace EntityFrameworkCore.Middlewares.Concrete;

public class EncodeDeleteMiddleware : IMiddleware<object, int>
{
    public ExecuteNonQueryMiddleware Next { get; set; }

    public EncodeDeleteMiddleware(ExecuteNonQueryMiddleware next) => Next = next;

    public int Invoke(object request)
    {
        var type = PortableType.Create(request.GetType());
        var sql = $"DELETE FROM [{type.Name}] WHERE [{type.PrimaryKey}] = '{type.GetPrimaryKeyValue(request)}'";
        return Next.Invoke(sql);
    }
}