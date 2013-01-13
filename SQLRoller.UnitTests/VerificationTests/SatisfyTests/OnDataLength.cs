using System.Data;
using NUnit.Framework;
using SQLRoller.Attributes;

namespace SQLRoller.UnitTests.VerificationTests.SatisfyTests
{
    [TestFixture]
    public class OnDataLength : DataBaseTestBase
    {
        [Test]
        public void ShouldNotSatisfyIfColumnDataLengthDoesNotMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't, due to incorrect DataLengths");
        }

        [Test]
        public void ShouldSatisfyIfColumnDataLengthDoesMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Shippers>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.True, "database didn't satisfy dataspec, when should've, due to correct DataLength");
        }

        private class Categories
        {
            [DataType(SqlDbType.Int)]
            public int CategoryID { get; set; }
            [DataType(SqlDbType.NVarChar), DataLength(20)]
            public string CategoryName { get; set; }
            [DataType(SqlDbType.NText)]
            public string Description { get; set; }
            [DataType(SqlDbType.Image)]
            public byte[] Picture { get; set; }
        }

        private class Shippers
        {
            [DataType(SqlDbType.Int)]
            public int ShipperID { get; set; }
            [DataType(SqlDbType.NVarChar), DataLength(40)]
            public string CompanyName { get; set; }
            [DataType(SqlDbType.NVarChar), DataLength(24)]
            public string Phone { get; set; }
        }
    }
}