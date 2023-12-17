using System;
using System.Linq;
using EntityFrameworkCore.ORMapping;
using EntityFrameworkCore.PropertySerializers.Concrete;

namespace EntityFrameworkCore.PropertySerializers.Tools;

public static class PropertiesSerializer
{
    private static readonly Layer0NavigationSerializer Serializer = new();

    public static string ColumnsOfSub(Type type, string ns) => Properties(PortableType.Create(type), ns, SerializeOption.ColumnOfSub, Serializer.Serialize)[0..^1];

    public static string ColumnsOfUnion(Type type, string ns) => Properties(PortableType.Create(type), ns, SerializeOption.ColumnOfUnion, Serializer.Serialize)[0..^1];

    public static string LeftJoinsOfUnion(Type type, string ns) => Properties(PortableType.Create(type), ns, SerializeOption.LeftJoinOfUnion, Serializer.Serialize);

    private static string Properties(PortableType type, string ns, SerializeOption option, Func<PortableProperty, string, SerializeOption, string> serialize) => type.Properties.Select(i => serialize(i, ns, option)).Aggregate(string.Empty, (current, i) => current + i);
}