using System;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Data.Tests
{
    [TestFixture]
    public class TraceLogTests : PersistenceTest
    {
        [Test]
        public void CanSaveAndLoadTraceLog()
        {
            var sut = new TraceLog
            {
                Controller = "Home",
                Action = "Index",
                Message = "{}",
                PerformedOn = new DateTime(2016, 01, 01),
                PerformedBy = "Developer"
            };

            var fromDb = SaveAndLoadEntity(sut);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.Controller, "Home");
            Assert.AreEqual(fromDb.Action, "Index");
            Assert.AreEqual(fromDb.Message, "{}");
            Assert.AreEqual(fromDb.PerformedOn, new DateTime(2016, 01, 01));
            Assert.AreEqual(fromDb.PerformedBy, "Developer");
        }
    }
}