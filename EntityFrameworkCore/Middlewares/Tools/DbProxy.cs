using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using EntityFrameworkCore.Middlewares.Concrete;

namespace EntityFrameworkCore.Middlewares.Tools;

public class DbProxy : IDisposable
{
    private readonly SqlConnection _connection;

    private readonly ExecuteReaderMiddleware _executeReader;

    private readonly EncodeCreateMiddleware _create;
    private readonly EncodeInsertMiddleware _insert;
    private readonly EncodeUpdateMiddleware _update;
    private readonly EncodeDeleteMiddleware _delete;
    private readonly EncodeScalarMiddleware _scalar;

    public DbProxy(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString) { MultipleActiveResultSets = true };
        _connection = new SqlConnection(builder.ToString());
        _connection.Open();
        var executeNonQuery = new ExecuteNonQueryMiddleware(_connection);
        var executeScalar = new ExecuteScalarMiddleware(_connection);
        _executeReader = new ExecuteReaderMiddleware(_connection);
        _create = new EncodeCreateMiddleware(executeNonQuery);
        _insert = new EncodeInsertMiddleware(executeNonQuery);
        _update = new EncodeUpdateMiddleware(executeNonQuery);
        _delete = new EncodeDeleteMiddleware(executeNonQuery);
        _scalar = new EncodeScalarMiddleware(executeScalar);
    }

    public void CreateModels(IEnumerable<Type> types)
    {
        foreach (var type in types)
            _create.Invoke(type);
    }

    public int Insert(object entity) => _insert.Invoke(entity);
    public int Delete(object entity) => _delete.Invoke(entity);
    public int Update(object entity) => _update.Invoke(entity);
    public object Scalar(Expression exp) => _scalar.Invoke(exp);
    public IEnumerable<T> Reader<T>(Expression exp) => new EncodeReaderMiddleware<T>(_executeReader).Invoke(exp);
    public void Dispose() => _connection.Close();
}