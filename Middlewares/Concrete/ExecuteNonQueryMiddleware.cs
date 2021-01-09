using System;
using System.Data.SqlClient;

namespace EntityFrameworkCore.Middlewares.Concrete
{
    public class ExecuteNonQueryMiddleware : IMiddleware<string, int>
    {
        public SqlConnection Connection { get; set; }

        public ExecuteNonQueryMiddleware(SqlConnection connection) => Connection = connection;

        public int Invoke(string request)
        {
            Console.WriteLine(request);
            Connection.Open();
            var count = new SqlCommand(request, Connection).ExecuteNonQuery();
            Connection.Close();
            return count;
        }
    }
}
