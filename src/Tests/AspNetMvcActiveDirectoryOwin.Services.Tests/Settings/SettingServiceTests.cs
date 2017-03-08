using System.Collections.Generic;
using System.Linq;
using AspNetMvcActiveDirectoryOwin.Core.Caching;
using AspNetMvcActiveDirectoryOwin.Core.Data;
using AspNetMvcActiveDirectoryOwin.Core.Domain;
using AspNetMvcActiveDirectoryOwin.Services.Settings;
using NSubstitute;
using NUnit.Framework;

namespace AspNetMvcActiveDirectoryOwin.Services.Tests.Settings
{
    [TestFixture]
    class SettingServiceTests
    {
        private Setting _setting1;
        private Setting _setting2;
        private Setting _setting3;
        private Setting _setting4;
        private IQueryable<Setting> _settings;
        private ICacheService _cacheService;
        private IRepository<Setting> _settingRepository;

        [SetUp]
        public void SetUp()
        {
            _setting1 = new Setting
            {
                Id = 1,
                Name = "text",
                Value = "Alpha Bravo Charlie"
            };
            _setting2 = new Setting
            {
                Id = 2,
                Name = "boolean",
                Value = "true"
            };
            _setting3 = new Setting
            {
                Id = 3,
                Name = "integer",
                Value = "123"
            };
            _setting4 = new Setting
            {
                Id = 4,
                Name = "decimal",
                Value = "12.34"
            };
            _settings = new List<Setting>
            {
                _setting1,
                _setting2,
                _setting3,
                _setting4,
            }.AsQueryable();

            _cacheService = new NullCache();

            var mockSet = NSubstituteHelper.CreateMockDbSet(_settings);
            var mockRepository = Substitute.For<IRepository<Setting>>();
            mockRepository.Entities.Returns(mockSet);
            _settingRepository = mockRepository;
        }

        [Test]
        public void GetSettingById_ValidId_Return1Setting()
        {
            var sut = new SettingService(_cacheService, _settingRepository);
            var setting = sut.GetSettingById(2);
            Assert.AreEqual(_setting2, setting);
        }

        [Test]
        public void GetSettingById_InvalidId_ReturnNull()
        {
            var mockSet = NSubstituteHelper.CreateMockDbSet(_settings);
            var mockRepository = Substitute.For<IRepository<Setting>>();
            mockRepository.Entities.Returns(mockSet);

            _settingRepository = mockRepository;

            var sut = new SettingService(_cacheService, _settingRepository);
            var setting = sut.GetSettingById(100);
            Assert.IsNull(setting);
        }

        [Test]
        public void GetSettings_RetrieveAllSettings()
        {
            var sut = new SettingService(_cacheService, _settingRepository);
            var settings = sut.GetAllSettings();
            Assert.AreEqual(4, settings.Count);
            Assert.IsTrue(settings.Contains(_setting1));
            Assert.IsTrue(settings.Contains(_setting2));
            Assert.IsTrue(settings.Contains(_setting3));
            Assert.IsTrue(settings.Contains(_setting4));
        }

        [Test]
        public void GetSettings_ReturnTextValue()
        {
            var sut = new SettingService(_cacheService, _settingRepository);
            var setting = sut.GetSettingByKey("text", "");
            Assert.AreEqual(setting, "Alpha Bravo Charlie");
        }

        [Test]
        public void GetSettings_ReturnBooleanValue()
        {
            var sut = new SettingService(_cacheService, _settingRepository);
            var setting = sut.GetSettingByKey("boolean", "");
            Assert.AreEqual(setting, "true");
        }

        [Test]
        public void GetSettings_ReturnIntegerValue()
        {
            var sut = new SettingService(_cacheService, _settingRepository);
            var setting = sut.GetSettingByKey("integer", "");
            Assert.AreEqual(setting, "123");
        }

        [Test]
        public void GetSettings_ReturnDecimalValue()
        {
            var sut = new SettingService(_cacheService, _settingRepository);
            var setting = sut.GetSettingByKey("decimal", "");
            Assert.AreEqual(setting, "12.34");
        }
    }
}