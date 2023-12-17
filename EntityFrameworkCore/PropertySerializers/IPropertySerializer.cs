using EntityFrameworkCore.ORMapping;

namespace EntityFrameworkCore.PropertySerializers;

public interface IPropertySerializer
{
    string Serialize(PortableProperty property, string prefix, SerializeOption option);
}

public enum SerializeOption
{
    ColumnOfSub,
    ColumnOfUnion,
    LeftJoinOfUnion,
}