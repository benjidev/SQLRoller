using System;
using NUnit.Framework;

namespace SQLRoller.UnitTests
{
    [TestFixture]
    public class ScopeResolverTests
    {
        [Test]
        public void GivenSchemaShouldRenderSchema()
        {
            const string someSchema = "someSchema";
            var ct = new ScopeResolver(schemaName:someSchema);
            string releaseScript = ct.Write();
            Assert.That(releaseScript, Is.EqualTo(String.Format("[{0}].", someSchema)));
        }

        [Test]
        public void GivenNothingShouldRenderBlank()
        {
            var ct = new ScopeResolver();
            string releaseScript = ct.Write();
            Assert.That(releaseScript, Is.EqualTo(string.Empty));
        }

        [Test]
        public void GivenSchemaAndDbShouldRenderSchemaAndDb()
        {
            const string someSchema = "someSchema";
            const string someDb = "someDb";
            var ct = new ScopeResolver(someDb, someSchema);
            string releaseScript = ct.Write();
            Assert.That(releaseScript, Is.EqualTo(String.Format("[{0}].[{1}].", someDb, someSchema)));
        }

        [Test]
        public void GivenOnlyDbShouldRenderOnlyDbAndLeavePlaceForSchema()
        {
            const string someDb = "someDb";
            var ct = new ScopeResolver(someDb);
            string releaseScript = ct.Write();
            Assert.That(releaseScript, Is.EqualTo(String.Format("[{0}]..", someDb)));
        }


    }
}
