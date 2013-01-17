using System;
using System.Data;
using NUnit.Framework;
using SQLRoller.Attributes;
using SQLRoller.Specify;

namespace SQLRoller.UnitTests.VerificationTests.SatisfyTests
{
    [TestFixture]
    public class OnPrimaryKey  : DataBaseTestBase
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
        public void SucceedIfEverythingMatches()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Suppliers>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.True);
        }

        //Wrong field Labeled
        private class Categories
        {
            [DataType(SqlDbType.NVarChar)]
            [PrimaryKey()]
            public string CategoryName { get; set; }
        }

        //Right Everything
        private class Suppliers
        {
            [DataType(SqlDbType.Int)]
            [PrimaryKey()]
            public int SupplierID { get; set; }
        }
    }


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
            dataspec.AddSchema<Suppliers>();
            var database = new Database(ConnectionString);
            Assert.That(database.Satisfies(dataspec), Is.True);
        }

        //Wrong field Labeled
        private class Categories
        {
            [DataType(SqlDbType.NVarChar)]
            [IdentityInt(1,1)]
            public string CategoryName { get; set; }
        }
        //Right Field Wrong Increment
        private class Shippers
        {
            [DataType(SqlDbType.Int)]
            [IdentityInt(1,2)]
            public int ShipperID { get; set; }
        }
        //Right Field Wrong Seed
        private class Employees
        {
            [DataType(SqlDbType.Int)]
            [IdentityInt(2, 1)]
            public int EmployeeID { get; set; }
        }
        //Right Everything
        private class Suppliers
        {
            [DataType(SqlDbType.Int)]
            [IdentityInt(1, 1)]
            public int SupplierID { get; set; }
        }
    }
}