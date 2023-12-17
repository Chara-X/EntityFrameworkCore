namespace EntityFrameworkCore.PropertySerializers.Structure;

public interface ILayerPropertySerializer : IPropertySerializer
{
    IPropertySerializer Secondary { get; set; }
}