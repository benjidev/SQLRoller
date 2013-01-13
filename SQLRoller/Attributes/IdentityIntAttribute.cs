using System;

namespace SQLRoller.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class IdentityIntAttribute : Attribute
    {

        public IdentityIntAttribute(int seed, int increment)
        {
            Increment = increment;
            Seed = seed;
        }

        public int Seed { get; private set; }
        public int Increment { get; private set; }
    }
}