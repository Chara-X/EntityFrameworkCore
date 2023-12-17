using System;
using System.Linq;
using System.Linq.Expressions;
using EntityFrameworkCore.Collections.Infrastructure;
using EntityFrameworkCore.Expressions.Concrete;

namespace EntityFrameworkCore.Collections.Extensions;

public static class QueryExtensions
{
    public static DbSet<T> Where<T>(this DbSet<T> source, Expression<Func<T, bool>> predicate) => new DbSet<T>(new DbSetLogger(source.Proxy), new WhereExpression(source.Expression, predicate));

    public static DbSet<TD> Select<TS, TD>(this DbSet<TS> source, Expression<Func<TS, TD>> selector) => new DbSet<TD>(new DbSetLogger(source.Proxy), new SelectExpression(source.Expression, selector));

    public static DbSet<T> Distinct<T>(this DbSet<T> source) => new DbSet<T>(new DbSetLogger(source.Proxy), new DistinctExpression(source.Expression));

    public static DbSet<T> Take<T>(this DbSet<T> source, int count) => new DbSet<T>(new DbSetLogger(source.Proxy), new TakeExpression(source.Expression, null, count));

    public static DbSet<T> Take<T>(this DbSet<T> source, Expression<Func<T, bool>> predicate, int count) => new DbSet<T>(new DbSetLogger(source.Proxy), new TakeExpression(source.Expression, predicate, count));

    public static DbSet<T> Skip<T, TK>(this DbSet<T> source, Expression<Func<T, TK>> keySector, int count) => new DbSet<T>(new DbSetLogger(source.Proxy), new SkipExpression(source.Expression, keySector, count));
       
    public static DbSet<T> OrderBy<T, TK>(this DbSet<T> source, Expression<Func<T, TK>> keySector) => new DbSet<T>(new DbSetLogger(source.Proxy), new OrderByExpression(source.Expression, keySector));

    public static DbSet<T> OrderByDescending<T, TK>(this DbSet<T> source, Expression<Func<T, TK>> keySector) => new DbSet<T>(new DbSetLogger(source.Proxy), new OrderByDescendingExpression(source.Expression, keySector));

    public static T First<T>(this DbSet<T> source) => source.Proxy.Reader<T>(new TakeExpression(source.Expression, null, 1)).FirstOrDefault();

    public static T First<T>(this DbSet<T> source, Expression<Func<T, bool>> predicate) => source.Proxy.Reader<T>(new TakeExpression(source.Expression, predicate, 1)).FirstOrDefault();

    public static int Count<T>(this DbSet<T> source) => (int) source.Proxy.Scalar(new CountExpression(source.Expression));

    public static TD Sum<TS,TD>(this DbSet<TS> source, Expression<Func<TS, TD>> selector) => (TD) source.Proxy.Scalar(new SumExpression(source.Expression, selector));

    public static TD Max<TS,TD>(this DbSet<TS> source, Expression<Func<TS, TD>> selector) => (TD)source.Proxy.Scalar(new MaxExpression(source.Expression, selector));

    public static TD Min<TS,TD>(this DbSet<TS> source, Expression<Func<TS, TD>> selector) => (TD)source.Proxy.Scalar(new MinExpression(source.Expression, selector));
}