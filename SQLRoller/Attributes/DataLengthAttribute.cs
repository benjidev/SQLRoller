using System;

namespace SQLRoller.Attributes
{
    public class DataLengthAttribute : Attribute
    {
        public DataLengthAttribute(int value)
        {
            Value = value;
        }
        public int Value { get; set; }
    }
}