using System;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Data.Tests
{
    [TestFixture]
    public class DomainTests : PersistenceTest
    {
        [Test]
        public void CanSaveAndLoadDomain()
        {
            var sut = new Domain
            {
                Name = "Domain1",
                Description = "Sample domain",
                CreatedBy = "Developer",
                CreatedOn = new DateTime(2016, 01, 01),
                ModifiedBy = "Developer",
                ModifiedOn = new DateTime(2016, 01, 01)
            };

            var fromDb = SaveAndLoadEntity(sut);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.Name, "Domain1");
            Assert.AreEqual(fromDb.Description, "Sample domain");
            Assert.AreEqual(fromDb.CreatedBy, "Developer");
            Assert.AreEqual(fromDb.CreatedOn, new DateTime(2016, 01, 01));
            Assert.AreEqual(fromDb.ModifiedBy, "Developer");
            Assert.AreEqual(fromDb.ModifiedOn, new DateTime(2016, 01, 01));
        }
    }
}