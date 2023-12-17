using System;
using System.ComponentModel;
using System.Linq;
using EntityFrameworkCore.ORMapping;
using EntityFrameworkCore.PropertySerializers.Structure;

namespace EntityFrameworkCore.PropertySerializers.Concrete;

public class Layer0NavigationSerializer : ILayerPropertySerializer
{
    public IPropertySerializer Secondary { get; set; } = new Layer1Serializer();

    public string Serialize(PortableProperty property,string prefix, SerializeOption option) => Base(property,prefix, option);

    private string Base(PortableProperty property, string prefix, SerializeOption option) =>
        property.HasForeignKey switch
        {
            true => option switch
            {
                SerializeOption.ColumnOfSub => ColumnOfSub(property, prefix),
                SerializeOption.ColumnOfUnion => ColumnOfUnion(property, prefix),
                SerializeOption.LeftJoinOfUnion => LeftJoinOfUnion(property, prefix),
                _ => throw new InvalidEnumArgumentException()
            },
            _ => Secondary.Serialize(property, prefix, option)
        };

    private string ColumnOfSub(PortableProperty property, string prefix) => Properties(property, prefix, SerializeOption.ColumnOfSub, Base);

    private string ColumnOfUnion(PortableProperty property, string prefix) => Properties(property, prefix, SerializeOption.ColumnOfUnion, Base);

    private string LeftJoinOfUnion(PortableProperty property, string prefix) => $"\nLEFT JOIN [{property.Type.Name}] AS [{prefix}_{property.Fullname}] ON [{prefix}_{property.Fullname}].[{property.Type.PrimaryKey}] = [{prefix + (property.Namespace == null ? null : '_' + property.Namespace)}].[{property.ForeignKey}] {Properties(property, prefix, SerializeOption.LeftJoinOfUnion, Base)}";
    
    private static string Properties(PortableProperty property, string prefix, SerializeOption option, Func<PortableProperty, string, SerializeOption, string> serialize) => property.Properties.Select(i => serialize(i, prefix, option)).Aggregate(string.Empty, (current, i) => current + i);
}