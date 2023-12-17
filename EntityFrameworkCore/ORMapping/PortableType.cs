using System;
using System.Data;
using System.Linq;

namespace EntityFrameworkCore.ORMapping;

public class PortableType
{
    public Type Type { get; }

    public string Name => Type.Name;

    public bool IsPrimitive => Type.IsPrimitive || Type == typeof(string);

    public string PrimaryKey
    {
        get
        {
            foreach (var property in Type.GetProperties())
                if (PortableProperty.Create(property).HasPrimaryKey)
                    return property.Name;
            throw new MissingPrimaryKeyException();
        }
    }

    public PortableProperty[] Properties => Type.GetProperties().Select(PortableProperty.Create).ToArray();

    internal PortableType(Type type) => Type = type;

    public static PortableType Create(Type type) => new PortableType(type);

    public string GetPrimaryKeyValue(object entity)
    {
        foreach (var property in Type.GetProperties())
            if (PortableProperty.Create(property).HasPrimaryKey)
                return property.GetValue(entity)?.ToString();
        throw new MissingPrimaryKeyException();
    }
}