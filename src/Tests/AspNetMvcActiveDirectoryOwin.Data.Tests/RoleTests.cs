using AspNetMvcActiveDirectoryOwin.Core.Domain;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Data.Tests
{
    [TestFixture]
    public class RoleTests : PersistenceTest
    {
        [Test]
        public void CanSaveAndLoadRole()
        {
            var sut = new Role
            {
                Name = "Developer"
            };

            var fromDb = SaveAndLoadEntity(sut);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.Name, "Developer");
        }
    }
}