using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EntityFrameworkCore.Collections;
using EntityFrameworkCore.Collections.Infrastructure;
using EntityFrameworkCore.Middlewares.Tools;

namespace EntityFrameworkCore
{
    public class DbContext:IDisposable
    {
        private readonly List<DbSetLogger> loggers = new List<DbSetLogger>();

        private readonly List<Type> models = new List<Type>();

        public DbProxy Proxy { get; set; }

        public DbContext(string connectionString)
        {
            Proxy = new DbProxy(connectionString);
            foreach (var property in GetType().GetProperties())
            {
                var type = property.PropertyType;
                if (type.Name != typeof(DbSet<>).Name) continue;
                models.Add(type.GenericTypeArguments[0]);
                var logger = new DbSetLogger(Proxy);
                property.SetValue(this, Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] {logger}, null));
                loggers.Add(logger);
            }
        }

        public void CreateModels() => Proxy.CreateModels(models);

        public int SaveChanges() => loggers.Sum(logger => logger.Submit());

        public void Dispose() => Proxy.Dispose();
    }
}
