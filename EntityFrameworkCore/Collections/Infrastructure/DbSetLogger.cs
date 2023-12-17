using System;
using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCore.Middlewares.Tools;
using EntityFrameworkCore.ORMapping;

namespace EntityFrameworkCore.Collections.Infrastructure;

public class DbSetLogger
{
    private readonly Dictionary<string, EntityLog> _logs = new();

    public DbProxy Proxy { get; set; }

    public DbSetLogger(DbProxy proxy) => Proxy = proxy;

    public void LogAdd(object entity)
    {
        var primaryKeyValue = PortableType.Create(entity.GetType()).GetPrimaryKeyValue(entity);
        if (_logs.ContainsKey(primaryKeyValue))
            _logs.Remove(primaryKeyValue);
        _logs.Add(primaryKeyValue, new EntityLog(entity, EntityState.Added));
    }

    public void LogRemove(object entity)
    {
        var primaryKeyValue = PortableType.Create(entity.GetType()).GetPrimaryKeyValue(entity);
        if (_logs.ContainsKey(primaryKeyValue))
            _logs.Remove(primaryKeyValue);
        _logs.Add(primaryKeyValue, new EntityLog(entity, EntityState.Removed));
    }

    public void LogUpdate(object entity)
    {
        var primaryKeyValue = PortableType.Create(entity.GetType()).GetPrimaryKeyValue(entity);
        if (_logs.ContainsKey(primaryKeyValue))
        {
            var log = _logs[primaryKeyValue];
            switch (log.State)
            {
                case EntityState.Added:
                case EntityState.Updated: log.Entity = entity; return;
                case EntityState.Removed: return;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        _logs.Add(primaryKeyValue, new EntityLog(entity, EntityState.Updated));
    }

    public int Submit()
    {
        var sum = _logs.Values.Sum(log => log.State switch
        {
            EntityState.Added => Proxy.Insert(log.Entity),
            EntityState.Updated => Proxy.Update(log.Entity),
            EntityState.Removed => Proxy.Delete(log.Entity),
            _ => throw new ArgumentOutOfRangeException()
        });
        _logs.Clear();
        return sum;
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