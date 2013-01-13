using System.Collections.Generic;

namespace SQLRoller.Specify
{
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
}