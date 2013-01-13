using System.Data;
using NUnit.Framework;
using SQLRoller.Attributes;

namespace SQLRoller.UnitTests.VerificationTests.SatisfyTests
{
    [TestFixture]
    public class OnDataTypes : DataBaseTestBase
    {
        [Test]
        public void ShouldNotSatisfyIfColumnDataTypeDoesNotMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't, due to incorrect DataTypes");
        }

        [Test]
        public void ShouldSatisfyIfColumnDataTypeDoesMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Region>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.True, "database didn't satisfy dataspec, when should've, due to correct DataTypes");
        }

        private class Categories
        {
            [DataType(SqlDbType.Int)]
            public int CategoryID { get; set; }
            [DataType(SqlDbType.NVarChar)]
            public string CategoryName { get; set; }
            [DataType(SqlDbType.NVarChar)]
            public string Description { get; set; }
            [DataType(SqlDbType.NVarChar)]
            public byte[] Picture { get; set; }
        }

        private class Region
        {
            [DataType(SqlDbType.Int)]
            public int RegionID { get; set; }
            [DataType(SqlDbType.NChar)]
            public string RegionDescription { get; set; }
        }
    }
}