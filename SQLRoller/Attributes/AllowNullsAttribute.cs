using System;

namespace SQLRoller.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class AllowNullsAttribute : Attribute
    {
        public AllowNullsAttribute(bool value)
        {
            Value = value;
        }
        public bool Value { get; private set; }
    }
}