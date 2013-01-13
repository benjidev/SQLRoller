using System;
using System.Data;

namespace SQLRoller.Attributes
{
    public class DataTypeAttribute : Attribute
    {
        public DataTypeAttribute(SqlDbType value)
        {
            Value = value;
        }
        public SqlDbType Value { get; set; }
    }
}