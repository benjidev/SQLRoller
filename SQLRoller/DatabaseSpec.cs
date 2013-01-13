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
                if (dataType != null)
                {
                    column.Type = dataType.DataType;
                    if (dataLength != null)
                    {
                        column.Length = dataLength.Length;
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
        public SqlDbType DataType { get; set; }

        public int? Length { get; set; }
    }
    public class DataLengthAttribute : Attribute
    {
        public int Length { get; set; }
    }
}