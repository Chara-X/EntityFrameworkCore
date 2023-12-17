using System;
using System.Linq.Expressions;

namespace EntityFrameworkCore.Expressions.Concrete;

public class UnionExpression : Expression
{
    public Type EntityType { get; set; }

    public UnionExpression(Type entityType) => EntityType = entityType;
}