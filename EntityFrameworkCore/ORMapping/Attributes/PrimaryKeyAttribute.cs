using System;

namespace EntityFrameworkCore.ORMapping.Attributes;

public class PrimaryKeyAttribute : Attribute
{
    public bool AutoIncrement { get; set; }

    public PrimaryKeyAttribute() { }

    public PrimaryKeyAttribute(bool autoIncrement) => AutoIncrement = autoIncrement;
}