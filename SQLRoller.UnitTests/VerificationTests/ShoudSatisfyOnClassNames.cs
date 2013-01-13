using NUnit.Framework;

namespace SQLRoller.UnitTests.VerificationTests
{
    [TestFixture]
    public class OnClassNames
    {
        private const string connectionString = "Integrated Security=SSPI;Initial Catalog=NorthWind;Data Source=ASUS-PC";

        /// <summary>
        /// Satisfy means the database has at least the tables and columns specified.
        /// </summary>
        [Test]
        public void ShouldSatisfyWhenClassRepresentsTablePresent()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(connectionString);
            Assert.That(database.Satisfies(dataspec), Is.True, "database did not satisfy dataspec");
        }

        /// <summary>
        /// Satisfy means the database has at least the tables and columns specified.
        /// </summary>
        [Test]
        public void ShouldNotSatisfyWhenClassRepresentsTableNotPresent()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<DummyClass>();
            var database = new Database(connectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't");
        }

        [Test]
        public void WhenMultipleTablesAddedIfOneNotPresentInDBTestShouldFail()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<DummyClass>();
            dataspec.AddSchema<Categories>();
            var database = new Database(connectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't");
             
        }

        private class Categories
        {

        } 
        
        private class DummyClass
        {
        }
    }

    [TestFixture]
    public class OnColumnNames
    {
        const string connectionString = "Integrated Security=SSPI;Initial Catalog=NorthWind;Data Source=ASUS-PC";
        [Test]
        public void ShouldNotSatisfyIfColumnNotPresentInDB()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(connectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't, due to incorrect column names");
        }

        private class Categories
        {
            public int CategoryID { get; set; }
            public string CategoryNames { get; set; }
            public string Description { get; set; }
            public byte[] Picture { get; set; }

        } 
    }
}
