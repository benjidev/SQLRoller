using System;
using System.Collections.Generic;

namespace SQLRoller
{
    public class DatabaseSpec
    {
        public IList<Table> tables { get; set; }

        public DatabaseSpec()
        {
            tables = new List<Table>();
        }

        public void AddSchema<T>()
        {
            Type newType = typeof(T);

            var table = new Table(newType.Name);
            foreach (var propertyInfo in newType.GetProperties())
            {
                table.Columns.Add(propertyInfo.Name);
            }

            tables.Add(table);
        }

        public class Table
        {
            public Table(string name)
            {
                Name = name;
                Columns = new List<string>();
            }
            public string Name { get; private set; }

            public IList<string> Columns { get; set; }
        }
    }
}