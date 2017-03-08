using System;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Data.Tests
{
    [TestFixture]
    public class LogTests : PersistenceTest
    {
        [Test]
        public void CanSaveAndLoadLog()
        {
            var sut = new Log
            {
                Date = new DateTime(2016, 01, 01),
                Username = "123456",
                Thread = "27",
                Level = "INFO",
                Logger = "MyWebApp.Web.Controllers.MVC.AccountController",
                Message = "'123456' is logged-in.",
                Exception = ""
            };

            var fromDb = SaveAndLoadEntity(sut);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.Date, new DateTime(2016, 01, 01));
            Assert.AreEqual(fromDb.Username, "123456");
            Assert.AreEqual(fromDb.Thread, "27");
            Assert.AreEqual(fromDb.Level, "INFO");
            Assert.AreEqual(fromDb.Logger, "MyWebApp.Web.Controllers.MVC.AccountController");
            Assert.AreEqual(fromDb.Message, "'123456' is logged-in.");
            Assert.AreEqual(fromDb.Exception, "");
        }
    }
}