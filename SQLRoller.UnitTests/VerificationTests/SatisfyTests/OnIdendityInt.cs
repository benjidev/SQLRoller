using NUnit.Framework;
using SQLRoller.Attributes;

namespace SQLRoller.UnitTests.VerificationTests.SatisfyTests
{
    [TestFixture]
    public class OnIdendityInt : DataBaseTestBase
    {
        [Test]
        public void FailIfWrongFieldLabeled()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.False);
        }

        [Test]
        public void FailIfIncrementDoesNotMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Shippers>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.False);
        }

        [Test]
        public void FailIfSeedDoesNotMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Employees>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.False);
        }

        [Test]
        public void SucceedIfEverythingMatches()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Employees>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.True);
        }

        //Wrong field Labeled
        private class Categories
        {
            [IdentityInt(1,1)]
            public string CategoryName { get; set; }
        }
        //Right Field Wrong Increment
        private class Shippers
        {
            [IdentityInt(1,2)]
            public int ShipperID { get; set; }
        }
        //Right Field Wrong Seed
        private class Employees
        {
            [IdentityInt(2, 1)]
            public int EmployeeID { get; set; }
        }
        //Right Everything
        private class Suppliers
        {
            [IdentityInt(1, 1)]
            public int SupplierID { get; set; }
        }
    }
}