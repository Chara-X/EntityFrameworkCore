using System.ComponentModel;
using System.Linq.Expressions;
using EntityFrameworkCore.Expressions.Concrete;
using EntityFrameworkCore.ExpressionSerializers.Structure;
using EntityFrameworkCore.PropertySerializers.Tools;
using UnaryExpression = EntityFrameworkCore.Expressions.Structure.UnaryExpression;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete;

public class Layer0StatementSerializer : IRightUnionExpressionSerializer
{
    public IExpressionSerializer Secondary { get; set; } = new Layer1BinarySerializer();

    public string Serialize(Expression exp) => Base(exp);

    private string Base(Expression exp) =>
        exp switch
        {
            UnionExpression e => Union(e),
            SelectExpression e => Select(e),
            WhereExpression e => Where(e),
            MaxExpression e => Max(e),
            MinExpression e => Min(e),
            SumExpression e => Sum(e),
            CountExpression e => Count(e),
            TakeExpression e => Take(e),
            SkipExpression e => Skip(e),
            DistinctExpression e => Distinct(e),
            OrderByExpression e => OrderBy(e),
            OrderByDescendingExpression e => OrderByDescending(e),
            _ => throw new InvalidEnumArgumentException()
        };

    private static string Union(UnionExpression exp) => $"SELECT {PropertiesSerializer.ColumnsOfUnion(exp.EntityType,"this")} FROM [{exp.EntityType.Name}] AS this {PropertiesSerializer.LeftJoinsOfUnion(exp.EntityType,"this")}";

    private string Select(SelectExpression exp) => $"SELECT {PropertiesSerializer.ColumnsOfSub(exp.Selector.ReturnType,Secondary.Serialize(exp.Selector.Body))} FROM ({Serialize(exp.Sub)}) AS this";

    private string Where(WhereExpression exp) => $"SELECT * FROM ({Serialize(exp.Sub)}) AS this WHERE {Secondary.Serialize(exp.Predicate.Body)}";

    private string Max(MaxExpression exp) => $"SELECT MAX({Secondary.Serialize(exp.Selector.Body)}) FROM ({Serialize(exp.Sub)}) AS this";

    private string Min(MinExpression exp) => $"SELECT MIN({Secondary.Serialize(exp.Selector.Body)}) FROM ({Serialize(exp.Sub)}) AS this";

    private string Sum(SumExpression exp) => $"SELECT SUM({Secondary.Serialize(exp.Selector.Body)}) FROM ({Serialize(exp.Sub)}) AS this";

    private string Count(UnaryExpression exp) => $"SELECT COUNT(*) FROM ({Serialize(exp.Sub)}) AS this";

    private string Take(TakeExpression exp) => $"SELECT TOP({exp.Count}) * FROM ({Serialize(exp.Sub)}) AS this {(exp.Predicate == null ? null : "WHERE " + Secondary.Serialize(exp.Predicate.Body))}";

    private string Skip(SkipExpression exp) => $"SELECT * FROM ({Serialize(exp.Sub)}) AS this ORDER BY {Secondary.Serialize(exp.KeySelector.Body)} OFFSET {exp.Count} ROWS";

    private string Distinct(UnaryExpression exp) => $"SELECT DISTINCT * FROM ({Serialize(exp.Sub)}) AS this";

    private string OrderBy(OrderByExpression exp) => $"SELECT * FROM ({Serialize(exp.Sub)}) AS this ORDER BY {Secondary.Serialize(exp.KeySelector.Body)} OFFSET 0 ROWS";

    private string OrderByDescending(OrderByDescendingExpression exp) => $"SELECT * FROM ({Serialize(exp.Sub)}) AS this ORDER BY {Secondary.Serialize(exp.KeySelector.Body)} DESC OFFSET 0 ROWS";
}