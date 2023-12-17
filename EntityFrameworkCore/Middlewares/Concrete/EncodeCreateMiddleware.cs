using System;
using EntityFrameworkCore.ORMapping;

namespace EntityFrameworkCore.Middlewares.Concrete;

public class EncodeCreateMiddleware : IMiddleware<Type, int>
{
    public ExecuteNonQueryMiddleware Next { get; set; }

    public EncodeCreateMiddleware(ExecuteNonQueryMiddleware next) => Next = next;

    public int Invoke(Type request)
    {
        var type = PortableType.Create(request);
        var sql = string.Empty;
        sql += $"CREATE TABLE [{type.Name}] \n(";
        foreach (var property in type.Properties)
        {
            if (!property.HasForeignKey)
            {
                sql += $"\n  [{property.Name}] {property.ColumnType}";
                if (property.HasPrimaryKey)
                    sql += " PRIMARY KEY";
                if (property.IsIdentity)
                    sql += " IDENTITY(1,1)";
                if (property.IsRequired)
                    sql += " NOT NULL";
                sql += ',';
            }
            else
                sql += $"FOREIGN KEY({property.ForeignKey}) REFERENCES [{property.Type.Name}]({property.Type.PrimaryKey}),";
        }

        sql = sql[0..^1] + "\n)\n";
        return Next.Invoke(sql);
    }
}