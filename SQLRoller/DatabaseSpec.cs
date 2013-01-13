using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SQLRoller
{
    public class DatabaseSpec
    {
        public IList<Table> Tables { get; set; }

        public DatabaseSpec()
        {
            Tables = new List<Table>();
        }

        public void AddSchema<T>()
        {
            Type newType = typeof(T);

            var table = new Table(newType.Name);
            foreach (var propertyInfo in newType.GetProperties())
            {
                var column = new Column(propertyInfo.Name);

                var dataType = propertyInfo.GetCustomAttributes<DataTypeAttribute>(false).FirstOrDefault();
                var dataLength = propertyInfo.GetCustomAttributes<DataLengthAttribute>(false).FirstOrDefault();
                var allowNulls = propertyInfo.GetCustomAttributes<AllowNullsAttribute>(false).FirstOrDefault();
                if (dataType != null)
                {
                    column.Type = dataType.Value;
                    if (dataLength != null)
                    {
                        column.Length = dataLength.Value;
                    }
                    if (allowNulls != null)
                    {
                        column.AllowNulls = allowNulls.Value;
                    }
                }
                table.Columns.Add(column);
            }

            Tables.Add(table);
        }

        public class Table
        {
            public Table(string name)
            {
                Name = name;
                Columns = new List<Column>();
            }
            public string Name { get; private set; }

            public IList<Column> Columns { get; set; }
        }
        public class Column
        {
            public Column(string name)
            {
                Name = name;
            }
            public string Name { get; private set; }
            public SqlDbType? Type { get; set; }
            public int? Length { get; set; }
            public bool? AllowNulls { get; set; }

            public bool HasTheSameDataTypeAs(Database.SchemaColumn existingType)
            {
                if (Type.HasValue && !existingType.Type.Equals(Type.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                if (Length.HasValue && existingType.Length != Length)
                {
                    return false;
                }
                if (AllowNulls.HasValue && existingType.AllowNulls != AllowNulls)
                {
                    return false;
                }
                return true;
            }

        }
    }

    public static class Extensions
    {
        public static IList<T> GetCustomAttributes<T>(this PropertyInfo pi, bool inherit)
        {
            var customAttributes = pi.GetCustomAttributes(typeof (T), inherit);
            return customAttributes.Select(customAttribute => (T) customAttribute).ToList();
        }
    }

    public class DataTypeAttribute : Attribute
    {
        public DataTypeAttribute(SqlDbType value)
        {
            Value = value;
        }
        public SqlDbType Value { get; set; }
    }
    public class DataLengthAttribute : Attribute
    {
        public DataLengthAttribute(int value)
        {
            Value = value;
        }
        public int Value { get; set; }
    }
    public class AllowNullsAttribute : Attribute
    {
        public AllowNullsAttribute(bool value)
        {
            Value = value;
        }
        public bool Value { get; set; }
    }
}