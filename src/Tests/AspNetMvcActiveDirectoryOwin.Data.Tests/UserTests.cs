using System;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Data.Tests
{
    [TestFixture]
    public class UserTests : PersistenceTest
    {
        [Test]
        public void CanSaveAndLoadUser()
        {
            var sut = new User
            {
                UserName = "johndoe",
                FirstName = "John",
                LastName = "Doe",
                Active = true,
                LastLoginDate = new DateTime(2016, 01, 01),
                CreatedBy = "Developer",
                CreatedOn = new DateTime(2016, 01, 01),
                ModifiedBy = "Developer",
                ModifiedOn = new DateTime(2016, 01, 01)
            };

            var fromDb = SaveAndLoadEntity(sut);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.UserName, "johndoe");
            Assert.AreEqual(fromDb.FirstName, "John");
            Assert.AreEqual(fromDb.LastName, "Doe");
            Assert.AreEqual(fromDb.Active, true);
            Assert.AreEqual(fromDb.LastLoginDate, new DateTime(2016, 01, 01));
            Assert.AreEqual(fromDb.CreatedBy, "Developer");
            Assert.AreEqual(fromDb.CreatedOn, new DateTime(2016, 01, 01));
            Assert.AreEqual(fromDb.ModifiedBy, "Developer");
            Assert.AreEqual(fromDb.ModifiedOn, new DateTime(2016, 01, 01));
        }
    }
}