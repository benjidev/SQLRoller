using System;
using System.Collections.Generic;
using System.Linq;
using SQLRoller.Attributes;

namespace SQLRoller.Specify
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
                var idendityInt = propertyInfo.GetCustomAttributes<IdentityIntAttribute>(false).FirstOrDefault();
                //this will work for singleKey only (I see a refactoring coming)
                var primaryKey = propertyInfo.GetCustomAttributes<PrimaryKeyAttribute>(false).FirstOrDefault();
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
                    if (idendityInt != null)
                    {
                        column.Identity = new IdentityInt(idendityInt.Seed, idendityInt.Increment);
                    }
                    if (primaryKey != null)
                    {
                        table.PrimaryKey = column;
                    }
                }
                table.Columns.Add(column);
            }

            Tables.Add(table);
        }

    }
}