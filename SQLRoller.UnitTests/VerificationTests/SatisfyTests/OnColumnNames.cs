using NUnit.Framework;

namespace SQLRoller.UnitTests.VerificationTests.SatisfyTests
{
    [TestFixture]
    public class OnColumnNames:DataBaseTestBase
    {
        [Test]
        public void ShouldNotSatisfyIfColumnNotPresentInDb()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't, due to incorrect column names");
        }

        [Test]
        public void ShouldSatisfyIfAllColumnsPresentInDb()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Region>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.True, "database didn't satisfy dataspec, all column names present");
        }

        private class Categories
        {
            public int CategoryID { get; set; }
            public string CategoryNames { get; set; }
            public string Description { get; set; }
            public byte[] Picture { get; set; }
        } 
        private class Region
        {
            public int RegionID { get; set; }
            public string RegionDescription { get; set; }
        }
    }
}