using System;
using NUnit.Framework;
using System.Data;

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

        [Test]
        public void ShouldSatisfyIfAllColumnsPresentInDB()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Region>();
            var database = new Database(connectionString);
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

    [TestFixture]
    public class OnDataTypes
    {
        const string connectionString = "Integrated Security=SSPI;Initial Catalog=NorthWind;Data Source=ASUS-PC";
        [Test]
        public void ShouldNotSatisfyIfColumnDataTypeDoesNotMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(connectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't, due to incorrect DataTypes");
        }

        [Test]
        public void ShouldSatisfyIfColumnDataTypeDoesMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Region>();
            var database = new Database(connectionString);
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

    [TestFixture]
    public class OnDataLength
    {
        const string connectionString = "Integrated Security=SSPI;Initial Catalog=NorthWind;Data Source=ASUS-PC";
        [Test]
        public void ShouldNotSatisfyIfColumnDataLengthDoesNotMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(connectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't, due to incorrect DataLengths");
        }

        [Test]
        public void ShouldSatisfyIfColumnDataLengthDoesMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Shippers>();
            var database = new Database(connectionString);
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

    [TestFixture]
    public class OnAllowNulls
    {
        const string connectionString = "Integrated Security=SSPI;Initial Catalog=NorthWind;Data Source=ASUS-PC";
        [Test]
        public void ShouldNotSatisfyIfColumnAllowNullsDoesNotMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Categories>();
            var database = new Database(connectionString);
            Assert.That(database.Satisfies(dataspec), Is.False, "database satisfied dataspec, when shouldn't, due to incorrect AllowNulls");
        }

        [Test]
        public void ShouldSatisfyIfColumnAllowNullsDoesMatch()
        {
            var dataspec = new DatabaseSpec();
            dataspec.AddSchema<Shippers>();
            var database = new Database(connectionString);
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
