using System;
using System.Data;

namespace SQLRoller.Specify
{
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
        public Identity Identity { get; set; }

        public bool IsSatisfiedBy(Column existingType)
        {
            if (Type.HasValue && existingType.Type != Type)
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
            if (Identity != null && !Identity.Equals(existingType.Identity))
            {
                return false;
            }
            return true;
        }

    }
}