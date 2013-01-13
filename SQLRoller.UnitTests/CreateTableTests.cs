using System;
using FakeItEasy;
using NUnit.Framework;

namespace SQLRoller.UnitTests
{
    [TestFixture]
    public class CreateTableTests
    {
        [Test]
        public void ReleaseShouldCreateCorrectScriptWithOnlyTableName()
        {
            var scopeResolver = A.Fake<IScopeResolver>();
            A.CallTo(() => scopeResolver.Write()).Returns(string.Empty);

            const string someTable = "someTable";
            var ct = new CreateTable(someTable, scopeResolver);
            string releaseScript = ct.GetReleaseSql();
            Assert.That(releaseScript, Is.EquivalentTo(String.Format("CREATE TABLE [{0}]", someTable)));
        }

        [Test]
        public void ReleaseShouldCreateCorrectScriptWithTableNameAndScope()
        {
            const string someTable = "someTable";
            const string someSchema = "[someSchema].";

            var scopeResolver = A.Fake<IScopeResolver>();
            A.CallTo(() => scopeResolver.Write()).Returns(someSchema);

            var ct = new CreateTable(someTable, scopeResolver);
            string releaseScript = ct.GetReleaseSql();
            Assert.That(releaseScript, Is.EquivalentTo(String.Format("CREATE TABLE {1}[{0}]", someTable, someSchema)));
            A.CallTo(() => scopeResolver.Write()).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void ReleaseShouldCreateCorrectScriptWithTableNameAndSchemaNameAndDbName()
        {
            const string someTable = "someTable";
            const string someSchema = "someSchema";
            const string someDb = "someDb";
            var ct = new CreateTable(someTable, someSchema, someDb);
            string releaseScript = ct.GetReleaseSql();
            Assert.That(releaseScript, Is.EqualTo(String.Format("CREATE TABLE [{2}].[{1}].[{0}]", someTable, someSchema, someDb)));
        }
    }

}
