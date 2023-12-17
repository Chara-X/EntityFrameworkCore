using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using EntityFrameworkCore.ORMapping;

namespace EntityFrameworkCore.Tools;

public static class ActivatorOfDictionary
{
    public static T CreateInstance<T>(IReadOnlyDictionary<string, object> row) =>
        typeof(T).GetCustomAttribute<CompilerGeneratedAttribute>() != null
            ? (T) CreateAnonymous(typeof(T), row)
            : (T) CreateInstance(typeof(T), row);

    private static object CreateInstance(Type type, IReadOnlyDictionary<string, object> row)
    {
        var entity = CreateDefault(type);
        var properties = PortableType.Create(type).Properties;
        foreach (var i in properties)
            i.SetValue(entity, Base(i, row));
        return entity;
    }

    private static object Base(PortableProperty property, IReadOnlyDictionary<string, object> row) =>
        property.HasForeignKey switch
        {
            false => row[property.Fullname],
            true => Navigation(property, row),
        };

    private static object Navigation(PortableProperty property, IReadOnlyDictionary<string, object> row)
    {
        var entity = CreateDefault(property.Type.Type);
        foreach (var i in property.Properties)
            i.SetValue(entity, Base(i, row));
        return entity;
    }

    private static object CreateDefault(Type type) => Activator.CreateInstance(type, type.GetConstructors()[0].GetParameters().Select(i => i.ParameterType).Select(i => i.IsValueType ? Activator.CreateInstance(i) : null).ToArray());

    private static object CreateAnonymous(Type type, IReadOnlyDictionary<string, object> row) => Activator.CreateInstance(type, type.GetConstructors()[0].GetParameters().Select(i => i.Name).Select(name => row[name]).ToArray());
}