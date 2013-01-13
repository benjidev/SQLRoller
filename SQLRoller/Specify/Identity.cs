namespace SQLRoller.Specify
{
    public class Identity
    {}

    public class Identity<T> : Identity where T : struct 
    {
        public T Seed { get; private set; }
        public T Increment { get; private set; }

        public Identity(T seed, T increment)
        {
            Seed = seed;
            Increment = increment;
        }
        public bool Equals(Identity<T> otherIdentity)
        {
            if (Seed == otherIdentity.Seed && Increment == otherIdentity.Increment)
            {
                return true;
            }
            return false;
        }
    }
}