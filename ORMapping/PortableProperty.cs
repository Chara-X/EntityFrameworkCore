using System.Linq;
using System.Reflection;
using EntityFrameworkCore.ORMapping.Attributes;

namespace EntityFrameworkCore.ORMapping
{
    public class PortableProperty
    {
        private readonly PropertyInfo property;

        public string Namespace { get;}

        public string Name => property.Name;

        public string Fullname => Namespace == null ? Name : $"{Namespace}_{Name}";

        public bool HasPrimaryKey => property.GetCustomAttribute<PrimaryKeyAttribute>() != null;

        public bool HasForeignKey => property.GetCustomAttribute<ForeignKeyAttribute>() != null;

        public bool IsRequired => property.GetCustomAttribute<RequiredAttribute>() != null;

        public string ForeignKey => property.GetCustomAttribute<ForeignKeyAttribute>()?.ForeignKey;

        public PortableType Type => PortableType.Create(property.PropertyType);

        public string ColumnType
        {
            get
            {
                var type = property.PropertyType;
                if (type == typeof(int) || type == typeof(int?))
                    return "INT";
                return "NVARCHAR(255)";
            }
        }

        internal PortableProperty(string ns, PropertyInfo property)
        {
            Namespace = ns;
            this.property = property;
        }

        public static PortableProperty Create(PropertyInfo property) => new PortableProperty(null, property);

        public object GetValue(object entity) => property.GetValue(entity);

        public string GetString(object entity) => GetValue(entity) != null ? $"'{GetValue(entity)}'" : "null";

        public void SetValue(object entity, object value) => property.SetValue(entity, value);

        public PortableProperty[] Properties => Type.IsPrimitive ? new PortableProperty[0] : Type.Properties.Select(i => new PortableProperty(Fullname, i.property)).ToArray();
    }
}
