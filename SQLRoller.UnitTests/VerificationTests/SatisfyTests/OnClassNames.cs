using NUnit.Framework;
using SQLRoller.Specify;

namespace SQLRoller.UnitTests.VerificationTests.SatisfyTests
{
    [TestFixture]
    public class OnClassNames : DataBaseTestBase
    {
        /// <summary>
        /// Satisfy means the database has at least the tables and columns specified.
        /// </summary>
        [Test]
        public void ShouldSatisfyWhenClassRepresentsTablePresent()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(ConnectionString);
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
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't");
        }

        [Test]
        public void WhenMultipleTablesAddedIfOneNotPresentInDBTestShouldFail()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<DummyClass>();
            dataspec.AddSchema<Categories>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't");
             
        }

        private class Categories
        {

        } 
        
        private class DummyClass
        {
        }
    }
}