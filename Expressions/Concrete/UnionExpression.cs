using System;

namespace EntityFrameworkCore.Expressions.Concrete
{
    public class UnionExpression : CachedExpression
    {
        public Type EntityType { get; set; }

        public UnionExpression(Type entityType) => EntityType = entityType;
    }
}
