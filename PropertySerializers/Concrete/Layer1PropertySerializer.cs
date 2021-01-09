using System.ComponentModel;
using EntityFrameworkCore.ORMapping;
using EntityFrameworkCore.Tools;

namespace EntityFrameworkCore.PropertySerializers.Concrete
{
    public class Layer1PropertySerializer : IPropertySerializer
    {
        public string Serialize(PortableProperty property, string prefix, SerializeOption option) =>
            Base(property, prefix, option);

        private static string Base(PortableProperty property, string ns, SerializeOption option)
        {
            return property.HasForeignKey switch
            {
                false => option switch
                {
                    SerializeOption.ColumnsSub => ColumnsSub(property, ns),
                    SerializeOption.ColumnsUnion => ColumnsUnion(property, ns),
                    SerializeOption.LeftJoinsUnion => null,
                    _ => throw new InvalidEnumArgumentException()
                },
                _ => throw new InvalidEnumArgumentException()
            };
        }

        private static string ColumnsSub(PortableProperty property, string ns) => $"\n{($"{ns}.{property.Fullname}").ReplaceFirst()} AS {property.Fullname},";

        private static string ColumnsUnion(PortableProperty property, string ns) => $"\n{($"{ns}.{property.Fullname}").ReplaceLast()} AS {property.Fullname},";
    }
}
