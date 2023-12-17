using System;
using System.Linq;
using System.Reflection;
using EntityFrameworkCore.ORMapping.Attributes;

namespace EntityFrameworkCore.ORMapping;

public class PortableProperty
{
    private readonly PropertyInfo _property;

    public string Namespace { get;}

    public string Name => _property.Name;

    public string Fullname => Namespace == null ? Name : $"{Namespace}_{Name}";

    public bool HasPrimaryKey => _property.GetCustomAttribute<PrimaryKeyAttribute>() != null;

    public bool HasForeignKey => _property.GetCustomAttribute<ForeignKeyAttribute>() != null;

    public bool IsIdentity => _property.GetCustomAttribute<PrimaryKeyAttribute>()?.AutoIncrement ?? false;

    public bool IsRequired => _property.GetCustomAttribute<RequiredAttribute>() != null;

    public string ForeignKey => _property.GetCustomAttribute<ForeignKeyAttribute>()?.ForeignKey;

    public PortableType Type => PortableType.Create(_property.PropertyType);

    public string ColumnType
    {
        get
        {
            var type = _property.PropertyType;
            if (type == typeof(int) || type == typeof(int?))
                return "INT";
            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return "DATETIME";
            return "NVARCHAR(255)";
        }
    }

    internal PortableProperty(string ns, PropertyInfo property)
    {
        Namespace = ns;
        this._property = property;
    }

    public static PortableProperty Create(PropertyInfo property) => new PortableProperty(null, property);

    public object GetValue(object entity) => _property.GetValue(entity);

    public string GetString(object entity) => GetValue(entity) != null ? $"'{GetValue(entity)}'" : "null";

    public void SetValue(object entity, object value) => _property.SetValue(entity, value);

    public PortableProperty[] Properties => Type.IsPrimitive ? new PortableProperty[0] : Type.Properties.Select(i => new PortableProperty(Fullname, i._property)).ToArray();
}