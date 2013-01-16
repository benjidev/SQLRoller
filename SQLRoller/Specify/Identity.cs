namespace SQLRoller.Specify
{
    public abstract class Identity
    {
        public abstract bool Equals(Identity otherIdentity);
    }

    public class IdentityInt : Identity
    {
        public int Seed { get; private set; }
        public int Increment { get; private set; }

        public IdentityInt(int seed, int increment)
        {
            Seed = seed;
            Increment = increment;
        }
        public override bool Equals(Identity otherIdentity)
        {
            var otherIdInt = otherIdentity as IdentityInt;
            if (otherIdInt != null && Seed == otherIdInt.Seed && Increment == otherIdInt.Increment)
            {
                return true;
            }
            return false;
        }
    }
}