using System;
using System.Linq;
using EntityFrameworkCore.ORMapping;
using EntityFrameworkCore.PropertySerializers.Concrete;

namespace EntityFrameworkCore.PropertySerializers.Tools
{
    public static class PropertiesSerializer
    {
        private static readonly Layer0NavigatePropertySerializer Serializer = new Layer0NavigatePropertySerializer();

        public static string ColumnsSub(Type type, string ns) => Properties(PortableType.Create(type), ns, SerializeOption.ColumnsSub, Serializer.Serialize)[0..^1];

        public static string ColumnsUnion(Type type, string ns) => Properties(PortableType.Create(type), ns, SerializeOption.ColumnsUnion, Serializer.Serialize)[0..^1];

        public static string LeftJoinsUnion(Type type, string ns) => Properties(PortableType.Create(type), ns, SerializeOption.LeftJoinsUnion, Serializer.Serialize);

        private static string Properties(PortableType type, string ns, SerializeOption option, Func<PortableProperty, string, SerializeOption, string> serialize) => type.Properties.Select(i => serialize(i, ns, option)).Aggregate(string.Empty, (current, i) => current + i);
    }
}
