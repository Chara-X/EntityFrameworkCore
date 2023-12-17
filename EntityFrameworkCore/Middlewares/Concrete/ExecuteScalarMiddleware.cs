using System;
using System.Data.SqlClient;

namespace EntityFrameworkCore.Middlewares.Concrete;

public class ExecuteScalarMiddleware : IMiddleware<string, object>
{
    public SqlConnection Connection { get; set; }

    public ExecuteScalarMiddleware(SqlConnection connection) => Connection = connection;

    public object Invoke(string request)
    {
        Console.WriteLine(request);
        return new SqlCommand(request, Connection).ExecuteScalar();
    }
}