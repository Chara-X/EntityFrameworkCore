using System.ComponentModel;
using System.Linq.Expressions;

namespace EntityFrameworkCore.ExpressionSerializers.Concrete
{
    public class Layer9FactorSerializer : IExpressionSerializer
    {
        public string Serialize(Expression exp) => Base(exp);

        private static string Base(Expression exp)
        {
            return exp.NodeType switch
            {
                ExpressionType.Constant => Constant((ConstantExpression) exp),
                ExpressionType.Call => Call((MethodCallExpression)exp),
                ExpressionType.Parameter => "this",
                _ => throw new InvalidEnumArgumentException(exp.NodeType.ToString())
            };
        }

        private static string Constant(ConstantExpression exp) => '\'' + exp.Value.ToString() + '\'';

        private static string Call(Expression exp) => Expression.Lambda(exp, null).Compile().DynamicInvoke()?.ToString();
    }
}
