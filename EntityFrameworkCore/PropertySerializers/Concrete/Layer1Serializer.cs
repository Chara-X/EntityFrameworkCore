using System.ComponentModel;
using EntityFrameworkCore.ORMapping;
using EntityFrameworkCore.Tools;

namespace EntityFrameworkCore.PropertySerializers.Concrete;

public class Layer1Serializer : IPropertySerializer
{
    public string Serialize(PortableProperty property, string prefix, SerializeOption option) =>
        Base(property, prefix, option);

    private static string Base(PortableProperty property, string prefix, SerializeOption option) =>
        option switch
        {
            SerializeOption.ColumnOfSub => ColumnsSub(property, prefix),
            SerializeOption.ColumnOfUnion => ColumnsUnion(property, prefix),
            SerializeOption.LeftJoinOfUnion => null,
            _ => throw new InvalidEnumArgumentException()
        };

    private static string ColumnsSub(PortableProperty property, string prefix) => $"\n{($"{prefix}.{property.Fullname}").ReplaceFirst()} AS [{property.Fullname}],";

    private static string ColumnsUnion(PortableProperty property, string prefix) => $"\n{($"{prefix}.{property.Fullname}").ReplaceLast()} AS [{property.Fullname}],";
}