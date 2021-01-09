using System;
using System.ComponentModel;
using System.Linq;
using EntityFrameworkCore.ORMapping;
using EntityFrameworkCore.PropertySerializers.Structure;

namespace EntityFrameworkCore.PropertySerializers.Concrete
{
    public class Layer0NavigatePropertySerializer : ILayerPropertySerializer
    {
        public IPropertySerializer Secondary { get; set; } = new Layer1PropertySerializer();

        public string Serialize(PortableProperty property,string ns, SerializeOption option) => Base(property,ns, option);

        private string Base(PortableProperty property, string ns, SerializeOption option)
        {
            return property.HasForeignKey switch
            {
                true => option switch
                {
                    SerializeOption.ColumnsSub => ColumnsSub(property, ns),
                    SerializeOption.ColumnsUnion => ColumnsUnion(property, ns),
                    SerializeOption.LeftJoinsUnion => LeftJoinsUnion(property, ns),
                    _ => throw new InvalidEnumArgumentException()
                },
                _ => Secondary.Serialize(property, ns, option)
            };
        }

        private string ColumnsSub(PortableProperty property, string ns) => Properties(property, ns, SerializeOption.ColumnsSub, Base);

        private string ColumnsUnion(PortableProperty property, string ns) => Properties(property, ns, SerializeOption.ColumnsUnion, Base);

        private string LeftJoinsUnion(PortableProperty property, string ns) => $"\nLEFT JOIN {property.Type.Name} AS {ns}_{property.Fullname} ON {ns}_{property.Fullname}.{property.Type.PrimaryKey} = {ns}{(property.Namespace == null ? null : '_' + property.Namespace)}.{property.ForeignKey} {Properties(property, ns, SerializeOption.LeftJoinsUnion, Base)}";

        private static string Properties(PortableProperty property, string ns, SerializeOption option, Func<PortableProperty, string, SerializeOption, string> serialize) => property.Properties.Select(i => serialize(i, ns, option)).Aggregate(string.Empty, (current, i) => current + i);
    }
}
