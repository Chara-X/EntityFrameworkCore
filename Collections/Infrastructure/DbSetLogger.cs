using System.Collections.Generic;
using System.Linq;
using EntityFrameworkCore.Middlewares.Tools;
using EntityFrameworkCore.ORMapping;

namespace EntityFrameworkCore.Collections.Infrastructure
{
    public class DbSetLogger
    {
        private readonly Dictionary<string, object> added  = new Dictionary<string, object>();

        private readonly Dictionary<string, object> updated = new Dictionary<string, object>();

        private readonly Dictionary<string, object> removed = new Dictionary<string, object>();

        public DbProxy Proxy { get; set; }

        public DbSetLogger(DbProxy proxy) => Proxy = proxy;

        public void LogAdd(object entity)
        {
            var primaryKeyValue = PortableType.Create(entity.GetType()).GetPrimaryKeyValue(entity);
            if (removed.ContainsKey(primaryKeyValue))
                removed.Remove(primaryKeyValue);
            if (updated.ContainsKey(primaryKeyValue))
                updated.Remove(primaryKeyValue);
            added.Add(primaryKeyValue, entity);
        }

        public void LogRemove(object entity)
        {
            var primaryKeyValue = PortableType.Create(entity.GetType()).GetPrimaryKeyValue(entity);
            if (added.ContainsKey(primaryKeyValue))
                added.Remove(primaryKeyValue);
            if (updated.ContainsKey(primaryKeyValue))
                updated.Remove(primaryKeyValue);
            removed.Add(primaryKeyValue, entity);
        }

        public void LogUpdate(object entity)
        {
            var primaryKeyValue = PortableType.Create(entity.GetType()).GetPrimaryKeyValue(entity);
            if (removed.ContainsKey(primaryKeyValue))
                removed.Remove(primaryKeyValue);
            if (added.ContainsKey(primaryKeyValue))
                added.Remove(primaryKeyValue);
            updated.Add(primaryKeyValue, entity);
        }

        public int Submit()=> added.Values.Sum(entity => Proxy.Insert(entity)) + updated.Values.Sum(entity => Proxy.Update(entity)) + removed.Values.Sum(entity => Proxy.Delete(entity));
    }
}
