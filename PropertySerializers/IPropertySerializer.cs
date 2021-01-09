using EntityFrameworkCore.ORMapping;

namespace EntityFrameworkCore.PropertySerializers
{
    public interface IPropertySerializer
    {
        string Serialize(PortableProperty property, string ns, SerializeOption option);
    }

    public enum SerializeOption
    {
        ColumnsSub,
        ColumnsUnion,
        JoinsUnion,
    }
}
