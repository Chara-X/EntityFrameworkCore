using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EntityFrameworkCore.Collections;
using EntityFrameworkCore.Collections.Infrastructure;
using EntityFrameworkCore.Middlewares.Tools;

namespace EntityFrameworkCore;

public class DbContext:IDisposable
{
    private readonly List<DbSetLogger> _loggers = new();

    private readonly List<Type> _models = new();

    public DbProxy Proxy { get; set; }

    public DbContext(string connectionString)
    {
        Proxy = new DbProxy(connectionString);
        foreach (var property in GetType().GetProperties())
        {
            var type = property.PropertyType;
            if (type.Name != typeof(DbSet<>).Name) continue;
            _models.Add(type.GenericTypeArguments[0]);
            var logger = new DbSetLogger(Proxy);
            property.SetValue(this, Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] {logger}, null));
            _loggers.Add(logger);
        }
    }

    public void CreateModels() => Proxy.CreateModels(_models);

    public int SaveChanges() => _loggers.Sum(logger => logger.Submit());

    public void Dispose() => Proxy.Dispose();
}