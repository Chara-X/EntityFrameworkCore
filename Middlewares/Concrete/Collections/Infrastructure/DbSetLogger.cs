using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCore.Middlewares.Tools;
using EntityFrameworkCore.ORMapping;

namespace EntityFrameworkCore.Collections.Infrastructure
{
    public class DbSetLogger
    {
        private readonly Dictionary<string, EntityLog> logs = new Dictionary<string, EntityLog>();

        public DbProxy Proxy { get; set; }

        public DbSetLogger(DbProxy proxy) => Proxy = proxy;

        public void LogAdd(object entity)
        {
            var primaryKeyValue = PortableType.Create(entity.GetType()).GetPrimaryKeyValue(entity);
            if (logs.ContainsKey(primaryKeyValue))
                logs.Remove(primaryKeyValue);
            logs.Add(primaryKeyValue, new EntityLog(entity, EntityState.Added));
        }

        public void LogRemove(object entity)
        {
            var primaryKeyValue = PortableType.Create(entity.GetType()).GetPrimaryKeyValue(entity);
            if (logs.ContainsKey(primaryKeyValue))
                logs.Remove(primaryKeyValue);
            logs.Add(primaryKeyValue, new EntityLog(entity, EntityState.Removed));
        }

        public void LogUpdate(object entity)
        {
            var primaryKeyValue = PortableType.Create(entity.GetType()).GetPrimaryKeyValue(entity);
            if (logs.ContainsKey(primaryKeyValue))
            {
                var log = logs[primaryKeyValue];
                switch (log.State)
                {
                    case EntityState.Added:
                    case EntityState.Updated: log.Entity = entity; return;
                    case EntityState.Removed: return;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            logs.Add(primaryKeyValue, new EntityLog(entity, EntityState.Updated));
        }

        public int Submit()
        {
            return logs.Values.Sum(log => log.State switch
            {
                EntityState.Added => Proxy.Insert(log.Entity),
                EntityState.Updated => Proxy.Update(log.Entity),
                EntityState.Removed => Proxy.Delete(log.Entity),
                _ => throw new ArgumentOutOfRangeException()
            });
        }

        public class EntityLog
        {
            public object Entity { get; set; }

            public EntityState State { get; set; }

            public EntityLog(object entity, EntityState state)
            {
                Entity = entity;
                State = state;
            }
        }

        public enum EntityState
        {
            Added,
            Updated,
            Removed
        }
    }
}
