using System.Linq.Expressions;

namespace EntityFrameworkCore.Expressions
{
    public abstract class CachedExpression : Expression
    {
        public string Sql { get; set; }
    }
}
