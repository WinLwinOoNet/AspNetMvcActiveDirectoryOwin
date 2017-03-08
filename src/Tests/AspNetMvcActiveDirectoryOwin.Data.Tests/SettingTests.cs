using AspNetMvcActiveDirectoryOwin.Core.Domain;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Data.Tests
{
    [TestFixture]
    public class SettingTests : PersistenceTest
    {
        [Test]
        public void CanSaveAndLoadSetting()
        {
            var sut = new Setting
            {
                Name = "Sample",
                Value = "Data"
            };

            var fromDb = SaveAndLoadEntity(sut);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.Name, "Sample");
            Assert.AreEqual(fromDb.Value, "Data");
        }
    }
}