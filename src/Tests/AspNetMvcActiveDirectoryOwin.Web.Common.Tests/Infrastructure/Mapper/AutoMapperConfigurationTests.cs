using AspNetMvcActiveDirectoryOwin.Web.Common.Mapper;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Web.Common.Tests.Infrastructure.Mapper
{
    [TestFixture]
    public class AutoMapperConfigurationTests
    {
        [Test]
        public void ConfigurationIsValid()
        {
            AutoMapperConfiguration.Initialize();
            AutoMapperConfiguration.MapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
