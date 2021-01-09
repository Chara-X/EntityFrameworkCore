using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EntityFrameworkCore.Collections;
using EntityFrameworkCore.Collections.Infrastructure;
using EntityFrameworkCore.Middlewares.Tools;

namespace EntityFrameworkCore
{
    public class DbContext
    {
        public DbProxy Proxy { get; set; }

        private readonly List<DbSetLogger> loggers = new List<DbSetLogger>();

        public DbContext(string connectionString)
        {
            Proxy = new DbProxy(connectionString);
            foreach (var property in GetType().GetProperties())
            {
                var type = property.PropertyType;
                if (type.Name != typeof(DbSet<>).Name) continue;
                var logger = new DbSetLogger(Proxy);
                property.SetValue(this, Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] {logger}, null));
                loggers.Add(logger);
            }
        }

        public int SaveChanged() => loggers.Sum(logger => logger.Submit());
    }
}
