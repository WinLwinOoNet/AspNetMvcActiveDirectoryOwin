using System;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Data.Tests
{
    [TestFixture]
    public class EmailTemplateTests : PersistenceTest
    {
        [Test]
        public void CanSaveAndLoadEmailTemplate()
        {
            var sut = new EmailTemplate
            {
                Name = "Add New User Notification",
                Subject = "Sample subject",
                Body = "<p>Sample body</p>",
                Description = "Sample description",
                CreatedBy = "Developer",
                CreatedOn = new DateTime(2016, 01, 01),
                ModifiedBy = "Developer",
                ModifiedOn = new DateTime(2016, 01, 01)
            };

            var fromDb = SaveAndLoadEntity(sut);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.Name, "Add New User Notification");
            Assert.AreEqual(fromDb.Subject, "Sample subject");
            Assert.AreEqual(fromDb.Body, "<p>Sample body</p>");
            Assert.AreEqual(fromDb.Description, "Sample description");
            Assert.AreEqual(fromDb.CreatedBy, "Developer");
            Assert.AreEqual(fromDb.CreatedOn, new DateTime(2016, 01, 01));
            Assert.AreEqual(fromDb.ModifiedBy, "Developer");
            Assert.AreEqual(fromDb.ModifiedOn, new DateTime(2016, 01, 01));
        }
    }
}