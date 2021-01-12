using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using EntityFrameworkCore.Middlewares.Concrete;

namespace EntityFrameworkCore.Middlewares.Tools
{
    public class DbProxy:IDisposable
    {
        private readonly SqlConnection connection;

        private readonly ExecuteReaderMiddleware executeReader;

        private readonly EncodeCreateMiddleware create;
        private readonly EncodeInsertMiddleware insert;
        private readonly EncodeUpdateMiddleware update;
        private readonly EncodeDeleteMiddleware delete;
        private readonly EncodeScalarMiddleware scalar;

        public DbProxy(string connectionString)
        {
            connection = new SqlConnection(new SqlConnectionStringBuilder(connectionString) { MultipleActiveResultSets = true }.ToString());
            connection.Open();
            var executeNonQuery = new ExecuteNonQueryMiddleware(connection);
            var executeScalar = new ExecuteScalarMiddleware(connection);
            executeReader = new ExecuteReaderMiddleware(connection);
            create = new EncodeCreateMiddleware(executeNonQuery);
            insert = new EncodeInsertMiddleware(executeNonQuery);
            update = new EncodeUpdateMiddleware(executeNonQuery);
            delete = new EncodeDeleteMiddleware(executeNonQuery);
            scalar = new EncodeScalarMiddleware(executeScalar);
        }

        public void Create(params Type[] types)
        {
            foreach (var type in types)
                create.Invoke(type);
        }

        public int Insert(object entity) => insert.Invoke(entity);

        public int Delete(object entity) => delete.Invoke(entity);

        public int Update(object entity) => update.Invoke(entity);

        public object Scalar(Expression exp) => scalar.Invoke(exp);

        public IEnumerable<T> Reader<T>(Expression exp) => new EncodeReaderMiddleware<T>(executeReader).Invoke(exp);

        public void Dispose() => connection.Close();
    }
}
