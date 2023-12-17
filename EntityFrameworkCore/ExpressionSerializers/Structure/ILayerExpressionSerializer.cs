namespace EntityFrameworkCore.ExpressionSerializers.Structure;

public interface ILayerExpressionSerializer : IExpressionSerializer
{
    IExpressionSerializer Secondary { get; set; }
}