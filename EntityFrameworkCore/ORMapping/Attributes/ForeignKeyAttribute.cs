using System;

namespace EntityFrameworkCore.ORMapping.Attributes;

public class ForeignKeyAttribute : Attribute
{
    public string ForeignKey { get; set; }

    public ForeignKeyAttribute(string foreignKey) => ForeignKey = foreignKey;
}