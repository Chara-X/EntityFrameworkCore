using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EntityFrameworkCore.Middlewares.Concrete;

public class ExecuteReaderMiddleware : IMiddleware<string, List<Dictionary<string, object>>>
{
    public SqlConnection Connection { get; set; }

    public ExecuteReaderMiddleware(SqlConnection connection) => Connection = connection;

    public List<Dictionary<string, object>> Invoke(string request)
    {
        Console.WriteLine(request);
        var rows = new List<Dictionary<string, object>>();
        var reader = new SqlCommand(request, Connection).ExecuteReader();
        while (reader.Read())
        {
            var row = new Dictionary<string, object>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                var value = reader.GetValue(i);
                row.Add(reader.GetName(i), value is DBNull ? null : value);
            }

            rows.Add(row);
        }

        return rows;
    }
}