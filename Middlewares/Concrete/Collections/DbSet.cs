using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using EntityFrameworkCore.Collections.Infrastructure;
using EntityFrameworkCore.Expressions.Concrete;
using EntityFrameworkCore.Middlewares.Tools;

namespace EntityFrameworkCore.Collections
{
    public class DbSet<T> : IEnumerable<T>
    {
        private readonly DbSetLogger logger;

        public DbProxy Proxy => logger.Proxy;

        public Expression Expression { get; set; }

        internal DbSet(DbSetLogger logger)
        {
            this.logger = logger;
            Expression = new UnionExpression(typeof(T));
        }

        internal DbSet(DbSetLogger logger, Expression expression)
        {
            this.logger = logger;
            Expression = expression;
        }

        public void Add(T entity) => logger.LogAdd(entity);

        public void Remove(T entity) => logger.LogRemove(entity);

        public void Update(T entity) => logger.LogUpdate(entity);

        public IEnumerator<T> GetEnumerator() => Proxy.Reader<T>(Expression).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
