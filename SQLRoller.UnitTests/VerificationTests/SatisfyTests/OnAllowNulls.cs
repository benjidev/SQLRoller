using NUnit.Framework;
using System.Data;
using SQLRoller.Attributes;
using SQLRoller.Specify;

namespace SQLRoller.UnitTests.VerificationTests.SatisfyTests
{
    [TestFixture]
    public class OnAllowNulls : DataBaseTestBase
    {
        [Test]
        public void ShouldNotSatisfyIfColumnAllowNullsDoesNotMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't, due to incorrect AllowNulls");
        }

        [Test]
        public void ShouldSatisfyIfColumnAllowNullsDoesMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Shippers>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.True, "database didn't satisfy dataspec, when should've, due to correct AllowNulls");
        }

        private class Categories
        {
            [DataType(SqlDbType.Int), AllowNulls(false)]
            public int CategoryID { get; set; }
            [DataType(SqlDbType.NVarChar), DataLength(15), AllowNulls(false)]
            public string CategoryName { get; set; }
            [DataType(SqlDbType.NText), AllowNulls(false)]
            public string Description { get; set; }
            [DataType(SqlDbType.Image), AllowNulls(true)]
            public byte[] Picture { get; set; }
        }

        private class Shippers
        {
            [DataType(SqlDbType.Int), AllowNulls(false)]
            public int ShipperID { get; set; }
            [DataType(SqlDbType.NVarChar), DataLength(40), AllowNulls(false)]
            public string CompanyName { get; set; }
            [DataType(SqlDbType.NVarChar), DataLength(24), AllowNulls(true)]
            public string Phone { get; set; }
        }
    }
}
